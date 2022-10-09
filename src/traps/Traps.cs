using Godot;
using System;

// Handle all traps.

public class Traps : Node2D{


    Node2D current_trap = null;
    bool in_building = false;
    public override void _Ready(){
        
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        HandleUserInput();
    }

    void HandleUserInput(){
        if(Input.IsActionJustPressed("B")){
            in_building = !in_building;
            if(in_building){
                ActivateBuildingMode();
            }
            else{
                DeactivateBuildingMode();
            }

        }
    }

    public void ActivateBuildingMode(){
        GetNode<CanvasLayer>("CanvasLayer").Visible = true;
    }

    public void DeactivateBuildingMode(){
        GetNode<CanvasLayer>("CanvasLayer").Visible = false;
    }



}
