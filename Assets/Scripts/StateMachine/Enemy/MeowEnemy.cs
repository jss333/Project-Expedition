using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeowEnemy : Enemy
{
    public MeowIdleState meowIdleState { get; private set; }

    public MeowTraverseState meowTraverseState { get; private set; }

    public MeowAlertedState meowAlertedState { get; private set; }


    protected override void Awake()
    {
        base.Awake();

        meowIdleState = new MeowIdleState(this, stateMachine, "Idle", this, Color.white);
        meowAlertedState = new MeowAlertedState(this, stateMachine, "Traverse", this, Color.yellow);
        meowTraverseState = new MeowTraverseState(this, stateMachine, "Traverse", this, Color.red);
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(meowIdleState);
    }

    protected override void Update()
    {
        base.Update();
    }
}
