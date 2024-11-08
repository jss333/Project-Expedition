using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeowMeleeAttackState : EnemyState
{
    private MeowEnemy meowEnemy;
    private Color color;

    private float maxCoolDown;

    private float coolDownTimer;

    public MeowMeleeAttackState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, MeowEnemy enemy, Color color) : base(enemyBase, stateMachine, animBoolName)
    {
        this.meowEnemy = enemy;
        this.color = color;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        meowEnemy.spriteRenderer.color = color;

        maxCoolDown = 1f;
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //Debug.Log("Agro");
        Transform playerPoint = GameObject.FindGameObjectWithTag("Player").transform;
        Vector2 dir = meowEnemy.transform.position - playerPoint.position;

        Debug.DrawRay(meowEnemy.transform.position, -dir.normalized * meowEnemy.MeleeAttackRange, Color.white);

        coolDownTimer += Time.deltaTime;
        if(coolDownTimer > maxCoolDown )
        {
            //Attack
           

            Vector2 targetPosition = new Vector2(meowEnemy.transform.position.x + meowEnemy.MeleeAttackRange, meowEnemy.transform.localScale.y / 2);

            meowEnemy.SpawnDamageCaster(playerPoint.position);
            coolDownTimer = 0f;
        }

        if (meowEnemy.DetectedPlayerInRange())
        {
           stateMachine.ChangeState(meowEnemy.meowRangeAttackState);
        }
    }
}