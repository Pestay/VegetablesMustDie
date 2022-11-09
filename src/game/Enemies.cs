using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

// Enemy coordinator, spawn etc...
public class Enemies : Node2D{


    List<PackedScene> enemies_group;
    Map GAME_MAP;
    WaveStructs.Wave current_wave;
    List<Enemy> current_enemies = new List<Enemy>(); //list of enemies in node


    public void Init(Map game_map){
        GAME_MAP = game_map;
    }

    Node2D SpawnEnemy(Vector2 spawn_pos, PackedScene enemy){
        Node2D new_enemy = enemy.Instance<Node2D>();
        new_enemy.GlobalPosition = spawn_pos;
        AddChild(new_enemy);
        return new_enemy;
    }

    // Spawn group of enemies at given pos
    async void SpawnEnemies(List<PackedScene> enemies, Vector2 initial_pos){
        foreach(PackedScene enemy in enemies){
            Enemy new_enemy = (Enemy) SpawnEnemy(initial_pos ,enemy);
            current_enemies.Add(new_enemy);
            new_enemy.Connect("Dead", this , nameof(_onEnemyDie));
            new_enemy.SetPath( GAME_MAP.GetPathToGoal(new_enemy.GlobalPosition) );
            await ToSignal(GetTree().CreateTimer(0.2f), "timeout");
        }
    }


    public void _OnMapUpdate(){
        GD.Print("Map update");
        foreach(Enemy enemy in current_enemies){
            enemy.SetPath( GAME_MAP.GetPathToGoal( enemy.GlobalPosition ) );
        }
    }


    public void _on_Map_MapUpdate() {
        GAME_MAP.SetFlowMap();
        foreach(Enemy enemy in current_enemies){
            //enemy.SetPath(GAME_MAP.GetPathFromFlowMap(enemy.GlobalPosition));
            enemy.SetPath( GAME_MAP.GetPathToGoal(enemy.GlobalPosition) );
        }
    }
    

    void _onEnemyDie(Enemy enemy){
        // Delete enemy from list
        current_enemies.Remove(enemy);
        GD.Print(current_enemies.Count());
    }


    // Init new wave
    public void StartWave(WaveStructs.Wave wave){
        // Delete current enemies
        GD.Print("Starting new wave");
        current_wave = wave;
        SpawnNextGroup( current_wave );
    }


    void SpawnNextGroup(WaveStructs.Wave wave){
        WaveStructs.Group group = wave.PopGroup();
        // Check if wave is empty
        Vector2 spawn_pos = GAME_MAP.GetGate(  group.GetSpawnGate() );
        SpawnEnemies( group.GetEnemies() , spawn_pos );
    }



    
    


}
