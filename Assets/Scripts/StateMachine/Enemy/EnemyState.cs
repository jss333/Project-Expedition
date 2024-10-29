using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected EnemyStateMachine stateMachine;
    public Enemy enemyBase;

    protected bool triggerCalled;
    private string animBoolName;

    protected float stateTimer;

    public EnemyState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName)
    {
        this.enemyBase = enemyBase;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
    }

    public virtual void OnEnter()
    {
        triggerCalled = false;
        //enemyBase.animator.SetBool(animBoolName, true);
    }

    public virtual void OnUpdate()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void OnExit()
    {
        //enemyBase.animator.SetBool(animBoolName, false);
    }
}
