using Godot;
using System;

public class BloodSplatter : Particles2D
{
    public override void _Ready()
    {
        Emitting = true;
    }
    

    public void _on_Timer_timeout()
    {
        SetProcess(false);
        SetPhysicsProcess(false);
        SetProcessInput(false);
        SetProcessInternal(false);
        SetProcessUnhandledInput(false);
        SetProcessUnhandledKeyInput(false);
        SpeedScale = 0;
    }
}
