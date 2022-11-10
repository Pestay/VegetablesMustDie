using Godot;
using System;

public class Turret : Trap{

    
    PackedScene bulletScene;

    float reload_speed = 1.0f;
    float last_shot = 0.0f;


    public override void _Ready()
    {
        bulletScene = GD.Load<PackedScene>("res://src/entities/Bullet.tscn");
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
        Vector2 Front = new Vector2(0,1);
        Bullet bullet = (Bullet)bulletScene.Instance();
        bullet.GlobalPosition = GlobalPosition + new Vector2(16,16);
        bullet.Rotation = Front.Angle();
        GetParent().AddChild(bullet);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
