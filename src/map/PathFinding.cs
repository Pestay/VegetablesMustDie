using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
// Pathfinding algorithms
public class PathFinding : Node{

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

        for (int i = 0; i < cells.GetLength(0); i++)
        {

            for (int j = 0; j < cells.GetLength(1); j++)
            {
            Vector2 cell_pos = new Vector2(j,i);
            if(!fScore.ContainsKey( cell_pos) )
                fScore[cell_pos] = 1000000;
            if(!gScore.ContainsKey(cell_pos) )
                gScore[cell_pos] = 1000000;
            }
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
                return reconstruct_path(cameFrom,current, cells);
            }
                 
            openSet.Remove(current);
            foreach (Vector2 neighbour in Neighbours(current, cells))
            {
                int tentative_gScore;
                if(cells[(int)neighbour.y,(int)neighbour.x] == 0)
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
                    
                    
                    if (!openSet.Contains(neighbour) && (cells[(int)neighbour.y,(int)neighbour.x] != 3))
                    {
                        openSet.Add(neighbour);
                        if (cells[(int)neighbour.y,(int)neighbour.x] == 10)
                            blocked = true;
                    }
                    

                }
            }
        }
        GD.Print("PATH NOT FOUNDD");
        List<Vector2> errList = new List<Vector2>();
        return Tuple.Create(errList, blocked);
    }


    private Tuple<List<Vector2>, bool> reconstruct_path(Dictionary<Vector2,Vector2> cameFrom, Vector2 current, int[,] cells)
    {
        bool blocked = false;
        List<Vector2> total_path = new List<Vector2>();

        total_path.Add(current);
        while (cameFrom.Keys.Contains(current)){
            if (cells[(int)current.y,(int)current.x] == 10)
                blocked = true;
            current = cameFrom[current];
            
            // SE SUMA EL OFFSET
            total_path.Insert(0,current);
        }
        return Tuple.Create(total_path,blocked);
    }


    private List<Vector2> Neighbours(Vector2 n, int[,] cells){
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


    private int getH(Vector2 current, Vector2 goal) {
        int h = (int) (Math.Abs(current.x - goal.x) + Math.Abs(current.y - goal.y));
        return h;
    }


}
