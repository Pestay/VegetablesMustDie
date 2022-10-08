using Godot;
using System;

public class FreeViewCamera : Camera2D{


    float speed = 400;
    float zoom_speed = 4;
    public override void _PhysicsProcess(float delta){
        base._PhysicsProcess(delta);
        Move(delta);
        HandleZoom(delta);
    }

    void Move(float delta){
        Vector2 dir = Vector2.Zero;
        if(Input.IsActionPressed("S")){
            dir += Vector2.Down;
        }
        if(Input.IsActionPressed("W")){
            dir += Vector2.Up;
        }
        if(Input.IsActionPressed("A")){
            dir += Vector2.Left;
        }
        if(Input.IsActionPressed("D")){
            dir += Vector2.Right;
        }
        this.GlobalPosition += speed*delta*dir;
    }

    void HandleZoom(float delta){
        int dir = 0;
        if(Input.IsActionJustReleased("scroll_up")){
            dir = -1;
        }
        if(Input.IsActionJustReleased("scroll_down")){
            dir = 1;
        }
        this.Zoom += dir*delta*zoom_speed*Vector2.One;
        
    }
}
