using Godot;
using System;
using System.Collections.Generic;
using System.Linq;



// El coordinador total, controla todo el juego, lo inicia y lo termina.
public class Game : Node2D{

    // Children
    Map GAME_MAP;
    Enemies ENEMIES;
    GameHud GAME_HUD;
    AudioStreamPlayer2D AUDIO_CONTROLLER;
    AudioStreamPlayer2D MUSIC_CONTROLLER;

    // Game stats
    int health_points = 5;
    bool is_game_over = false;

    public override void _Ready(){
        GAME_MAP = GetNode<Map>("Map");
        ENEMIES = GetNode<Enemies>("Enemies");
        GAME_HUD = GetNode<GameHud>("GameHud");
        MUSIC_CONTROLLER = GetNode<AudioStreamPlayer2D>("Music");
        MUSIC_CONTROLLER.Stream = GD.Load<AudioStream>("res://src/game/combat.mp3");
        MUSIC_CONTROLLER.Play();
        AUDIO_CONTROLLER = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
        AUDIO_CONTROLLER.Stream = GD.Load<AudioStream>("res://src/game/Alarm_01.wav");
        

        // Init nodes
        ENEMIES.Init(GAME_MAP);

        // DEBUG PURPOSES
        PackedScene enemy = Godot.ResourceLoader.Load<PackedScene>("res://src/enemies/Enemy.tscn");
        WaveStructs.Group enemies1 = new WaveStructs.Group(Enumerable.Repeat( enemy, 2).ToList(), 0); 
        WaveStructs.Group enemies2 = new WaveStructs.Group(Enumerable.Repeat( enemy, 5).ToList(), 0); 
        WaveStructs.Wave waves = new WaveStructs.Wave( new List<WaveStructs.Group>(){enemies1,enemies2} );
        ENEMIES.StartWave(waves);
        GAME_HUD.UpdateHP(health_points);
    }



    public override void _Process(float delta){
        base._Process(delta);
    }


    public void TakeDamage(int dmg){
        if(!is_game_over){
            health_points -= dmg;
            GAME_HUD.UpdateHP(health_points);
            if(health_points <= 0){
                GameOver();   
            }
        }
        
    }



    void _on_Map_EnemyReachGoal(){
        TakeDamage(1);
        AUDIO_CONTROLLER.Play();
    }

    void _on_Enemies_WaveFinished(){
        GD.Print(" FIIN!!");
        if(!is_game_over)
            Win();
    }


    async void GameOver(){
        AUDIO_CONTROLLER.Stream = GD.Load<AudioStream>("res://src/game/lose.mp3");
        AUDIO_CONTROLLER.Play();
        is_game_over = true;
        Control game_over = GetNode<Control>("GameHud/GameOver");
        game_over.Visible = true;
        await ToSignal(GetTree().CreateTimer(3), "timeout");
        GetTree().ChangeScene("res://src/main_menu/Menu.tscn");
    }


    async void Win(){
        AUDIO_CONTROLLER.Stream = GD.Load<AudioStream>("res://src/game/win.mp3");
        AUDIO_CONTROLLER.Play();
        Control win = GetNode<Control>("GameHud/Victory");
        win.Visible = true;
        await ToSignal(GetTree().CreateTimer(3), "timeout");
        GetTree().ChangeScene("res://src/main_menu/Menu.tscn");
    }


}
