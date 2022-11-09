using System.Collections.Generic;
using Godot;

namespace WaveStructs{


    // Enemies group
    public struct Group{
        public List<PackedScene> enemies;
        public int spawn_gate;

        public Group(List<PackedScene> set_enemies, int gate){
            enemies = set_enemies;
            spawn_gate = gate;
        }

        public List<PackedScene> GetEnemies(){
            return enemies;
        }

        public int GetSpawnGate(){
            return spawn_gate;
        }
    }

    // Group of list of enemies
    public struct Wave{
        List<Group> wave_enemies;

        public Wave(List<Group> enemies){
            wave_enemies = enemies;
        }

        public Group PopGroup(){
            Group enemies = wave_enemies[0];
            wave_enemies.RemoveAt(0);
            return enemies;
        }

        public bool IsEmpty(){
            if(wave_enemies.Count <= 0){
                return true;
            }
            return false;
        }
}
    
}