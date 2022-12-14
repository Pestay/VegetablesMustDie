using Godot;
using System;

public class Turret : Trap{

    
    PackedScene bulletScene;

    AudioStreamPlayer2D audioController;

    float reload_speed = 1.0f;
    float last_shot = 0.0f;


    public override void _Ready()
    {
        bulletScene = GD.Load<PackedScene>("res://src/entities/Bullet.tscn");
        audioController = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
        audioController.Stream = GD.Load<AudioStream>("res://src/traps/turret/bubble_1.wav");
        this.GlobalPosition += new Vector2(16,16);
    }

    public override void _PhysicsProcess(float delta)
    {   
        last_shot += delta;
        if(last_shot >= reload_speed)
        {
            Shoot();
            last_shot = 0.0f;
        }

    }

    void Shoot(){
        Vector2 Front = new Vector2(-Mathf.Sin(this.Rotation),Mathf.Cos(this.Rotation));
        Bullet bullet = (Bullet)bulletScene.Instance();
        bullet.GlobalPosition = GlobalPosition + Front*30.0f;
        bullet.Rotation = Front.Angle();
        audioController.Play();
        GetParent().AddChild(bullet);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
