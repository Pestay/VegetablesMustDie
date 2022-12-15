using Godot;
using System;

public class BloodSplatter : Particles2D
{

    AudioStreamPlayer2D AUDIO_CONTROLLER;

    public override void _Ready()
    {
        Emitting = true;
        AUDIO_CONTROLLER = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
        AUDIO_CONTROLLER.Stream = GD.Load<AudioStream>("res://src/enemies/plant_death.wav");

    }
    

    public void _on_Timer_timeout()
    {
        AUDIO_CONTROLLER.Play();
        SetProcess(false);
        SetPhysicsProcess(false);
        SetProcessInput(false);
        SetProcessInternal(false);
        SetProcessUnhandledInput(false);
        SetProcessUnhandledKeyInput(false);
        SpeedScale = 0;
    }
}
