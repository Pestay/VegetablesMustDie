using Godot;
using System;

public class WoodenBlock1x1 : Trap{


    const float MAX_HEALTH = 200.0f;
    float health = 200.0f;
    Sprite TABLE_SPRITE;
    AudioStreamPlayer2D AUDIO_CONTROLLER;
    PackedScene DEATH;

    [Signal]
    delegate void OnDestroy(WoodenBlock1x1 self);

    WoodenBlock1x1(){
        can_block = true;
        price = 200;
    }

    public override void _Ready(){
        health = MAX_HEALTH;
        TABLE_SPRITE = GetNode<Sprite>("WoodenBlock1x1/Sprite");
        AUDIO_CONTROLLER = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
        AUDIO_CONTROLLER.Stream = GD.Load<AudioStream>("res://src/traps/1x1block/hit.wav");
        DEATH = GD.Load<PackedScene>("res://src/effects/Death.tscn");
    }

    public void TakeDamage(float dmg){
        
        health -= dmg;
        
        if(health <= 0.2*MAX_HEALTH){
            TABLE_SPRITE.Frame = 2;
        }
        else if(health <= 0.5*MAX_HEALTH){
            TABLE_SPRITE.Frame = 1;
        }

        AUDIO_CONTROLLER.Play();

        if(health <= 0){
            GD.Print(" MESITA DESTRUIDA");
            EmitSignal(nameof(OnDestroy), this);
            Death death_instance = (Death)DEATH.Instance();
            GetTree().CurrentScene.AddChild(death_instance);
            QueueFree();
        }
        
    }



}
