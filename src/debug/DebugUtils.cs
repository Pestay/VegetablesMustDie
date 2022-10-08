using Godot;
using System;

public class DebugUtils : Node2D{
    
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
