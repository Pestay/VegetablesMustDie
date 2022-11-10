using Godot;
using System;
using System.Collections.Generic;

public class DetergentFloor : Trap{


    PackedScene effect;

    Dictionary<Enemy, SlowDownEffect> effect_by_enemy = new Dictionary<Enemy, SlowDownEffect>();

    public override void _Ready(){
        effect = ResourceLoader.Load<PackedScene>("res://src/effects/SlowDownEffect.tscn");
    }
    

    void _on_Detector_body_entered(Node2D body){
        if(body.IsInGroup("Enemy")){
            Enemy enemy = (Enemy) body;

            if(!effect_by_enemy.ContainsKey(enemy)){

                SlowDownEffect new_effect = effect.Instance<SlowDownEffect>();
                enemy.AddPropertyEffect(new_effect);
                effect_by_enemy[enemy] = new_effect;
            }
        }
    }


    void _on_Detector_body_exited(Node2D body){
        if(body.IsInGroup("Enemy")){
            Enemy enemy = (Enemy) body;

            if(effect_by_enemy.ContainsKey(enemy)){
                enemy.RemovePropertyEffect(effect_by_enemy[enemy]);
                effect_by_enemy.Remove(enemy);
            }
        }

    }


}
