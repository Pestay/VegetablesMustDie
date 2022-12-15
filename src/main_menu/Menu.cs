using Godot;
using System;

public class Menu : Control{

    AudioStreamPlayer2D AUDIO_CONTROLLER;
    AcceptDialog Instrucciones; 
    WindowDialog Assets;

    public override void _Ready(){
        AUDIO_CONTROLLER = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
        AUDIO_CONTROLLER.Stream = GD.Load<AudioStream>("res://src/main_menu/menu.mp3");
        AUDIO_CONTROLLER.Play();
        Instrucciones = GetNode<AcceptDialog>("Instrucciones");
        Assets = GetNode<WindowDialog>("Assets");
    }

    public void _OnPlayPressed(){
        Instrucciones.Popup_();
        Assets.Popup_();
    }

    public void _on_Instrucciones_confirmed()
    {
        GetTree().ChangeScene("res://src/game/Game.tscn");
    }

    public void _OnExitPressed(){
        GetTree().Quit();
    }
}
