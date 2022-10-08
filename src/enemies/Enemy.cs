using Godot;
using System;


// Enemy for testing purposes
public class Enemy : KinematicBody2D{


    Texture WALK_ANIMATION;
    Texture IDLE_ANIMATION;

    Vector2 velocity = Vector2.Zero;
    float max_speed = 4000;
    Sprite enemy_sprite;

    EnemyFSM brain;


    [Export]
    public Vector2 test_target = Vector2.Zero;


    public override void _Ready(){
        enemy_sprite = GetNode<Sprite>("Sprite");
        brain = GetNode<EnemyFSM>("EnemyFSM");

        WALK_ANIMATION = ResourceLoader.Load<Texture>("res://src/enemies/enemy_walk.png");
        IDLE_ANIMATION = ResourceLoader.Load<Texture>("res://src/enemies/enemy_idle.png");
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        brain.UpdateFSM(delta);
    }

    void MoveTo(float delta, Vector2 destination){
        Vector2 to_pos = (destination - this.GlobalPosition).Normalized();
        Vector2 new_velocity = to_pos*delta*max_speed;
        velocity = new_velocity;
        velocity = this.MoveAndSlide(velocity);
    }

    // --- Actions

    // Constantly move linearly to the position



    public void MoveToCurrentTarget(float delta){
        MoveTo(delta, test_target);
    }


    public void IdleAnimation(){
        enemy_sprite.Texture = IDLE_ANIMATION;
    }

    public void WalkAnimation(){
        enemy_sprite.Texture = WALK_ANIMATION;
    }
    

    // --- Checkers (Transitions)
    public bool HasReachTarget(){
        if(test_target.DistanceSquaredTo(this.Position) < 400.0f){
            return true;
        }
        return false;
    }


}
