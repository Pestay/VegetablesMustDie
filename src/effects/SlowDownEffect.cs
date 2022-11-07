using Godot;
using System;


public class SlowDownEffect : PropertyEffect{


    float effect_time = 5.0f;// In seconds
    float modifier = 0.5f;
    Timer TIMER;

    public override void _Ready(){
        TIMER = GetNode<Timer>("Timer");
        TIMER.Start();
    }

    public override Vector2 ApplyEffect(Vector2 velocity){
        Vector2 new_velocity = velocity*modifier;
        return new_velocity;
    }
    



}
