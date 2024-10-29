using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeowIdleState : EnemyState
{
    private MeowEnemy enemy;
    private Color color;

    public MeowIdleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, MeowEnemy enemy, Color color) : base(enemy, stateMachine, animBoolName)
    {
        this.enemy = enemy;
        this.color = color;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        enemy.spriteRenderer.color = color;
        stateTimer = 1f;
    }

    public override void OnExit()
    {
        if (stateTimer < 0)
        {
            //Debug.Log("Stopped Idle");
        }

        base.OnExit();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        Debug.Log("Idle");

        if (enemy.IsAgro())
        {
            stateMachine.ChangeState(enemy.meowTraverseState);
        }

        if (enemy.IsAlerted())
        {
            stateMachine.ChangeState(enemy.meowAlertedState);
        }
    }
}
