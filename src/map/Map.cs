using Godot;
using System;
using System.Collections.Generic;


// Establece el entorno del juego y entrega informacion sobre el entorno
public class Map : Node2D{
    
    TileMap TILE_MAP;
    Position2D ENEMY_GOAL;
    Dictionary<int, Vector2> ENEMIES_GATES = new Dictionary<int, Vector2>();
    Position2D PLAYER_SPAWN;
    PathFinding PATH_FINDING;
    
    int[,] map_matrix;

    public override void _Ready(){
        TILE_MAP = GetNode<TileMap>("TileMap");
        ENEMY_GOAL = GetNode<Position2D>("Goal");
        PLAYER_SPAWN =GetNode<Position2D>("PlayerSpawn");
        PATH_FINDING = GetNode<PathFinding>("PathFinding");

        int loop = 0;
        foreach( Position2D spawn in GetNode<Node2D>("Gates").GetChildren()){
            ENEMIES_GATES[loop] = spawn.GlobalPosition;
            loop++;
        }
        

        map_matrix = CreateMatrixMap();
    }

    // Create 2D array map filled with tile ids
    int[,] CreateMatrixMap(){
        //Initialize map array
        Godot.Vector2 map_size = TILE_MAP.GetUsedRect().Size;
        int[,] map_array = new int[(int) map_size.y, (int) map_size.x];
        for (int i = 0; i < map_array.GetLength(0); i++)
        {
            for (int j = 0; j < map_array.GetLength(1); j++)
            {
                map_array[i,j] = -1;
            }
        }
        //Fill map array with tile ids
        Godot.Collections.Array tiles_coordinates = TILE_MAP.GetUsedCells();
        Godot.Vector2 map_offset = TILE_MAP.GetUsedRect().Position;
        foreach(Vector2 tile_cell in tiles_coordinates){
            int x = (int) (tile_cell.x - map_offset.x);
            int y = (int) (tile_cell.y - map_offset.y);
            map_array[y,x] = TILE_MAP.GetCell((int) tile_cell.x,(int) tile_cell.y);
        }

    return map_array;
    }

    //public int[,] GetMatrixMap() => map_matrix;

    // Add a trap to map matrix
    public void SetNewBlock(Vector2 tile_pos, int block_weight){
        map_matrix[(int) tile_pos.y, (int) tile_pos.x] = block_weight;
        // Emit signal map update
    }

    public Dictionary<int, Vector2> GetGates() => ENEMIES_GATES;

    public Position2D GetPlayerSpawn() => PLAYER_SPAWN;

    public TileMap GetTileMap() => TILE_MAP;


    public Tuple<List<Vector2>, bool> GetPathToGoal(Vector2 from){
        Tuple<List<Vector2>, bool> result = PATH_FINDING.FindPath( TILE_MAP.WorldToMap(from), TILE_MAP.WorldToMap(ENEMY_GOAL.GlobalPosition), map_matrix);
        // Convert path local coordinates to global coordinates
        List<Vector2> path = new List<Vector2>();
        foreach(Vector2 coord in result.Item1){
            path.Add( TILE_MAP.MapToWorld(coord) + new Vector2(32,32));
        }
        return new Tuple<List<Vector2>, bool>(path, result.Item2);
    }


}
