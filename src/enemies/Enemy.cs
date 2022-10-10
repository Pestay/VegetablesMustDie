using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// Enemy for testing purposes
public class Enemy : KinematicBody2D{


    Texture WALK_ANIMATION;
    Texture IDLE_ANIMATION;

    [Export]
    List<Vector2> current_path = new List<Vector2>();

    Vector2 velocity = Vector2.Zero;
    float max_speed = 12000;
    Sprite enemy_sprite;

    TileMap tile_map;

    int[,] cells;

    EnemyFSM brain;

    public List<Vector2> best_path = new List<Vector2>();

    public override void _Ready(){
        enemy_sprite = GetNode<Sprite>("Sprite");
        brain = GetNode<EnemyFSM>("EnemyFSM");

        WALK_ANIMATION = ResourceLoader.Load<Texture>("res://src/enemies/enemy_walk.png");
        IDLE_ANIMATION = ResourceLoader.Load<Texture>("res://src/enemies/enemy_idle.png");

        // DEBUG
        TestPositions test_path = GetParent().GetParent().GetNode<TestPositions>("TestPositions");
        Map map = GetTree().Root.GetNode("Game").GetNode<Map>("Map");
        Goal goal = GetTree().Root.GetNode("Game").GetNode<Goal>("Goal");

        cells = map.GetMatrixMap();

        tile_map = (TileMap) map.Get("tile_map");
        
        GD.Print("Entering Find Path");
        FindPath(cells, tile_map.WorldToMap(goal.initial_pos));
        GD.Print(tile_map.WorldToMap(goal.initial_pos));
        GD.Print(goal.initial_pos);
        SetPath(best_path);
    }

    public override void _PhysicsProcess(float delta){
        base._PhysicsProcess(delta);
        brain.UpdateFSM(delta);
    }

     // Constantly move linearly to the position
    void MoveTo(float delta, Vector2 destination){
        Vector2 to_pos = (destination - this.GlobalPosition).Normalized();
        Vector2 new_velocity = to_pos*delta*max_speed;
        velocity = new_velocity;
        velocity = this.MoveAndSlide(velocity);
    }

    // --- Actions


    // Follow a set of points
    public void FollowPath(float delta){
        if(current_path.Count <= 0){
            return;
        }
        Vector2 destination = current_path[0];
        MoveTo(delta, destination);
        if(destination.DistanceSquaredTo(this.GlobalPosition) < 256){
            current_path.RemoveAt(0);
        }
    }

    public void IdleAnimation(){
        enemy_sprite.Texture = IDLE_ANIMATION;
    }

    public void WalkAnimation(){
        enemy_sprite.Texture = WALK_ANIMATION;
    }
    

    // --- Checkers (Transitions)
    public bool HasReachTarget(){
        if(current_path.Count <= 0){
            return true;
        }
        return false;
    }

    private int getH(Vector2 current, Vector2 goal) {
        int h = (int) (Math.Abs(current.x - goal.x) + Math.Abs(current.y - goal.y));
        return h;
    }

    public void FindPath(int[,] cells, Vector2 goal){

        Vector2 initial_pos = tile_map.WorldToMap(this.GlobalPosition);
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
                reconstruct_path(cameFrom,current);
                return;
            }
                 
            openSet.Remove(current);
            foreach (Vector2 neighbour in Neighbours(current))
            {
                int tentative_gScore = gScore[current] + 1;
                
                if (!gScore.ContainsKey(neighbour))
                {
                    gScore[neighbour] = 100000;
                }
                
                
                if (tentative_gScore < gScore[neighbour])
                {
                    cameFrom[neighbour] = current;
                    gScore[neighbour] = tentative_gScore;
                    fScore[neighbour] = tentative_gScore + getH(neighbour, goal);
                    
                    
                    if (!openSet.Contains(neighbour) && (tile_map.GetCellv(neighbour) == 1))
                    {
                        openSet.Add(neighbour);
                    }
                    

                }
            }
        }
        GD.Print("ERROR");
        return;
    }

    private void reconstruct_path(Dictionary<Vector2,Vector2> cameFrom, Vector2 current)
    {
        List<Vector2> total_path = new List<Vector2>();

        total_path.Add(tile_map.MapToWorld(current));
        while (cameFrom.Keys.Contains(current))
        {
            current = cameFrom[current];
            // SE SUMA EL OFFSET
            total_path.Insert(0,tile_map.MapToWorld(current)+new Vector2(32,32));
        }
        best_path = total_path;
        return;
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

    // Setters and Getters

    public void SetPath(List<Vector2> new_path){
        current_path = new_path;
    }

}

