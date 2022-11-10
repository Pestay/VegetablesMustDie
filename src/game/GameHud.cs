using Godot;
using System;

public class GameHud : CanvasLayer{
    
    Label HP_LABEL;
    public override void _Ready(){
        HP_LABEL = GetNode<Label>("Hud/HBoxContainer/HealthPoints/HealthPoints");
    }

    public void UpdateHP(int new_hp){
        HP_LABEL.Text = new_hp.ToString();
    }


}
