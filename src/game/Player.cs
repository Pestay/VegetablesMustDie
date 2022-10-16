using Godot;
using System;

public class Player : KinematicBody2D
{
    Texture WALK_ANIMATION;
    Texture IDLE_ANIMATION;

    [Export] public int speed = 200;

    public Vector2 velocity = new Vector2();

    Sprite player_sprite;

    bool trapsState;

    PackedScene bulletScene;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        bulletScene = GD.Load<PackedScene>("res://src/entities/Bullet.tscn");
        
        player_sprite = GetNode<Sprite>("Sprite");
        WALK_ANIMATION = ResourceLoader.Load<Texture>("res://src/enemies/enemy_walk.png");
        IDLE_ANIMATION = ResourceLoader.Load<Texture>("res://src/enemies/enemy_idle.png");
    }

    public void IdleAnimation(){
        player_sprite.Texture = IDLE_ANIMATION;
    }

    public void WalkAnimation(){
        player_sprite.Texture = WALK_ANIMATION;
    }

    public void GetInput()
    {
        velocity = new Vector2();

        if (Input.IsActionPressed("right"))
            velocity.x += 1;

        if (Input.IsActionPressed("left"))
            velocity.x -= 1;

        if (Input.IsActionPressed("down"))
            velocity.y += 1;

        if (Input.IsActionPressed("up"))
            velocity.y -= 1;

        velocity = velocity.Normalized() * speed;
    }

    public override void _PhysicsProcess(float delta)
    {
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

}
