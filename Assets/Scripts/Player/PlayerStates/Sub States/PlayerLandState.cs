
using UnityEngine;

public class PlayerLandState : PlayerGroundedState
{
    public PlayerLandState(Player player, PlayerStateMachine stateMachine, PlayerData playerData,string animationName) : base(player, stateMachine, playerData, animationName)
    {
    }

    

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();
        if (!isExitingState)
        {
            if (input.x != 0)
            {
                stateMachine.ChangeState(player.PlayerMoveState);
            }
            else
            {
                stateMachine.ChangeState(player.PlayerIdleState);
            }
        }
    }

}
