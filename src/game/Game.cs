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

    public override void _Ready(){
        GAME_MAP = GetNode<Map>("Map");
        ENEMIES = GetNode<Enemies>("Enemies");

        // Init nodes
        ENEMIES.Init(GAME_MAP);

        // DEBUG PURPOSES
        PackedScene enemy = Godot.ResourceLoader.Load<PackedScene>("res://src/enemies/Enemy.tscn");
        Wave new_wave = new Wave(Enumerable.Repeat( enemy, 1).ToList(), 0); 
        waves = new WaveConfig( new List<Wave>(){new_wave} );
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        if (Input.IsActionJustPressed("ui_accept")){
            StartWave();
        }
    }


    public void StartWave(){
        GD.Print("Starting wave");
        Wave wave = waves.PopWave();
        int n_gate = wave.spawn_gate;
        Vector2 spawn_pos = GAME_MAP.GetGates()[n_gate];
        ENEMIES.SpawnEnemyGroup(wave.wave_enemies, spawn_pos);
    }

}
