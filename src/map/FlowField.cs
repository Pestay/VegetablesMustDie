using System;
using System.Collections.Generic;
using System.Linq;



// Based on https://www.redblobgames.com/pathfinding/tower-defense/

public class FlowField{

    public int[,] GetFlowField(Map map){
        
        // Generate djikstra grid, set all cells to null and wall to infinty
        int length_y = map.GetMatrixMap().GetLength(0);
        int length_x = map.GetMatrixMap().GetLength(1);
        int[,] flow_map = new int[length_y,length_x];
        for(int y = 0; y < length_y; y++){
            for(int x = 0; x < length_x; x++){
                flow_map[y,x] =  999;
            }
        }

        List<Map.Coord> queue = new List<Map.Coord>();
        Dictionary<int, int> distance = new Dictionary<int, int>();
        Map.Coord goal = map.GetGoalCoord() ;
        queue.Add( goal);
        distance[ goal.y*length_x + goal.x ] = 0;
        flow_map[ goal.y, goal.x] = 0;
        while(queue.Count > 0){
            Map.Coord current = queue.First();
            queue.RemoveAt(0);
            foreach( Map.Coord neighbour in map.GetNeighbours(current.x, current.y, map.GetMatrixMap())){
                int coord_1d = neighbour.y*length_x + neighbour.x;
                if( !distance.ContainsKey(coord_1d) && !map.CoordIsWall(neighbour)){
                    queue.Add(neighbour);
                    distance[coord_1d] = 1 + distance[ current.y*length_x + current.x ];
                    flow_map[ neighbour.y, neighbour.x] = 1 + distance[ current.y*length_x + current.x ];
                }
            }
        }

        //DebugUtils.Print2DArray(flow_map);
        return flow_map;
	}
    

}



