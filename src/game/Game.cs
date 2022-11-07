using Godot;
using System;
using System.Collections.Generic;
using System.Linq;


public struct WaveConfig{
    public WaveConfig(List<Wave> enemies){
        wave_enemies = enemies;
    }

    public List<Wave> wave_enemies;
    public Wave PopWave(){
        Wave r_wave = wave_enemies[0];
        wave_enemies.RemoveAt(0);
        return r_wave;
    }
}

public struct Wave{
    public Wave(List<PackedScene> enemies, int gate){
        wave_enemies = enemies;
        spawn_gate =gate;
    }

    public List<PackedScene> wave_enemies { get; }
    public int spawn_gate { get; }
}

// El coordinador total, controla todo el juego, lo inicia y lo termina.
public class Game : Node2D{


    Map GAME_MAP;
    Enemies ENEMIES;
    WaveConfig waves;
    int health_points = 5;
    bool is_game_over = false;

    public override void _Ready(){
        GAME_MAP = GetNode<Map>("Map");
        ENEMIES = GetNode<Enemies>("Enemies");

        // Init nodes
        ENEMIES.Init(GAME_MAP);

        // DEBUG PURPOSES
        PackedScene enemy = Godot.ResourceLoader.Load<PackedScene>("res://src/enemies/Enemy.tscn");
        Wave new_wave = new Wave(Enumerable.Repeat( enemy, 10).ToList(), 0); 
        waves = new WaveConfig( new List<Wave>(){new_wave} );

        StartWave();
        UpdateHP(health_points);
    }

    public override void _Process(float delta){
        base._Process(delta);
    }


    public void StartWave(){
        GD.Print("Starting wave");
        Wave wave = waves.PopWave();
        int n_gate = wave.spawn_gate;
        Vector2 spawn_pos = GAME_MAP.GetGates()[n_gate];
        ENEMIES.SpawnEnemyGroup(wave.wave_enemies, spawn_pos);
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



    async void GameOver(){
        is_game_over = true;
        Control game_over = GetNode<Control>("GameHud/GameOver");
        game_over.Visible = true;
        await ToSignal(GetTree().CreateTimer(3), "timeout");
        GetTree().ChangeScene("res://src/main_menu/Menu.tscn");
    }

}
