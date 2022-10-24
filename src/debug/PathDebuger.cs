using Godot;

public class PathDebuger : Node{


    public void _Process(){
        GetNode<Line2D>("Line2D").GlobalPosition = new Vector2(0,0);
        GD.Print("TETE");
    }

    public void AddPoint(Vector2 point){
        GetNode<Line2D>("Line2D").AddPoint(point);
    }

    public void ClearPoints(){
        GetNode<Line2D>("Line2D").ClearPoints();
    }


}
