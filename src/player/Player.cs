using Godot;
using System;

public class Player : KinematicBody2D{


    AnimationPlayer ANIMATIONS;
    PlayerFSM BRAIN;

    [Export] public int speed = 200;

    public Vector2 velocity = new Vector2();

    Sprite PLAYER_SPRITE;

    bool trapsState;

    PackedScene bulletScene;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        bulletScene = GD.Load<PackedScene>("res://src/entities/Bullet.tscn");
        
        PLAYER_SPRITE = GetNode<Sprite>("Sprite");

        ANIMATIONS = GetNode<AnimationPlayer>("AnimationPlayer");
        BRAIN = GetNode<PlayerFSM>("PlayerFSM");
    }

    public void IdleAnimation(){
        if(ANIMATIONS.IsPlaying() ){
            ANIMATIONS.Stop();
        }
        ANIMATIONS.Play("Idle");
    }

    public void WalkAnimation(){
        if(ANIMATIONS.IsPlaying()){
            ANIMATIONS.Stop();
        }
        ANIMATIONS.Play("Walk");
    }

    
    public void GetInput(){
        velocity = new Vector2();

        if (Input.IsActionPressed("right")){
            velocity.x += 1;
            PLAYER_SPRITE.FlipH = false;
        }
            

        if (Input.IsActionPressed("left")){
            velocity.x -= 1;
            PLAYER_SPRITE.FlipH = true;
        }
            

        if (Input.IsActionPressed("down"))
            velocity.y += 1;

        if (Input.IsActionPressed("up"))
            velocity.y -= 1;

        velocity = velocity.Normalized() * speed;
    }

    public override void _PhysicsProcess(float delta)
    {
        BRAIN.UpdateFSM();
        GetInput();
        trapsState = GetTree().Root.GetNode<Traps>("Game/Traps").in_building;
        velocity = MoveAndSlide(velocity);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if(@event is InputEventMouseButton mouseEvent && !trapsState) 
        {
            if (mouseEvent.ButtonIndex == (int)ButtonList.Left && mouseEvent.Pressed)
            {
                Vector2 CursorPos = GetLocalMousePosition();
                Bullet bullet = (Bullet)bulletScene.Instance();
                bullet.Position = Position;
                bullet.Rotation = CursorPos.Angle();
                GetParent().AddChild(bullet);
                GetTree().SetInputAsHandled();
            }
        }
    }

    public Vector2 GetVelocity() => velocity;

}
