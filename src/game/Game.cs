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
    Timer TIMER_BETWEEN_WAVES;
    int time_left = 20;

    // Game stats
    int health_points = 5;
    bool is_game_over = false;

    List<WaveStructs.Wave> total_waves = new List<WaveStructs.Wave>();

    public override void _Ready(){
        GAME_MAP = GetNode<Map>("Map");
        ENEMIES = GetNode<Enemies>("Enemies");
        GAME_HUD = GetNode<GameHud>("GameHud");
        TIMER_BETWEEN_WAVES = GetNode<Timer>("TimeBetweenWaves");
        // Init nodes
        ENEMIES.Init(GAME_MAP);

        // DEBUG PURPOSES
        PackedScene enemy = Godot.ResourceLoader.Load<PackedScene>("res://src/enemies/Enemy.tscn");
        // FIRST WAVE
        WaveStructs.Group enemies1 = new WaveStructs.Group(Enumerable.Repeat( enemy, 2).ToList(), 0); 
        WaveStructs.Group enemies2 = new WaveStructs.Group(Enumerable.Repeat( enemy, 5).ToList(), 0); 
        WaveStructs.Wave wave1 = new WaveStructs.Wave( new List<WaveStructs.Group>(){enemies1,enemies2} );
        total_waves.Add(wave1);
        // SECOND WAVE
        WaveStructs.Group enemies21 = new WaveStructs.Group(Enumerable.Repeat( enemy, 5).ToList(), 0); 
        WaveStructs.Group enemies22 = new WaveStructs.Group(Enumerable.Repeat( enemy, 10).ToList(), 0); 
        WaveStructs.Group enemies23 = new WaveStructs.Group(Enumerable.Repeat( enemy, 10).ToList(), 0); 
        WaveStructs.Wave wave2 = new WaveStructs.Wave( new List<WaveStructs.Group>(){enemies21,enemies22,enemies23} );
        total_waves.Add(wave2);

        //StartNextWave();
        StartTimeBetweenWaves();
        GAME_HUD.UpdateHP(health_points);
    }

    void StartNextWave(){
        if(total_waves.Count > 0){
            WaveStructs.Wave next_wave = total_waves[0];
            total_waves.RemoveAt(0);
            ENEMIES.StartWave(next_wave);
        }
        else{
            if(!is_game_over)
                Win();
        }
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
    }

    void _on_Enemies_WaveFinished(){
        //StartNextWave();
        StartTimeBetweenWaves();
        /*
        if(!is_game_over)
            Win();
        */
    }

    void StartTimeBetweenWaves(){

        time_left = 5;
        TIMER_BETWEEN_WAVES.Start();
        
        
    }

    public void _on_TimeBetweenWaves_timeout(){
        time_left -= 1;
        GAME_HUD.UpdateTimeLeft(time_left);
        if(time_left <= 0){
            TIMER_BETWEEN_WAVES.Stop();
            StartNextWave();
        }
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
