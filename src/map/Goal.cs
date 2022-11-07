using Godot;
using System;

public class Goal : Area2D{

    [Signal]
    delegate void EnemyReachGoal();

    public void _on_Goal_body_entered( Node body ){
        if(body.IsInGroup("Enemy")){
            EmitSignal( nameof(EnemyReachGoal) );
            body.QueueFree();
        }

    }



}
