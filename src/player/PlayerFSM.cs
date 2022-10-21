using Godot;
using System;

public class PlayerFSM : FSM
{
    
    public const ushort STATE_IDLE = 1;
    public const ushort STATE_WALK = 2;
    Player parent;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready(){
        parent = GetParent<Player>();
        current_state = STATE_IDLE;
    }

    protected override ushort GetTransitions(ushort state){
        
        switch(state){
            case STATE_IDLE:
                if( parent.GetVelocity().LengthSquared() >= 1.0f){
                    return STATE_WALK;
                }
                break;
            case STATE_WALK:
                if( parent.GetVelocity().LengthSquared() <= 1.0f){
                    return STATE_IDLE;
                }
                break;
            }

        return base.GetTransitions(state);
        }
    
    protected override void OnEnterState(ushort previous_state, ushort next_state){
        switch(next_state){
            case STATE_IDLE:
                parent.IdleAnimation();
                break;
            case STATE_WALK:
                parent.WalkAnimation();
                break;
        }
    }
        
    

}
