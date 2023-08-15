
using UnityEngine;

public class PlayerInAirState : PlayerState
{
    private bool isGrounded;
    private Vector2 input;
    private bool isTouchingWall;
    private bool isTouchingWallBack;
    private bool jumpInput;
    private bool dashInput;
    private bool grapInput;
    private bool coyoteTime;
    private bool isJumping;
    private bool jumpInputStop;
    public PlayerInAirState(Player player, PlayerStateMachine stateMachine, PlayerData playerData,string animationName) : base(player, stateMachine, playerData,animationName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isGrounded = player.CheckIfGrounded();

        isTouchingWall = player.CheckIfTouchingWall();
        isTouchingWallBack = player.CheckIfTouchingWallBack();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();
        CheckCoyoteTime();
        input = player.InputHandler.MovementInput;
        jumpInput = player.InputHandler.JumpInput;
        jumpInputStop = player.InputHandler.JumpInputStop;
        grapInput = player.InputHandler.GrapInput;
        dashInput = player.InputHandler.DashInput;
        CheckJumpMultiplier();
        /*if (player.BoxCollider.IsTouchingLayers(LayerMask.GetMask("Hazard"))) {
            stateMachine.ChangeState(player.PlayerDieState);
        }*/
        if (isGrounded)
        {
            stateMachine.ChangeState(player.PlayerIdleState);
        }
        // we should put the wall jump if before the normal jump 
        else if (jumpInput && (isTouchingWall || isTouchingWallBack) ){
            player.PlayerWallJumpState.DetermineWallJumpDirection(isTouchingWall);
            stateMachine.ChangeState(player.PlayerWallJumpState);
        }
        else if (jumpInput && player.PlayerJumpState.CheckIfCanJump())
        {
            stateMachine.ChangeState(player.PlayerJumpState);
        }
        else if (isTouchingWall && grapInput)
        {
            stateMachine.ChangeState(player.PlayerWallGrabState);
        }
        else if (isTouchingWall && player.InputHandler.NormInputX == player.FacingDirection && player.CurrentVelocity.y < 0)
        {
            stateMachine.ChangeState(player.PlayerWallSlideState);
        }
        /*else if (dashInput && player.PlayerDashState.CheckIfCanDash())
        {
            stateMachine.ChangeState(player.PlayerDashState);
        }*/
        else if (dashInput && player.PlayerDashStateTwo.CheckIfCanDash())
        {
            stateMachine.ChangeState(player.PlayerDashStateTwo);
        }
        else
        {
            player.CheckIfCanFlip((int)input.x);
            player.SetVelocityX(input.x * playerData.movementVelocity);
            player.Anim.SetFloat("yVelocity", player.CurrentVelocity.y);
        }
    }

    public override void PhysicalUpdate()
    {
        base.PhysicalUpdate();
    }
    public void CheckCoyoteTime()
    {
        if (coyoteTime && Time.time > startedTime + playerData.coyoteTime)
        {
            coyoteTime = false;
            player.PlayerJumpState.DecreaseAmountOfJumps();
        }
    }
    public void CheckJumpMultiplier()
    {
        if (isJumping)
        {
            if (jumpInputStop)
            {
                player.SetVelocityY(player.CurrentVelocity.y * playerData.jumpHeightMultiplier);
                isJumping = false;
            }
            else if (player.CurrentVelocity.y <= 0)
            {
                isJumping = false;
            }
        }
    }
    public void StartCoyoteTime() => coyoteTime = true;
    public void SetIsJumping() => isJumping = true;
}
