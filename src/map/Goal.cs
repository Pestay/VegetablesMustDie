using Godot;
using System;

public class Goal : Area2D{

    public override void _Ready(){
        
    }

    


    public void _on_Goal_body_entered( Node body ){
        GD.Print("INSIDE");
        if(body.IsInGroup("Enemy")){
            body.QueueFree();
        }

    }



}
