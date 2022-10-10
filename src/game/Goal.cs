using Godot;
using System;

public class Goal : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    public Vector2 initial_pos; 

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        initial_pos = this.GlobalPosition;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
