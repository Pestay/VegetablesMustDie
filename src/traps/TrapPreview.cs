using Godot;
using System;

// Trap preview on building mode
public class TrapPreview : Node2D{

    Sprite TRAP_SPRITE;
    Map MAP;
    Area2D OBJECT_DETECTOR; // To check if the trap is not intefering with another traps
    TileMap tile_map;
    PackedScene trap_scene;

    bool valid_position = false;
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
        MAP = GetTree().Root.GetNode("Game").GetNode<Map>("Map");
    }


    public override void _Process(float delta){
        MoveTrapPreview();
        RotationInput();

        // Check if object is in valid position
        valid_position = IsValidPosition();
        if(valid_position){
            TRAP_SPRITE.Modulate = new Color("#FFFFFF");
        }
        else{
            TRAP_SPRITE.Modulate = new Color("#FF4500");
        }


        if(Input.IsActionJustPressed("left_click")){
            PlaceNewTrap();
        }
    }


    void RotationInput(){
        if(Input.IsActionJustPressed("Q")){
            trap_rotation -= 90;
            this.RotationDegrees = trap_rotation;
        }
        if(Input.IsActionJustPressed("E")){
            trap_rotation += 90;
            this.RotationDegrees = trap_rotation;
        }

    }

    public void SetTrap(Texture trap_texture, PackedScene trap, Vector2 size, string name){
        trap_scene = trap;
        trap_name = name;
        TRAP_SPRITE.Texture = trap_texture;
        RectangleShape2D collision_shape = new RectangleShape2D();
        collision_shape.Extents = size*16 - new Vector2(5,5); //realsize - Offset
        OBJECT_DETECTOR.GetNode<CollisionShape2D>("CollisionShape2D").Shape = collision_shape;
    }

    bool IsValidPosition(){
        Map.Coord cell_pos = MAP.GlobalToCoord(this.GlobalPosition);
        // Check if the object is being collided
        if(objects_detected > 0){
            return false;
        }

        // Check if the environment allows placement of the object (Wall, Floor)
        // only can be placed on wall or not
        bool only_wall = trap_name == "Turret"; // HAY MEJORES FORMA DE HACELO!
        if(only_wall){
            
            // Check if is in the border of wall
            if (!MAP.CoordIsWall(cell_pos)){
                return false;
            }

        }
        else{
            if (MAP.CoordIsWall(cell_pos)){
                return false;
            }
        }


        return true;
    }
    

    public void _OnArea2DBodyEntered(Node body){
        objects_detected += 1;
    }
    


    public void _OnArea2DBodyExited(Node body){
        objects_detected -= 1;
    }


    // Moves the trap to the mouse position, and center it to a cell
    void MoveTrapPreview(){
        Vector2 pos = tile_map.WorldToMap(GetGlobalMousePosition());
        pos = tile_map.MapToWorld(pos) + new Vector2(16,16);
        this.GlobalPosition = pos;

    }


    void PlaceNewTrap(){
        if(valid_position){
            //new_trap.GlobalPosition = GlobalPosition;
            EmitSignal(nameof(PlaceTrap), GlobalPosition - new Vector2(16,16), trap_scene, trap_rotation );
            //GetParent().AddChild(new_trap);
        }
    }

}
