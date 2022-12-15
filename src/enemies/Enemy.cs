using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;


// Enemy for testing purposes
public class Enemy : KinematicBody2D{

    AnimationPlayer ANIMATIONS;
    AnimationPlayer EFFECTS;
    HealthBar HEALTH_BAR;
    EffectsManager EFFECTS_MANAGER;
    PackedScene BLOOD_SPLATTER;
    PackedScene COIN;
    List<PathFindingCell> current_path = new List<PathFindingCell>();
    
    Vector2 velocity = Vector2.Zero;
    float max_speed = 50;
    public float damage = 20.0f;
    Sprite enemy_sprite;
    float health = 100;
    int enemy_value = 100;
    EnemyFSM brain;
    public Map enviroment;

    public bool inSpike = false;
    public float spikeDmg;
    public float dotReload;
    public float lastDot;

    [Signal]
    delegate void Dead(Enemy self);



    public override void _Ready(){
        enemy_sprite = GetNode<Sprite>("Sprite");
        brain = GetNode<EnemyFSM>("EnemyFSM");
        ANIMATIONS = GetNode<AnimationPlayer>("AnimationPlayer");
        EFFECTS = GetNode<AnimationPlayer>("Effects");
        EFFECTS_MANAGER =  GetNode<EffectsManager>("EffectsManager");
        HEALTH_BAR = GetNode<HealthBar>("HealthBar");
        HEALTH_BAR.SetMaxValue(health);
        COIN = GD.Load<PackedScene>("res://src/effects/Coin.tscn");
        BLOOD_SPLATTER = GD.Load<PackedScene>("res://src/effects/BloodSplatter.tscn");
        //Debug
        
    }

    void SetPointsLinedebug(){
        PathDebuger line_debug = GetNode<PathDebuger>("PathDebuger");
        line_debug.ClearPoints();
        foreach(PathFindingCell cell in current_path){
            line_debug.AddPoint(cell.coord);
        }
    }


    public override void _PhysicsProcess(float delta){
        base._PhysicsProcess(delta);
        brain.UpdateFSM(delta);
        if(inSpike) {
            lastDot += delta;
            if(lastDot >= dotReload)
            {
                TakeDamage(spikeDmg);
                lastDot = 0.0f;
            }
        }
    }

     // Constantly move linearly to the position
    void MoveTo(float delta, Vector2 destination){
        Vector2 to_pos = (destination - this.GlobalPosition).Normalized();
        Vector2 new_velocity = to_pos*max_speed;
        velocity = new_velocity;
        // Apply effects
        velocity = EFFECTS_MANAGER.ApplyEffectsForProperty(EffectsManager.PROPERTY_TYPE.Velocity, velocity);
        velocity = this.MoveAndSlide(velocity);
    }

    // --- Actions
    struct EnvironmentObservation{
        public Vector2 global_pos;
        public int value;
        public bool is_obstacle;
    }

    // Return what the agent see in the current position
    List<EnvironmentObservation> GetObservations(){
        List<Map.Coord> neighbours = enviroment.GetNeighboursFromGlobal(this.GlobalPosition);
        List<EnvironmentObservation> observations = new List<EnvironmentObservation>();
        Map.Coord pivot = enviroment.GlobalToCoord(this.GlobalPosition);
        foreach(Map.Coord neighbour in neighbours){
            
            EnvironmentObservation new_observation = new EnvironmentObservation();
            new_observation.global_pos = enviroment.CoordToGlobal(neighbour); 
            new_observation.is_obstacle = enviroment.CoordIsObstacle(neighbour);
            new_observation.value = enviroment.GetCoordFlowValue(neighbour);
            observations.Add(new_observation);
            
        }
        
        return observations;
    }

    EnvironmentObservation selected_cell;
    public Vector2 current_destination = new Vector2(Int32.MaxValue, Int32.MaxValue);
    // Moves to goal following the current flow field map
    public void MoveToGoal(float delta){
        // If is near to the destination
        if(current_destination.DistanceSquaredTo(this.GlobalPosition) < 32){ // < 8*8 (1/4 of tile)
            Vector2 next_pos = this.GlobalPosition;
            int min_value = Int32.MaxValue;
            EnvironmentObservation cell = new EnvironmentObservation();
            List<EnvironmentObservation> observations = GetObservations();
            
            foreach(EnvironmentObservation observation in observations){
                
                if(observation.value < min_value){
                    next_pos = observation.global_pos;
                    min_value = observation.value;
                    cell = observation;
                }
                
            }
            //
            selected_cell = cell;
            current_destination = next_pos;
            
        }
        MoveTo(delta, current_destination);
    }
    

    // Follow a set of points
    public void FollowPath(float delta){
        if(current_path.Count <= 0){
            return;
        }
        Vector2 destination = current_path[0].coord;
        MoveTo(delta, destination);
        //SetPointsLinedebug();
        
        if(destination.DistanceSquaredTo(this.GlobalPosition) < 64){ // < 8*8 (1/4 of tile)
            current_path.RemoveAt(0);
            
        }
    }


    public bool SelectedCellIsObstacle() => selected_cell.is_obstacle;

    // Animations

    public void IdleAnimation(){
        if(ANIMATIONS.IsPlaying())
            ANIMATIONS.Stop();
        enemy_sprite.Frame = 0;
    }

    public void WalkAnimation(){
        if(!ANIMATIONS.IsPlaying())
            ANIMATIONS.Play("walk");
    }

    public void AttackAnimation(){
        if(!ANIMATIONS.IsPlaying())
            ANIMATIONS.Play("attack");
    }

    // Enemy attack the current cell
    
    public WoodenBlock1x1 GetObstacle(){
        Map.Coord coord = enviroment.GlobalToCoord(selected_cell.global_pos);
        return enviroment.GetObstacleAtCoord(coord);
    }
    

    // --- Checkers (Transitions)
    public bool HasReachTarget(){
        if(current_path.Count <= 0){
            return true;
        }
        return false;
    }


    public bool IsBlocked(){
        if(current_path.Count > 0){
            if( (current_path[0].has_obstacle ) ){
                return true;
            }
        }
        return false;
    }


    public void TakeDamage(float dmg){
        health -= dmg;
        HEALTH_BAR.SetValue(health);
        EFFECTS.Play("TakeDamage");
        if(health <= 0){
            BloodSplatter blood_instance = (BloodSplatter)BLOOD_SPLATTER.Instance();
            GetTree().CurrentScene.AddChild(blood_instance);
            blood_instance.GlobalPosition = GlobalPosition;
            Coin coin_instance = (Coin)COIN.Instance();
            coin_instance.enemy_value = enemy_value.ToString();
            GetTree().CurrentScene.AddChild(coin_instance);
            coin_instance.GlobalPosition = GlobalPosition;
            Die();
        }
        
    }

    // Setters and Getters
    public void SetPath(List<PathFindingCell> new_path){
        current_path = new_path;
    }

    public void Die(){
        EmitSignal(nameof(Dead), this);
        QueueFree();
    }

    public void AddPropertyEffect(PropertyEffect effect){
        EFFECTS_MANAGER.AddPropertyEffect(effect);
    }

    public void RemovePropertyEffect(PropertyEffect effect){
        EFFECTS_MANAGER.RemovePropertyEffect(effect);
    }


}

