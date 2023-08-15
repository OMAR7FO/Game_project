
using UnityEngine;

public class PlayerWallClimpState : PlayerTouchingWallState
{
    public PlayerWallClimpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData,string animationName) : base(player, stateMachine, playerData, animationName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        player.SetClimpCollider();
    }
    public override void Exit()
    {
        base.Exit();
        player.SetNormalCollider();
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();
        if (!isExitingState)
        {
            player.SetVelocityY(playerData.wallClimbVelocity);
            if (yInput != 1 && !isExitingState)
            {
                stateMachine.ChangeState(player.PlayerWallGrabState);
            }
        }
    }
}
