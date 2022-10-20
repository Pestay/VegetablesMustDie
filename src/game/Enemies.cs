using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

// Enemy coordinator, spawn etc...
public class Enemies : Node2D{


    List<PackedScene> enemies_group;
    Map GAME_MAP;

    public override void _Ready(){

        
    }


    public void Init(Map game_map){
        GAME_MAP = game_map;
    }

    Node2D SpawnEnemy(Vector2 spawn_pos, PackedScene enemy){
        Node2D new_enemy = enemy.Instance<Node2D>();
        new_enemy.GlobalPosition = spawn_pos;
        AddChild(new_enemy);
        return new_enemy;
    }


    public void SpawnEnemyGroup(List<PackedScene> enemies, Vector2 initial_pos){
        uint enemies_in_col = 3; //The number of enemies in a column
        uint count = 0;
        Vector2 offset_vector = new Vector2(0,0);
        foreach(PackedScene enemy in enemies){
            Enemy new_enemy = (Enemy) SpawnEnemy(initial_pos + offset_vector ,enemy);
            
            new_enemy.SetPath(GAME_MAP.GetPathToGoal(new_enemy.GlobalPosition).Item1);
            count += 1;
            offset_vector += new Vector2(0, 64);
            if(count >= enemies_in_col){
                offset_vector += new Vector2(-64, -offset_vector.y);
                count = 0;
            }
        }
    }


}
