using Godot;
using System;

// Trap preview on building mode
public class TrapPreview : Node2D{

    Sprite TRAP_SPRITE;
    TileMap tile_map;
    PackedScene trap_scene;
    bool valid_position = false;
    Area2D OBJECT_DETECTOR; // Check if the trap is not intefering with another traps
    int objects_detected = 0;
    string trap_name;
    float trap_rotation = 0;

    //Signals

    [Signal] delegate void PlaceTrap(Vector2 tile_pos, Node2D trap);
    
    
    public void Constructor(TileMap new_tilemap){
        tile_map = new_tilemap;
    }

    public override void _Ready(){
        OBJECT_DETECTOR = GetNode<Area2D>("Area2D");
        TRAP_SPRITE = this.GetNode<Sprite>("Sprite");
        CheckValidPosition();
    }


    public override void _Process(float delta){
        MoveTrapPreview();
        if(Input.IsActionJustPressed("R"))
        {
            trap_rotation += 90;
            this.RotationDegrees = trap_rotation;
        }
        if(Input.IsActionJustPressed("left_click")){
            PlaceNewTrap();
        }
    }


    public void SetTrap(Texture trap_texture, PackedScene trap, Vector2 size, string name){
        trap_scene = trap;
        trap_name = name;
        TRAP_SPRITE.Texture = trap_texture;
        TRAP_SPRITE.Frame = 0;
        TRAP_SPRITE.Hframes = 3;
        RectangleShape2D collision_shape = new RectangleShape2D();
        collision_shape.Extents = size*16 - new Vector2(5,5); //realsize - Offset
        OBJECT_DETECTOR.GetNode<CollisionShape2D>("CollisionShape2D").Shape = collision_shape;
    }


    void CheckValidPosition(){
        if(objects_detected > 0 && trap_name != "Turret"){
            valid_position = false;
            TRAP_SPRITE.Modulate = new Color("#FF4500");
        }
        else{
            valid_position = true;
            TRAP_SPRITE.Modulate = new Color("#FFFFFF");
        }
    }


    public void _OnArea2DBodyEntered(Node body){
        objects_detected += 1;
        CheckValidPosition();
    }


    public void _OnArea2DBodyExited(Node body){
        objects_detected -= 1;
        CheckValidPosition();
    }


    void MoveTrapPreview(){
        Vector2 pos = tile_map.WorldToMap(GetGlobalMousePosition());
        pos = tile_map.MapToWorld(pos);
        this.GlobalPosition = pos;
    }


    void PlaceNewTrap(){
        if(valid_position){
            Trap new_trap = trap_scene.Instance<Trap>();
            new_trap.GlobalPosition = GlobalPosition;
            
            EmitSignal(nameof(PlaceTrap), GlobalPosition, new_trap, trap_rotation );
            //GetParent().AddChild(new_trap);
        }
    }


}
