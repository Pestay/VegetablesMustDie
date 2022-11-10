using Godot;
using System;


public class SlowDownEffect : PropertyEffect{

    
    float modifier = 0.5f;
    Timer TIMER;


    SlowDownEffect(){
        can_stack = false;
        PropertyType = EffectsManager.PROPERTY_TYPE.Velocity;
        effect_name = "SlowDown";
    }


    public override void _Ready(){
        TIMER = GetNode<Timer>("Timer");
        TIMER.Start();
    }


    public override Vector2 ApplyEffect(Vector2 velocity){
        Vector2 new_velocity = velocity*modifier;
        return new_velocity;
    }



}
