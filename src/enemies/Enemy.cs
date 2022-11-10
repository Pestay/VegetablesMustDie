using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// Enemy for testing purposes
public class Enemy : KinematicBody2D{

    AnimationPlayer ANIMATIONS;
    HealthBar HEALTH_BAR;
    EffectsManager EFFECTS_MANAGER;
    List<PathFindingCell> current_path = new List<PathFindingCell>();
    
    Vector2 velocity = Vector2.Zero;
    float max_speed = 50;
    Sprite enemy_sprite;
    float health = 100;
    EnemyFSM brain;

    [Signal]
    delegate void Dead(Enemy self);



    public override void _Ready(){
        enemy_sprite = GetNode<Sprite>("Sprite");
        brain = GetNode<EnemyFSM>("EnemyFSM");
        ANIMATIONS = GetNode<AnimationPlayer>("AnimationPlayer");
        EFFECTS_MANAGER =  GetNode<EffectsManager>("EffectsManager");
        HEALTH_BAR = GetNode<HealthBar>("HealthBar");
        HEALTH_BAR.SetMaxValue(health);
    
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
        if(health <= 0){
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

