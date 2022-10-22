using Godot;
using System;

public class HealthBar : Node2D{


    TextureProgress bar;
    float max_health = 0;
    float current_health = 0;

    public override void _Ready(){
        bar = GetNode<TextureProgress>("TextureProgress");
        bar.Visible = false;
    }

    public void SetMaxValue(float value){
        max_health = value;
        current_health = max_health;
        bar.MaxValue = value;
    }

    public void SetValue(float value){
        current_health = value;
        bar.Value = value;
        bar.Visible = true;
    }


}
