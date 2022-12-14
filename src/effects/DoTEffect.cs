using Godot;
using System;

public class DoTEffect : PropertyEffect
{
    float interval = 0.5f;


    DoTEffect(){
        can_stack = false;
        PropertyType = EffectsManager.PROPERTY_TYPE.Health;
        effect_name = "DoT";
    }


    public override float ApplyEffect(float damage){
        float n_damage = damage;
        return n_damage;
    }

}
