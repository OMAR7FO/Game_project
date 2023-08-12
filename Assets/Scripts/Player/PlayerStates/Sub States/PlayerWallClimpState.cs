using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallClimpState : PlayerTouchingWallState
{
    public PlayerWallClimpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData,string animationName) : base(player, stateMachine, playerData, animationName)
    {
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
