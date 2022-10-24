using Godot;
using System;

public class WoodenBlock1x1 : StaticBody2D{


    const float MAX_HEALTH = 200.0f;
    float health = 200.0f;
    Sprite TABLE_SPRITE;

    public override void _Ready(){
        health = MAX_HEALTH;
        TABLE_SPRITE = GetNode<Sprite>("Sprite");
    }

    public void TakeDamage(float dmg){
        health -= dmg;

        if(health <= 0.2*MAX_HEALTH){
            TABLE_SPRITE.Frame = 2;
        }
        else if(health <= 0.5*MAX_HEALTH){
            TABLE_SPRITE.Frame = 1;
        }
        
        if(health <= 0){
            QueueFree();
        }
        
    }

}
