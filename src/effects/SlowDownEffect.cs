using Godot;
using System;


public class SlowDownEffect : PropertyEffect{


    float modifier = 0.5f;


    SlowDownEffect(){
        can_stack = false;
        PropertyType = EffectsManager.PROPERTY_TYPE.Velocity;
        effect_name = "SlowDown";
    }


    public override Vector2 ApplyEffect(Vector2 velocity){
        Vector2 new_velocity = velocity*modifier;
        return new_velocity;
    }



}
