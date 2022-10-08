using Godot;
using System;

public class Map : Node2D{
    
    TileMap tile_map;
    int[,] map_matrix;


    public override void _Ready(){
        tile_map = GetNode<TileMap>("TileMap");
        map_matrix = CreateMatrixMap();
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


}
