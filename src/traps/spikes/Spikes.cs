using Godot;
using System;

public class Spikes : Trap
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    Spikes(){
        price = 50;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    public void _on_Area2D_body_entered(Node body){
        if(body.IsInGroup("Enemy")){
            Enemy enemy = (Enemy) body;
            enemy.inSpike = true;
            enemy.spikeDmg = 15;
            enemy.dotReload = 1.5f;
            enemy.lastDot = 1.5f;
        }
    }


    public void _on_Area2D_body_exited(Node body){
        if(body.IsInGroup("Enemy")){
            Enemy enemy = (Enemy) body;
            enemy.inSpike = false;
        }
    }
}
