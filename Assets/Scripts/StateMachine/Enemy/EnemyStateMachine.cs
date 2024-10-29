using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    public EnemyState currentState {  get; private set; }

    public void Initialize(EnemyState startState)
    {
        currentState = startState;
        currentState.OnEnter();
    }

    public void ChangeState(EnemyState newState)
    {
        currentState.OnExit();
        currentState = newState;
        currentState.OnEnter();
    }
}
