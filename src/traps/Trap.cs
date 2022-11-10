using Godot;
using System;

public class Trap : Node2D{
    
    protected int price = 0;
    protected bool can_block = false;


    public override void _Ready(){
        
    }


    public bool CanBlock() => can_block;

    public int GetPrice() => price;


}
