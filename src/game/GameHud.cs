using Godot;
using System;

public class GameHud : CanvasLayer{
    
    Label HP_LABEL;
    Label MONEY_LABEL;

    public override void _Ready(){
        HP_LABEL = GetNode<Label>("Hud/HBoxContainer/HealthPoints/HealthPoints");
        MONEY_LABEL = GetNode<Label>("GameHud/Hud/HBoxContainer/Money/Money");
    }

    public void UpdateHP(int new_hp){
        HP_LABEL.Text = new_hp.ToString();
    }

    public void UpdateMoney(int new_money){
        MONEY_LABEL.Text = new_money.ToString();
    }


}
