using Godot;
using System;

public class Death : AudioStreamPlayer2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Stream = GD.Load<AudioStream>("res://src/traps/1x1block/destroy.wav");
        Play();
    }

    public void _on_Timer_timeout()
    {
        QueueFree();
    }
}
