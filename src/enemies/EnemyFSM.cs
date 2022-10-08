using Godot;
using System;

public class EnemyFSM : Node
{
    protected enum STATES : ushort{
        NULL, // used for return null
        IDLE,
        WALK,
        ATTACK
    }

    STATES current_state = STATES.NULL;
    Enemy parent;
    public override void _Ready(){
        InitializeFSM();
        current_state = STATES.IDLE;
    }

    protected virtual void InitializeFSM(){
        parent = GetParent<Enemy>();
    }
    
    public void UpdateFSM(float delta){ // called each frame
        STATES new_state = GetTransitions(current_state);
        if(new_state != STATES.NULL){
            OnExitState(current_state,new_state);
            OnEnterState(current_state, new_state);
            //OnExitState(new_state, current_state);
            current_state = new_state;
        }
        DoAction(delta, current_state);
    }

    // Check transition for selected state 
    protected virtual STATES GetTransitions(STATES state){ 
        switch(state){
            case STATES.WALK:
                if(parent.HasReachTarget()){
                    return STATES.IDLE;
                }
                break;
            case STATES.IDLE:
                if(!parent.HasReachTarget()){
                    return STATES.WALK;
                }
                break;
        }
        return STATES.NULL;
    }

    // What it does when it enters a state
    protected virtual void OnEnterState(STATES out_state, STATES in_state){
        switch(in_state){
            case STATES.WALK:
                parent.WalkAnimation();
                break;
            case STATES.IDLE:
                parent.IdleAnimation();
                break;
        }
    }

    // What it does when it exits from a state
    protected virtual void OnExitState(STATES out_state, STATES in_state){
    }


    // Action that is executed while in a state
    protected virtual void DoAction(float delta, STATES state){ 
        switch(state){
            case STATES.WALK:
                parent.FollowPath(delta);
                break;
        }
    }

}
