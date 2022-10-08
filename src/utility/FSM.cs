using Godot;
using System;

// ESTE SCRIPT NO ES REUTILIZABLE SOLO ESTA PARA COPIAR Y PEGAR EL FORMATO
public class FSM : Node{

    protected enum STATES : ushort{
        NULL // used for return null
    }

    STATES current_state = STATES.NULL;

    public override void _Ready(){
        InitializeFSM();
    }

    protected virtual void InitializeFSM(){
        
    }

    
    protected void UpdateFSM(){ // called each frame
        STATES new_state = GetTransitions(current_state);
        if(new_state != STATES.NULL){
            OnExitState(current_state,new_state);
            OnEnterState(current_state, new_state);
            //OnExitState(new_state, current_state);
            current_state = new_state;
        }
        DoAction(current_state);
    }

    // Check transition for selected state 
    protected virtual STATES GetTransitions(STATES state){ 
        return STATES.NULL;
    }

    // What it does when it enters a state
    protected virtual void OnEnterState(STATES out_state, STATES in_state){
    }

    // What it does when it exits from a state
    protected virtual void OnExitState(STATES out_state, STATES in_state){
    }


    // Action that is executed while in a state
    protected virtual void DoAction(STATES state){ 
    }
}
