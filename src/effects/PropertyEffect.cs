using Godot;
using System;
using System.Collections.Generic;

public class PropertyEffect : Node{
    

    public override void _Ready(){
        
    }

    public virtual Vector2 ApplyEffect(Vector2 property){
        return property;
    }

    public virtual int ApplyEffect(int property){
        return property;
    }



}
