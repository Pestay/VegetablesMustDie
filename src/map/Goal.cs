using Godot;
using System;

public class Goal : Area2D{

    [Signal]
    delegate void EnemyReachGoal();

    public void _on_Goal_body_entered( Node body ){
        if(body.IsInGroup("Enemy")){
            Enemy enemy = (Enemy) body;
            EmitSignal( nameof(EnemyReachGoal) );
            //body.TakeDamage(10000);
            enemy.Die();
        }

    }



}
