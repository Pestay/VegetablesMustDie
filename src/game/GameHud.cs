using Godot;
using System;

public class GameHud : CanvasLayer{
    
    Label HP_LABEL;
    Label MONEY_LABEL;
    Label ENEMIES_LEFT;
    Label TIME_LEFT;

    public override void _Ready(){
        HP_LABEL = GetNode<Label>("Hud/HBoxContainer/HealthPoints/HealthPoints");
        MONEY_LABEL = GetNode<Label>("Hud/HBoxContainer/Money/Money");
        ENEMIES_LEFT = GetNode<Label>("Hud/EnemiesLeft");
        TIME_LEFT = GetNode<Label>("Hud/TimeLeft");
    }

    public void UpdateHP(int new_hp){
        HP_LABEL.Text = new_hp.ToString();
    }

    public void UpdateMoney(int new_money){
        MONEY_LABEL.Text = new_money.ToString();
    }

    public void UpdateEnemiesLeft(int new_enemies){
        ENEMIES_LEFT.Visible = true;
        TIME_LEFT.Visible = false;
        if(new_enemies >= 0){
            ENEMIES_LEFT.Text = "Enemies Left: " + new_enemies.ToString();
        }
    }

    public void UpdateTimeLeft(int new_time){
        ENEMIES_LEFT.Visible = false;
        TIME_LEFT.Visible = true;
        TIME_LEFT.Text = new_time.ToString() + " for the next wave";
    }


}
