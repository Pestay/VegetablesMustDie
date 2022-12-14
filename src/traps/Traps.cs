using Godot;
using System;
using Newtonsoft.Json;
// Handle all traps.

public class Traps : Node2D{

    // Traps
    PackedScene TRAP_PREVIEW;
    
    PreviewsData.Data PREVIEWS_DATA = new PreviewsData.Data();

    // Nodes
    Map MAP;
    GameHud HUD;

    TrapPreview current_trap = null;
    public bool in_building = false;

    int current_money = 2000;


    public override void _Ready(){
        TRAP_PREVIEW = (PackedScene)ResourceLoader.Load("res://src/traps/TrapPreview.tscn");
        HUD = GetNode<GameHud>("../GameHud");
        HUD.UpdateMoney(current_money);
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
        GetNode<CanvasLayer>("BuildingMenu").Visible = true;
        //CreateTrapPreview();
    }


    public void DeactivateBuildingMode(){
        GetNode<CanvasLayer>("BuildingMenu").Visible = false;
        DeleteTrapPreview();
    }


    void CreateTrapPreview(string trap){
        DeleteTrapPreview();
        current_trap = TRAP_PREVIEW.Instance<TrapPreview>();
        current_trap.Constructor( MAP.GetTileMap() );
        current_trap.GlobalPosition = new Vector2(300, 540);
        current_trap.Connect("PlaceTrap", this, "_OnPlaceTrap");
        AddChild(current_trap);
        // Get Trap data
        PackedScene scn = ResourceLoader.Load<PackedScene>(PREVIEWS_DATA.data[trap].Scene);
        Texture texture = ResourceLoader.Load<Texture>(PREVIEWS_DATA.data[trap].TexturePreview);
        current_trap.SetTrap(texture, scn, new Vector2(1,1), trap);
        // Set wall 1x1 
        
    }


    void DeleteTrapPreview(){
        if(IsInstanceValid(current_trap)){
            current_trap.QueueFree();
        }
    }


    public void _OnPlaceTrap(Vector2 place_pos , PackedScene trap_scene, float rotation){
        Trap new_trap = trap_scene.Instance<Trap>();
        int price = new_trap.GetPrice();
        if(current_money - price > 0){
            current_money -= price;
            HUD.UpdateMoney(current_money);

            new_trap.GlobalPosition = place_pos;
            new_trap.RotationDegrees = rotation;
            AddChild(new_trap);
            if(new_trap.CanBlock()){
                MAP.SetNewBlock(MAP.GetTileMap().WorldToMap( place_pos ), (WoodenBlock1x1) new_trap );
            }
        }
        else{
            new_trap.QueueFree();
        }
    }


    void _on_Detergent_pressed(){
        CreateTrapPreview("Detergent");

    }


    void _on_Table_pressed(){
        CreateTrapPreview("WoodenTable");

    }

    void _on_Turret_pressed(){
        CreateTrapPreview("Turret");
    }

    void _on_Spikes_pressed(){
        CreateTrapPreview("Spikes");
    }


    public void _OnEnemyDie(Enemy enemy){
        current_money += enemy.GetReward();
        HUD.UpdateMoney(current_money);
    }

}
