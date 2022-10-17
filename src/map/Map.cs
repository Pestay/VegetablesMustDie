using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class Map : Node2D{
    
    TileMap tile_map;
    int[,] map_matrix;

    public List<Vector2> best_path = new List<Vector2>();

    public List<Vector2> spawn_points = new List<Vector2>();

    Goal goal;
    Node2D spawn;
    public List<Enemy> enemies = new List<Enemy>();

    int[,] cells;

    public override void _Ready(){
        tile_map = GetNode<TileMap>("TileMap");
        map_matrix = CreateMatrixMap();
        goal = GetTree().Root.GetNode("Game").GetNode<Goal>("Goal");
        /*
        foreach (Node2D node in GetTree().Root.GetNode("Spawns").GetChildren())
            spawn_points.Append( node.GlobalPosition);
        */

        foreach (Enemy enemy in GetTree().Root.GetNode("Game").GetNode("Enemies").GetChildren())
            enemies.Add(enemy);

        
        spawn = GetTree().Root.GetNode("Game").GetNode("Spawns").GetNode<Node2D>("Spawn 1");
        cells = GetMatrixMap();
        best_path = FindPath(tile_map.WorldToMap( spawn.GlobalPosition), tile_map.WorldToMap(goal.initial_pos), cells).Item1;
        //DebugUtils.Print2DArray(map_matrix);
    }

    // Create 2D array map filled with tile ids
    int[,] CreateMatrixMap(){
        //Initialize map array
        Godot.Vector2 map_size = tile_map.GetUsedRect().Size;
        int[,] map_array = new int[(int) map_size.y, (int) map_size.x];
        for (int i = 0; i < map_array.GetLength(0); i++)
        {
            for (int j = 0; j < map_array.GetLength(1); j++)
            {
                map_array[i,j] = -1;
            }
        }

        //Fill map array with tile ids
        Godot.Collections.Array tiles_coordinates = tile_map.GetUsedCells();
        Godot.Vector2 map_offset = tile_map.GetUsedRect().Position;
        foreach(Vector2 tile_cell in tiles_coordinates){
            int x = (int) (tile_cell.x - map_offset.x);
            int y = (int) (tile_cell.y - map_offset.y);
            map_array[y,x] = tile_map.GetCell((int) tile_cell.x,(int) tile_cell.y);
        }

    return map_array;
    }

    public int[,] GetMatrixMap(){
        return map_matrix;
    }

    public void SetNewBlock(Vector2 tile_pos, int block_weight){
        GD.Print("Set!!");
        map_matrix[(int) tile_pos.y, (int) tile_pos.x] = block_weight;
        foreach(Enemy enemy in enemies)
        {
            enemy.SetPath(FindPath(tile_map.WorldToMap(enemy.GlobalPosition), tile_map.WorldToMap(goal.initial_pos), cells).Item1);
        }
    }


    public TileMap GetTileMap() => tile_map;


    private int getH(Vector2 current, Vector2 goal) {
        int h = (int) (Math.Abs(current.x - goal.x) + Math.Abs(current.y - goal.y));
        return h;
    }

    public Tuple<List<Vector2>, bool> FindPath(Vector2 initial_pos, Vector2 goal, int[,] cells){
        bool blocked = false;
        Vector2 current;
        int min = 1000000000;
        Vector2 key = new Vector2(-1,-1);
        Dictionary<Vector2, float> OpenList = new Dictionary<Vector2, float>();

        List<Vector2> openSet = new List<Vector2>();

        openSet.Add(initial_pos);


        var cameFrom = new Dictionary<Vector2,Vector2>();
        var gScore = new Dictionary<Vector2,int>();
        
        gScore[initial_pos] = 0;

        var fScore = new Dictionary<Vector2, int>();
        fScore[initial_pos] = getH(initial_pos, goal);
        foreach(var cell in tile_map.GetUsedCells())
        {
            if(!fScore.ContainsKey((Vector2) cell))
                fScore[(Vector2) cell] = 1000000;

            if(!gScore.ContainsKey((Vector2) cell))
                gScore[(Vector2) cell] = 1000000;
        }


        while (openSet.Count > 0) {
            min = 1000000000;
            key = new Vector2(-1,-1);
            foreach(Vector2 value in openSet)
            {
                if (fScore[value] < min)
                {
                    min = fScore[value]; 
                    key = value;
                }
                   
            }

            current = key;
            
            if (current == goal)
            {
                return reconstruct_path(cameFrom,current);
            }
                 
            openSet.Remove(current);
            foreach (Vector2 neighbour in Neighbours(current))
            {
                int tentative_gScore;
                if(cells[(int)neighbour.y,(int)neighbour.x] == 1)
                    tentative_gScore = gScore[current] + 1;
                else
                    tentative_gScore = gScore[current] + 100;
                
                if (!gScore.ContainsKey(neighbour))
                {
                    gScore[neighbour] = 100000;
                }
                
                
                if (tentative_gScore < gScore[neighbour])
                {
                    cameFrom[neighbour] = current;
                    gScore[neighbour] = tentative_gScore;
                    fScore[neighbour] = tentative_gScore + getH(neighbour, goal);
                    
                    
                    if (!openSet.Contains(neighbour) && (cells[(int)neighbour.y,(int)neighbour.x] != 0))
                    {
                        openSet.Add(neighbour);
                        if (cells[(int)neighbour.y,(int)neighbour.x] == 10)
                            blocked = true;
                    }
                    

                }
            }
        }
        List<Vector2> errList = new List<Vector2>();
        GD.Print("ERROR");
        return Tuple.Create(errList,blocked);
    }

    private Tuple<List<Vector2>, bool> reconstruct_path(Dictionary<Vector2,Vector2> cameFrom, Vector2 current)
    {
        bool blocked = false;
        List<Vector2> total_path = new List<Vector2>();

        total_path.Add(tile_map.MapToWorld(current));
        while (cameFrom.Keys.Contains(current))
        {
            if (cells[(int)current.y,(int)current.x] == 10)
                blocked = true;
            current = cameFrom[current];
            
            // SE SUMA EL OFFSET
            total_path.Insert(0,tile_map.MapToWorld(current)+new Vector2(32,32));
        }
        return Tuple.Create(total_path,blocked);
    }

    private List<Vector2> Neighbours(Vector2 n)
    {
        List<Vector2> temp = new List<Vector2>();

        int row = (int)n.y;
        int col = (int)n.x;

        if(row + 1 < cells.GetLength(0))
        {
            temp.Add(new Vector2(col,row + 1));
        }
        if(row - 1 >= 0)
        {
            temp.Add(new Vector2(col,row - 1));
        }
        if(col - 1 >= 0)
        {
            temp.Add(new Vector2(col - 1,row));
        }
        if(col + 1 < cells.GetLength(1))
        {
            temp.Add(new Vector2(col + 1,row));
        }

        return temp;
    }
}
