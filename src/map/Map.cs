using Godot;
using System;

public class Map : Node2D{
    
    TileMap tile_map;



    public override void _Ready(){
        tile_map = GetNode<TileMap>("TileMap");
        //Print2DArray(GetMatrix());
        
    }

    // Return 2D array of the map
    public int[,] GetMatrix(){
        Godot.Collections.Array tiles_coordinates = tile_map.GetUsedCells();
        Godot.Vector2 map_size = tile_map.GetUsedRect().Size;
        int[,] map_array = new int[(int) map_size.x,(int) map_size.y];
        // Transform coordinates to id
        int x = 0;
        int y = 0;
        for(int i = 0; i < tiles_coordinates.Count; i++ ){
            Vector2 coordinate = (Vector2) tiles_coordinates[i];
            map_array[y,x] = tile_map.GetCell((int) coordinate.x, (int) coordinate.y);
            x += 1;
            if(x >= map_size.x){
                y += 1;
                x = 0;
            }

        }

    return map_array;
    }

    public static void Print2DArray<T>(T[,] matrix)
    {
        string buffer = "";
        for (int i = 0; i < matrix.GetLength(0); i++)
        {

            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                buffer += matrix[i,j].ToString();
                buffer += ",";
            }

            GD.Print(buffer);
            buffer = "";
        }
    }


}
