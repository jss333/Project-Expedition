using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeowAlertedState : EnemyState
{
    private MeowEnemy meowEnemy;
    private Color color;

    public MeowAlertedState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, MeowEnemy enemy, Color color) : base(enemyBase, stateMachine, animBoolName)
    {
        this.meowEnemy = enemy;
        this.color = color;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        meowEnemy.spriteRenderer.color = color;

        meowEnemy.ShootBall();
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //Debug.Log("Alerted");

        if(meowEnemy.IsAgro())
        {
            stateMachine.ChangeState(meowEnemy.meowTraverseState);
        }

        if (!meowEnemy.IsAgro() && !meowEnemy.IsAlerted())
        {
            stateMachine.ChangeState(meowEnemy.meowIdleState);
        }
    }
}
