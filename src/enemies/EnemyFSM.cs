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

    EnemyAttackState current_state_test;
    EnemyWalkState walk_state;

    STATES current_state = STATES.NULL;
    FSMState current_stated;
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
                if(parent.HasObstacle()){
                    return STATES.ATTACK;
                }
                break;
            case STATES.IDLE:
                if(!parent.HasReachTarget()){
                    return STATES.WALK;
                }
                break;
            case STATES.ATTACK:
                if(!parent.HasObstacle()){
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
                //parent.current_destination = parent.GlobalPosition;
                //parent.WalkAnimation();
                //GD.Print(" comenzandoo");
                StartWalkState();
                break;
            case STATES.IDLE:
                parent.IdleAnimation();
                break;
            case STATES.ATTACK:
                PackedScene scene = GD.Load<PackedScene>("res://src/enemies/enemy_states/EnemyAttackState.tscn");
                current_state_test = scene.Instance<EnemyAttackState>();
                current_state_test.Constructor(parent, parent.GetObstacle());
                AddChild(current_state_test);
                break;
        }
    }

    void StartWalkState(){
        PackedScene scene = GD.Load<PackedScene>("res://src/enemies/enemy_states/EnemyWalkState.tscn");
        walk_state = scene.Instance<EnemyWalkState>();
        walk_state.Constructor(parent, parent.ANIMATIONS);
        AddChild(walk_state);
    }


    // What it does when it exits from a state
    protected virtual void OnExitState(STATES out_state, STATES in_state){
        switch(out_state){
            case STATES.WALK:
                walk_state.RemoveState();
                break;
            case STATES.ATTACK:
                current_state_test.RemoveState();
                break;
        }
    }


    // Action that is executed while in a state
    protected virtual void DoAction(float delta, STATES state){ 
        switch(state){
            case STATES.WALK:
                //parent.FollowPath(delta);
                //parent.MoveToGoal(delta);
                break;
            case STATES.ATTACK:
                //GD.Print("ATTACK");
                //if(IsInstanceValid(current_state_test))
                //current_state_test.RunStatex(delta);
                break;
        }
    }




}
