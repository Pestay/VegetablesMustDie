using Godot;
using System;

// Trap preview on building mode
public class TrapPreview : Node2D{

    public override void _Ready(){
        
    }

    public void SetTrap(Texture trap_texture){
        this.GetNode<Sprite>("Sprite").Texture = trap_texture;
    }

}
