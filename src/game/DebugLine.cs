using Godot;
using System;
using System.Collections.Generic;

public class DebugLine : Line2D
{


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Enemy target = GetTree().Root.GetNode("Game").GetNode("Enemies").GetNode<Enemy>("Enemy");
        drawLine(target.best_path);
    }

    private void drawLine(List<Vector2> path)
    {
        for(int i = 0; i < path.Count; i++)
        {
            this.AddPoint(path[i]);
        }
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
