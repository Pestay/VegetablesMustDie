using Godot;
using System;

// Handle all traps.

public class Traps : Node2D{

    // Traps
    PackedScene TRAP_PREVIEW;
    PackedScene BLOCK_1_1;
    Texture BLOCK_1_1_PREVIEW;

    // Nodes
    Map MAP;

    TrapPreview current_trap = null;
    bool in_building = false;


    public override void _Ready(){
        TRAP_PREVIEW = (PackedScene)ResourceLoader.Load("res://src/traps/TrapPreview.tscn");
        BLOCK_1_1 = (PackedScene)ResourceLoader.Load("res://src/traps/1x1block/WoodenBlock1x1.tscn");
        BLOCK_1_1_PREVIEW = (Texture)ResourceLoader.Load("res://src/traps/1x1block/wooden_block_1x1.png");
        MAP = GetNode<Map>("../Map");
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
        CreateTrapPreview();
    }


    public void DeactivateBuildingMode(){
        GetNode<CanvasLayer>("CanvasLayer").Visible = false;
        DeleteTrapPreview();
    }


    void CreateTrapPreview(){
        current_trap = TRAP_PREVIEW.Instance<TrapPreview>();
        current_trap.Constructor(MAP.GetTileMap());
        current_trap.GlobalPosition = new Vector2(300, 540);
        current_trap.Connect("PlaceTrap", this, "_OnPlaceTrap");
        AddChild(current_trap);
        current_trap.SetTrap(BLOCK_1_1_PREVIEW, BLOCK_1_1, new Vector2(1,1));
        // Set wall 1x1 
        
    }


    void DeleteTrapPreview(){
        if(IsInstanceValid(current_trap)){
            current_trap.QueueFree();
        }
    }


    public void _OnPlaceTrap(Vector2 tile_pos){
        MAP.SetNewBlock(tile_pos, 10);
    }




}
