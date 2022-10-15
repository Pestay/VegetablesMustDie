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

    //Signals

    [Signal] delegate void PlaceTrap(Vector2 tile_pos);
    
    
    public void Constructor(TileMap new_tilemap){
        tile_map = new_tilemap;
    }

    public override void _Ready(){
        OBJECT_DETECTOR = GetNode<Area2D>("Area2D");
        TRAP_SPRITE = this.GetNode<Sprite>("Sprite");
    }


    public override void _Process(float delta){
        MoveTrapPreview();
        if(Input.IsActionJustPressed("left_click")){
            PlaceNewTrap();
        }
    }


    public void SetTrap(Texture trap_texture, PackedScene trap, Vector2 size){
        trap_scene = trap;
        TRAP_SPRITE.Texture = trap_texture;
        RectangleShape2D collision_shape = new RectangleShape2D();
        collision_shape.Extents = size*32 - new Vector2(5,5); //realsize - Offset
        OBJECT_DETECTOR.GetNode<CollisionShape2D>("CollisionShape2D").Shape = collision_shape;
    }


    void CheckValidPosition(){
        if(objects_detected > 0){
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
            Node2D new_trap = trap_scene.Instance<Node2D>();
            new_trap.GlobalPosition = GlobalPosition;
            EmitSignal(nameof(PlaceTrap), tile_map.WorldToMap( GetGlobalMousePosition()) );
            GetParent().AddChild(new_trap);
        }
    }


}
