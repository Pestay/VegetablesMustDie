using Godot;
using System;

public class Coin : Particles2D
{
    Label VALUE;
    public String enemy_value;
    public override void _Ready()
    {
        VALUE = GetNode<Label>("Value");
        VALUE.Text = enemy_value;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
    public void _on_Timer_timeout()
    {
        QueueFree();
    }
}
