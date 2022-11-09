using Godot;
using System;
using System.Collections.Generic;
using System.Linq;



// El coordinador total, controla todo el juego, lo inicia y lo termina.
public class Game : Node2D{

    // Children
    Map GAME_MAP;
    Enemies ENEMIES;

    // Game stats
    int health_points = 5;
    bool is_game_over = false;

    public override void _Ready(){
        GAME_MAP = GetNode<Map>("Map");
        ENEMIES = GetNode<Enemies>("Enemies");

        // Init nodes
        ENEMIES.Init(GAME_MAP);

        // DEBUG PURPOSES
        PackedScene enemy = Godot.ResourceLoader.Load<PackedScene>("res://src/enemies/Enemy.tscn");
        WaveStructs.Group enemies1 = new WaveStructs.Group(Enumerable.Repeat( enemy, 2).ToList(), 0); 
        WaveStructs.Group enemies2 = new WaveStructs.Group(Enumerable.Repeat( enemy, 5).ToList(), 0); 
        WaveStructs.Wave waves = new WaveStructs.Wave( new List<WaveStructs.Group>(){enemies1,enemies2} );
        ENEMIES.StartWave(waves);


        UpdateHP(health_points);
    }



    public override void _Process(float delta){
        base._Process(delta);
    }


    public void TakeDamage(int dmg){
        if(!is_game_over){
            UpdateHP(health_points - dmg);
            if(health_points <= 0){
                GameOver();   
            }
        }
        
    }

    void UpdateHP(int new_hp){
        health_points = new_hp;
        Label label = GetNode<Label>("GameHud/Control/HBoxContainer/HealthPoints");
        label.Text = health_points.ToString();
        
    }

    void _on_Map_EnemyReachGoal(){
        TakeDamage(1);
    }

    void _on_Enemies_WaveFinished(){
        Win();
    }


    async void GameOver(){
        is_game_over = true;
        Control game_over = GetNode<Control>("GameHud/GameOver");
        game_over.Visible = true;
        await ToSignal(GetTree().CreateTimer(3), "timeout");
        GetTree().ChangeScene("res://src/main_menu/Menu.tscn");
    }


    async void Win(){
        Control win = GetNode<Control>("GameHud/Victory");
        win.Visible = true;
        await ToSignal(GetTree().CreateTimer(3), "timeout");
        GetTree().ChangeScene("res://src/main_menu/Menu.tscn");
    }


}
