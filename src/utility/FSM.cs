using Godot;
using System;
using System.Collections.Generic;

// ESTE SCRIPT NO ES REUTILIZABLE SOLO ESTA PARA COPIAR Y PEGAR EL FORMATO
public class FSM : Node{

    const ushort STATE_NULL = 0;

    public ushort current_state = STATE_NULL;

    public override void _Ready(){

    }


    public void UpdateFSM()
    { // called each frame
        ushort new_state = GetTransitions(current_state);
        if (new_state != STATE_NULL)
        {
            OnExitState(current_state, new_state);
            OnEnterState(current_state, new_state);
            //OnExitState(new_state, current_state);
            current_state = new_state;
        }
        DoAction(current_state);
    }

    // Check transition for selected state 
    protected virtual ushort GetTransitions(ushort state)
    {
        return STATE_NULL;
    }

    // What it does when it enters a state
    protected virtual void OnEnterState(ushort out_state, ushort in_state)
    {
    }

    // What it does when it exits from a state
    protected virtual void OnExitState(ushort out_state, ushort in_state)
    {
    }


    // Action that is executed while in a state
    protected virtual void DoAction(ushort state)
    {
    }
}
