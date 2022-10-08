using Godot;
using System;
using System.Collections.Generic;

public class TestPositions : Node2D{

    public List<Vector2> GetPositions(){
        List<Vector2> positions = new List<Vector2>();
        foreach(Position2D pos in GetChildren()){
            positions.Add(pos.GlobalPosition);
        }
        return positions;
    }
}

