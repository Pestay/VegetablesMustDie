using Godot;
using System;

public class Menu : Control{

    public override void _Ready(){
        
    }

    public void _OnPlayPressed(){
        GetTree().ChangeScene("res://src/game/Game.tscn");
    }

    public void _OnExitPressed(){
        GetTree().Quit();
    }
}
