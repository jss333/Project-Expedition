using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeowRangeAttackState : EnemyState
{
    private MeowEnemy meowEnemy;
    private Color color;
    private float maxCoolDown;

    private float coolDownTimer;

    public MeowRangeAttackState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, MeowEnemy enemy, Color color) : base(enemyBase, stateMachine, animBoolName)
    {
        this.meowEnemy = enemy;
        this.color = color;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        maxCoolDown = 1f;

        meowEnemy.spriteRenderer.color = color;
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //Debug.Log("Alerted");

        coolDownTimer += Time.deltaTime;
        if(coolDownTimer > maxCoolDown)
        {
            meowEnemy.ShootBall();
            coolDownTimer = 0f;
        }

        if(meowEnemy.IsAgro())
        {
            stateMachine.ChangeState(meowEnemy.meowTraverseState);
        }

        if (!meowEnemy.IsAgro() && !meowEnemy.DetectedPlayerInRange())
        {
            stateMachine.ChangeState(meowEnemy.meowIdleState);
        }
    }
}
