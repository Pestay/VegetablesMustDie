using Godot;
using System;
using System.Collections.Generic;

public class EnemyWalkState : Node{

    AnimationPlayer ANIMATIONS;
    Enemy parent;
    Enemy.EnvironmentObservation selected_cell;
    Vector2 current_destination = new Vector2(Int32.MaxValue, Int32.MaxValue);


    public void Constructor(Enemy _parent, AnimationPlayer _animations){
        parent = _parent;
        ANIMATIONS = _animations;
    }


    public override void _Ready(){
        OnEnterState();
    }

    void OnEnterState(){
        current_destination = parent.GlobalPosition;
        WalkAnimation();
    }

    public void RemoveState(){
        QueueFree();
    }


    public override void _Process(float delta){
        Walk(delta);
    }
    

    void WalkAnimation(){
        if(ANIMATIONS.CurrentAnimation != "walk")
            ANIMATIONS.Play("walk");
    }


    public void Walk(float delta){
        // If is near to the destination
        if(current_destination.DistanceSquaredTo( parent.GlobalPosition ) < 32){ // < 8*8 (1/4 of tile)
            Vector2 next_pos = parent.GlobalPosition;
            int min_value = Int32.MaxValue;
            Enemy.EnvironmentObservation cell = new Enemy.EnvironmentObservation();
            List< Enemy.EnvironmentObservation > observations = parent.GetObservations();
            foreach(Enemy.EnvironmentObservation observation in observations){
                if(observation.value < min_value){
                    next_pos = observation.global_pos;
                    min_value = observation.value;
                    cell = observation;
                }
            }
            selected_cell = cell;
            parent.has_obstacle = selected_cell.is_obstacle;
            parent.obstacle_pos = selected_cell.global_pos;
            current_destination = next_pos;
        }
        parent.MoveTo(delta, current_destination);
    }


    
}
