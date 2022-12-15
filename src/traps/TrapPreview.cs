using Godot;
using System;

// Trap preview on building mode
public class TrapPreview : Node2D{

    Sprite TRAP_SPRITE;
    Map MAP;
    PathFinding PATH_FINDING;
    TileMap tile_map;
    PackedScene trap_scene;
    bool valid_position = false;
    Area2D OBJECT_DETECTOR; // Check if the trap is not intefering with another traps
    int objects_detected = 0;
    bool valid_wall = false;
    bool turret_in_the_way = false;
    string trap_name;
    float trap_rotation = 0;
    int[,] map_matrix;

    //Signals

    [Signal] delegate void PlaceTrap(Vector2 tile_pos, Node2D trap);
    
    
    public void Constructor(TileMap new_tilemap){
        tile_map = new_tilemap;
    }

    public override void _Ready(){
        OBJECT_DETECTOR = GetNode<Area2D>("Area2D");
        TRAP_SPRITE = this.GetNode<Sprite>("Sprite");
        MAP =GetTree().Root.GetNode("Game").GetNode<Map>("Map");
        PATH_FINDING = MAP.PATH_FINDING;
        map_matrix = MAP.map_matrix;
        CheckValidPosition();
    }


    public override void _Process(float delta){
        map_matrix = MAP.map_matrix;
        MoveTrapPreview();
        RotationInput();

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
        TRAP_SPRITE.Frame = 0;
        TRAP_SPRITE.Hframes = 3;
        RectangleShape2D collision_shape = new RectangleShape2D();
        collision_shape.Extents = size*16 - new Vector2(5,5); //realsize - Offset
        OBJECT_DETECTOR.GetNode<CollisionShape2D>("CollisionShape2D").Shape = collision_shape;
    }


    void CheckValidPosition(){
        Vector2 pos = tile_map.WorldToMap(GlobalPosition);
        pos = InsideMatrixCheck(pos);
        if(MAP.TILE_MAP.GetCell((int)pos.x,(int)pos.y) == 0 && !turret_in_the_way)
        {
            foreach (Vector2 neighbour in PATH_FINDING.Neighbours(pos, map_matrix))
            {
                Vector2 nb = InsideMatrixCheck(neighbour);
                if(MAP.TILE_MAP.GetCell((int)nb.x,(int)nb.y) != 0 && MAP.TILE_MAP.GetCell((int)nb.x,(int)nb.y) != 10)
                {
                    valid_wall = true;
                    break;
                }
                else
                {
                    valid_wall = false;
                }
            }
        } else {
            valid_wall = false;
        }
        if(trap_name == "Turret" && valid_wall) {
            valid_position = true;
            TRAP_SPRITE.Modulate = new Color("#FFFFFF");
        } else if (objects_detected > 1) {
            valid_position = false;
            TRAP_SPRITE.Modulate = new Color("#FF4500");
        } else if (trap_name != "Turret") {
            valid_position = true;
            TRAP_SPRITE.Modulate = new Color("#FFFFFF");
        } else {
            valid_position = true;
            TRAP_SPRITE.Modulate = new Color("#FFFFFF");
        }
    }


    public void _OnArea2DBodyEntered(Node body){
        objects_detected += 1;
        GD.Print(body);
        if(body.IsInGroup("Turret")) {
            turret_in_the_way = true;
        }
        CheckValidPosition();
    }


    public void _OnArea2DBodyExited(Node body){
        objects_detected -= 1;
        if(body.IsInGroup("Turret")) {
            turret_in_the_way = false;
        }
        valid_wall = false;
        CheckValidPosition();
    }


    void MoveTrapPreview(){
        Vector2 pos = tile_map.WorldToMap(GetGlobalMousePosition());
        pos = tile_map.MapToWorld(pos);
        this.GlobalPosition = pos;
        CheckValidPosition();
    }


    void PlaceNewTrap(){
        if(valid_position){
            Trap new_trap = trap_scene.Instance<Trap>();
            new_trap.GlobalPosition = GlobalPosition;
            
            EmitSignal(nameof(PlaceTrap), GlobalPosition, new_trap, trap_rotation );
            //GetParent().AddChild(new_trap);
        }
    }

    Vector2 InsideMatrixCheck(Vector2 position) {
        int x = (int)position.x;
        int y = (int)position.y;
        int max_x = map_matrix.GetLength(0)-1;
        int max_y = map_matrix.GetLength(1)-1;
        if(position.x > max_x) {
            x = max_x;
        }
        if (position.y > max_y) {
            y = max_y;
        }
        if (position.x < 0) {
            x = 0;
        }
        if (position.y < 0) {
            y = 0;
        }

        Vector2 final_pos = new Vector2(x,y);

        return final_pos;
    }


}
