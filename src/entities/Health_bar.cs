using Godot;
using System;

public class Health_bar : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    TextureProgress bar;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        bar = GetNode<TextureProgress>("TextureProgress");
    }

    public void setMaxValue(float value)
    {
        bar.MaxValue = value;
    }

    public void setValue(float value)
    {
        bar.Value = value;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
