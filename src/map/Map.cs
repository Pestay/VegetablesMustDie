using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

// Establece el entorno del juego y entrega informacion sobre el entorno
public class Map : Node2D{

    public struct Coord{
        public int x;
        public int y;
    }

    [Signal] delegate void MapUpdate();  // Evento cuando el mapa se actualiza
    [Signal] delegate void EnemyReachGoal(); // Cuando el enemigo alcanza la meta


    public TileMap TILE_MAP;
    Area2D ENEMY_GOAL;
    Dictionary<int, Vector2> ENEMIES_GATES = new Dictionary<int, Vector2>();
    Dictionary<int, WoodenBlock1x1> wooden_obstacles = new Dictionary<int, WoodenBlock1x1>();
    Dictionary<Vector2, FlowMapCell> FLOW_MAP;

    Position2D PLAYER_SPAWN;
    public PathFinding PATH_FINDING;
    


    public int[,] map_matrix;
    int[,] flow_field;



    public override void _Process(float delta){
        if(Input.IsActionJustPressed("ui_accept")){
            DebugUtils.Print2DArray(flow_field);
        }
    }
    public override void _Ready(){
        // Inicializar el mapa
        TILE_MAP = GetNode<TileMap>("TileMap");
        ENEMY_GOAL = GetNode<Area2D>("Goal");
        PLAYER_SPAWN = GetNode<Position2D>("PlayerSpawn");
        PATH_FINDING = GetNode<PathFinding>("PathFinding");

        int loop = 0;
        foreach( Position2D spawn in GetNode<Node2D>("Gates").GetChildren()){
            ENEMIES_GATES[loop] = spawn.GlobalPosition;
            loop++;
        }
        

        map_matrix = CreateCostMatrix();
        flow_field = GenerateFlowField(this);
    }


    // Create 2D array map filled with tile ids
    int[,] CreateCostMatrix(){
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
            int cell_id = TILE_MAP.GetCell((int) tile_cell.x,(int) tile_cell.y);
            if(cell_id == 0){
                map_array[y,x] = 9999;
            }else{
                map_array[y,x] = 1;
            }

            
        }
    return map_array;
    }

    public int[,] GetMatrixMap() => map_matrix;

    public bool CoordIsWall(Coord coord){
        if(map_matrix[coord.y,coord.x] == 0){
            return true;
        }
        return false;
    }

    public bool CoordIsObstacle(Coord coord){
        if(wooden_obstacles.ContainsKey(map_matrix.GetLength(1)*coord.y + coord.x)){
            return true;
        }
        /*
        if(map_matrix[coord.y, coord.x] >= 10 && !CoordIsWall(coord)){
            return true;            
        }
        */
        return false;
    }


    public WoodenBlock1x1 GetObstacleAtCoord(Coord coord){
        return wooden_obstacles[map_matrix.GetLength(1)*coord.y + coord.x];
    }


    public Coord GetGoalCoord(){
        Vector2 pos = TILE_MAP.WorldToMap(ENEMY_GOAL.GlobalPosition);
        Coord coord = new Coord(){ x = (int) pos.x, y = (int)  pos.y };
        return coord;
    }

    public Coord GlobalToCoord(Vector2 global_pos){
        Vector2 pos = TILE_MAP.WorldToMap(global_pos);
    
        Coord r_coord = new Coord(){x = (int) pos.x, y = (int) pos.y};
        return r_coord;
    }


    
    public List<Coord> GetNeighbours(int x, int y){
        
        List<Coord> neighbours = new List<Coord>();
        int length_y = map_matrix.GetLength(0);
        int length_x = map_matrix.GetLength(1);
        if((x - 1< length_x) && (x -1 >= 0)){
            neighbours.Add( new Coord(){x = x - 1, y = y});
        }
        if((x + 1< length_x) && (x + 1 >= 0)){
            neighbours.Add( new Coord(){x = x + 1, y = y});
        }
        if((y - 1< length_y) && (y -1 >= 0)){
            neighbours.Add( new Coord(){x = x , y = y - 1});
        }
        if((y + 1< length_y) && (y +1 >= 0)){
            neighbours.Add( new Coord(){x = x , y = y + 1});
        }
        
        return neighbours;
    }


    public List<Coord> GetNeighboursFromGlobal(Vector2 global_pos){
        Vector2 tile_pos = TILE_MAP.WorldToMap(global_pos);
        List<Coord> neighbours = GetNeighbours( (int) tile_pos.x, (int) tile_pos.y);
        return neighbours;
    }
    

    // Add a trap to map matrix
    public void SetNewBlock(Vector2 tile_pos, WoodenBlock1x1 block){
        map_matrix[(int) tile_pos.y, (int) tile_pos.x] = 50; // ENCONTRAR UN PESO RELATIVO AL TAMANO DEL MAPA
        flow_field = GenerateFlowField(this);
        GD.Print(" NEW BLOCK");
        // Emit signal map update
        wooden_obstacles[( (int) tile_pos.y)*map_matrix.GetLength(1) + ( (int) tile_pos.x)] = block;
        block.Connect("OnDestroy",this, "RemoveObstacle");
        EmitSignal(nameof(MapUpdate));
    }

    public void RemoveObstacle(WoodenBlock1x1 obstacle){
        Coord pos = GlobalToCoord(obstacle.GlobalPosition);
        GD.Print("REMOVIEENDOO");
        wooden_obstacles.Remove(pos.y*map_matrix.GetLength(1) + pos.x);
    }


    public void SetFlowMap() {
        FLOW_MAP = PATH_FINDING.CreateDijkstraMap(TILE_MAP.WorldToMap(ENEMY_GOAL.GlobalPosition), map_matrix);
        //foreach(Vector2 key in FLOW_MAP.Keys)
        //    GD.Print(key);
    }

    public Vector2 CoordToGlobal(Coord coord){
        return ( TILE_MAP.MapToWorld( new Vector2(coord.x,coord.y) ) + new Vector2(16,16) );
    }

    public int GetCoordFlowValue(Coord coord){
        return flow_field[coord.y, coord.x];
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

    public Vector2 GetGate(int gate){
        if(ENEMIES_GATES.ContainsKey(gate)){
            return ENEMIES_GATES[gate];
        }
        GD.Print(" Map gate ", gate, " doesn't exist!");
        return ENEMIES_GATES[gate];
    }

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

    void _on_Goal_EnemyReachGoal(){
        EmitSignal( nameof(EnemyReachGoal) );
    }


    int TwoDimensionToOne(Coord coord, int length_x){
        return coord.y*length_x + coord.x;
    }


    public int[,] GenerateFlowField(Map map){
        
        // Get the dimensions of the cost map
        int length_y = map.GetMatrixMap().GetLength(0);
        int length_x = map.GetMatrixMap().GetLength(1);

        // Create a initial 2D matrix filled with 999
        int[,] flow_map = new int[length_y,length_x];
        for(int y = 0; y < length_y; y++){
            for(int x = 0; x < length_x; x++){
                flow_map[y,x] =  9999;
            }
        }

        List<int> queue = new List<int>();
        Dictionary<int, int> distance = new Dictionary<int, int>();
        Coord goal = map.GetGoalCoord() ;
        

        // Set goal node cost to 0
        //distance[ goal.y*length_x + goal.x ] = 0;
        flow_map[ goal.y, goal.x] = 0;
        queue.Add( goal.y*length_x + goal.x );



        // While queue is not empty
        while(queue.Count > 0){
            // Get the next node in the queue
            int current_node_id = queue.First();

            int current_node_x = current_node_id%length_x;
            int current_node_y = current_node_id/length_x;

            queue.RemoveAt(0);

            // Iterate through each neighbour of the current_node
            List<Coord> neighbours = map.GetNeighbours(current_node_x, current_node_y);
            foreach( Map.Coord neighbour in neighbours){
                // If the neighbour is not a wall
                if(map_matrix[neighbour.y, neighbour.x] < 9999){

                int end_node_cost = flow_map[current_node_y, current_node_x] + map_matrix[neighbour.y, neighbour.x];
                if((end_node_cost < flow_map[neighbour.y, neighbour.x])  ){
                    if( !queue.Contains( neighbour.y*length_x + neighbour.x)  ){
                        queue.Add( neighbour.y*length_x + neighbour.x  );

                    }
                    flow_map[neighbour.y , neighbour.x] = end_node_cost;
                }

            }
            }
        }

        
        return flow_map;
	}

}
