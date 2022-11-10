using Godot;
using System;
using System.Collections.Generic;

public class DetergentFloor : Area2D{


    PackedScene effect;
    List<Enemy> enemies = new List<Enemy>();
    Dictionary<Enemy, SlowDownEffect> effect_by_enemy = new Dictionary<Enemy, SlowDownEffect>();

    public override void _Ready(){
        effect = ResourceLoader.Load<PackedScene>("res://src/effects/SlowDownEffect.tscn");
    }
    

    void _on_DetergentFloor_body_entered(Node2D body){
        if(body.IsInGroup("Enemy")){
            Enemy enemy = (Enemy) body;
            enemy.AddPropertyEffect(effect);
            if(!enemies.Contains(enemy)){
                enemies.Add(enemy);
            }
        }
    }




}
