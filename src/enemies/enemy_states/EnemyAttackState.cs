using Godot;
using System;

// Defines the enemy behaviour in attack state
public class EnemyAttackState : Node{

    WoodenBlock1x1 target; 
    Enemy parent;
    AnimationPlayer ANIMATIONS;


    public void Constructor(Enemy _parent, WoodenBlock1x1 _target){
        target = _target;
        parent = _parent;
        ANIMATIONS = parent.ANIMATIONS;
    }

    public override void _Ready(){
        AttackAnimation();
    }
    
    void Attack(){
        // If target is not destroyed
        if(IsInstanceValid(target) ){
            target.TakeDamage( parent.damage);
        }
        else{
            parent.has_obstacle = false;
        }
    }
    
    float cum_delta = 0.0f;
    public override void _Process(float delta){
        cum_delta += delta*10;
        if(cum_delta > 1){
            Attack();
            cum_delta = 0.0f;
        }
    }

    public void AttackAnimation(){
        if(ANIMATIONS.CurrentAnimation != "attack"){
            ANIMATIONS.Play("attack");
        }
            
    }

    public void RemoveState(){
        parent.has_obstacle = false;
        QueueFree();
    }
    

}