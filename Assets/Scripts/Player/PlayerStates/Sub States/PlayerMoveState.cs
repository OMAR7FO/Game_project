using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    private float currentSpeed;
    private float accelerationRate;
    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData,string animationName) : base(player, stateMachine, playerData, animationName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        currentSpeed = 3.0f;
        accelerationRate = playerData.maxSpeed / playerData.accelerationTime;
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();
        /*currentSpeed = Mathf.MoveTowards(currentSpeed, playerData.maxSpeed, accelerationRate * Time.deltaTime);
        player.CheckIfCanFlip((int)input.x);
     
        Vector2 force = new Vector2(currentSpeed * input.x, 0);
        player.AddForceX(force);*/
        player.CheckIfCanFlip((int)input.x);
        player.SetVelocityX( input.x * playerData.movementVelocity);
        if (input.x == 0 && !isExitingState)
        {
            stateMachine.ChangeState(player.PlayerIdleState);
        }
    }

    public override void PhysicalUpdate()
    {
        base.PhysicalUpdate();
    }
}
