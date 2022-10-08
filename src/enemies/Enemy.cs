using Godot;
using System;
using System.Collections.Generic;

// Enemy for testing purposes
public class Enemy : KinematicBody2D{


    Texture WALK_ANIMATION;
    Texture IDLE_ANIMATION;

    [Export]
    List<Vector2> current_path = new List<Vector2>();

    Vector2 velocity = Vector2.Zero;
    float max_speed = 12000;
    Sprite enemy_sprite;

    EnemyFSM brain;

    public override void _Ready(){
        enemy_sprite = GetNode<Sprite>("Sprite");
        brain = GetNode<EnemyFSM>("EnemyFSM");

        WALK_ANIMATION = ResourceLoader.Load<Texture>("res://src/enemies/enemy_walk.png");
        IDLE_ANIMATION = ResourceLoader.Load<Texture>("res://src/enemies/enemy_idle.png");

        // DEBUG
        TestPositions test_path = GetParent().GetParent().GetNode<TestPositions>("TestPositions");
        SetPath(test_path.GetPositions());
    }

    public override void _PhysicsProcess(float delta){
        base._PhysicsProcess(delta);
        brain.UpdateFSM(delta);
    }

     // Constantly move linearly to the position
    void MoveTo(float delta, Vector2 destination){
        Vector2 to_pos = (destination - this.GlobalPosition).Normalized();
        Vector2 new_velocity = to_pos*delta*max_speed;
        velocity = new_velocity;
        velocity = this.MoveAndSlide(velocity);
    }

    // --- Actions


    // Follow a set of points
    public void FollowPath(float delta){
        if(current_path.Count <= 0){
            return;
        }
        Vector2 destination = current_path[0];
        MoveTo(delta, destination);
        if(destination.DistanceSquaredTo(this.GlobalPosition) < 256){
            current_path.RemoveAt(0);
        }
    }


    public void IdleAnimation(){
        enemy_sprite.Texture = IDLE_ANIMATION;
    }

    public void WalkAnimation(){
        enemy_sprite.Texture = WALK_ANIMATION;
    }
    

    // --- Checkers (Transitions)
    public bool HasReachTarget(){
        if(current_path.Count <= 0){
            return true;
        }
        return false;
    }

    // Setters and Getters

    public void SetPath(List<Vector2> new_path){
        current_path = new_path;
    }

}
