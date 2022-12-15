using Godot;
using System;

public class WoodenBlock1x1 : Trap{


    const float MAX_HEALTH = 200.0f;
    float health = 200.0f;
    Sprite TABLE_SPRITE;

    WoodenBlock1x1(){
        can_block = true;
        price = 100;
    }

    public override void _Ready(){
        health = MAX_HEALTH;
        TABLE_SPRITE = GetNode<Sprite>("WoodenBlock1x1/Sprite");
    }

    public void TakeDamage(float dmg){
        
        health -= dmg;
        /*
        if(health <= 0.2*MAX_HEALTH){
            TABLE_SPRITE.Frame = 2;
        }
        else if(health <= 0.5*MAX_HEALTH){
            TABLE_SPRITE.Frame = 1;
        }
        */
        if(health <= 0){
            GD.Print(" MESITA DESTRUIDA");
            QueueFree();
        }
        
    }

}
