using Godot;
using System;
using System.Collections.Generic;


// Establece el entorno del juego y entrega informacion sobre el entorno
public class Map : Node2D{
    [Signal]
    delegate void MapUpdate();

    

    TileMap TILE_MAP;
    Area2D ENEMY_GOAL;
    Dictionary<int, Vector2> ENEMIES_GATES = new Dictionary<int, Vector2>();

    Dictionary<Vector2, FlowMapCell> FLOW_MAP;

    Position2D PLAYER_SPAWN;
    PathFinding PATH_FINDING;
    
    int[,] map_matrix;

    public override void _Ready(){
        TILE_MAP = GetNode<TileMap>("TileMap");
        ENEMY_GOAL = GetNode<Area2D>("Goal");
        PLAYER_SPAWN =GetNode<Position2D>("PlayerSpawn");
        PATH_FINDING = GetNode<PathFinding>("PathFinding");

        int loop = 0;
        foreach( Position2D spawn in GetNode<Node2D>("Gates").GetChildren()){
            ENEMIES_GATES[loop] = spawn.GlobalPosition;
            loop++;
        }
        

        map_matrix = CreateMatrixMap();
        SetFlowMap();
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
            GD.Print(map_array[y,x]);

        }

    return map_array;
    }

    //public int[,] GetMatrixMap() => map_matrix;

    // Add a trap to map matrix
    public void SetNewBlock(Vector2 tile_pos, int block_weight){
        map_matrix[(int) tile_pos.y, (int) tile_pos.x] = 10; // CHANGE THISS!!!
        // Emit signal map update
        EmitSignal(nameof(MapUpdate));
        
    }

    public void SetFlowMap() {
        FLOW_MAP = PATH_FINDING.CreateDijkstraMap(TILE_MAP.WorldToMap(ENEMY_GOAL.GlobalPosition), map_matrix);
        foreach(Vector2 key in FLOW_MAP.Keys)
            GD.Print(key);
    }

    public List<PathFindingCell> GetPathFromFlowMap(Vector2 from)
    {
        List<PathFindingCell> result = FLOW_MAP[TILE_MAP.WorldToMap(from)].path_to_goal;
        List<PathFindingCell>  path = new List<PathFindingCell>();
        foreach(PathFindingCell cell in result){
            PathFindingCell new_cell = new PathFindingCell();
            new_cell.coord = TILE_MAP.MapToWorld(cell.coord) + new Vector2(16,16);
            new_cell.has_obstacle = cell.has_obstacle;
            path.Add(new_cell);
        }
        path.Reverse();
        return path;
    }

    public Dictionary<Vector2, FlowMapCell> GetFlowMap() => FLOW_MAP;

    public Dictionary<int, Vector2> GetGates() => ENEMIES_GATES;

    public Position2D GetPlayerSpawn() => PLAYER_SPAWN;

    public TileMap GetTileMap() => TILE_MAP;




    public List<PathFindingCell> GetPathToGoal(Vector2 from){
        List<PathFindingCell> result = PATH_FINDING.FindPath( TILE_MAP.WorldToMap(from), TILE_MAP.WorldToMap(ENEMY_GOAL.GlobalPosition), map_matrix);
        
        // Convert  local coordinates to global coordinates
        List<PathFindingCell>  path = new List<PathFindingCell>();
        foreach(PathFindingCell cell in result){
            PathFindingCell new_cell = new PathFindingCell();
            new_cell.coord = TILE_MAP.MapToWorld(cell.coord) + new Vector2(16,16);
            new_cell.has_obstacle = cell.has_obstacle;
            path.Add(new_cell);
        }
        return path;
    }


}
