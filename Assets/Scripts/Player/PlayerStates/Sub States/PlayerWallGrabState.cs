
using UnityEngine;

public class PlayerWallGrabState : PlayerTouchingWallState
{
    private Vector2 holdPosition;
    public PlayerWallGrabState(Player player, PlayerStateMachine stateMachine, PlayerData playerData,string animationName) : base(player, stateMachine, playerData, animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        holdPosition = player.transform.position;
        HoldPosition();
        player.SetClimpCollider();
    }
    public override void Exit()
    {
        base.Exit();
        player.RB.isKinematic = false;
        player.SetNormalCollider();
    }
    public override void LogicalUpdate()
    {
        base.LogicalUpdate();
        if (!isExitingState)
        {
                HoldPosition();

            if (yInput > 0)
            {
                stateMachine.ChangeState(player.PlayerWallClimbState);
            }
            else if (yInput < 0 || !grapInput) 
            {
                stateMachine.ChangeState(player.PlayerWallSlideState);
            }

        }
    }
    private void HoldPosition()
    {
        if (player.IsHoldingMovingPlatform)
        {
            Vector3 platformOffSet = player.transform.position - player.MovingPlatformPosition ;
            player.transform.position = player.MovingPlatformPosition + platformOffSet;//MovingPlatfromPosition + platformOffSet;
            player.RB.isKinematic = true;
        }
        else
        {
            player.SetVelocityX(0);
            // try to remove this when you add the cinemachine camera and see what happen 
            // the cinemachine camera follow the velocity of the player because that we make the y velocity is 0
            player.SetVelocityY(0);
            player.transform.position = holdPosition;
        }
    }
}
