
using UnityEngine;

public class PlayerJumpState : PlayerAbilityState
{
    private int jumpsLeft;
    public PlayerJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData,string animationName) : base(player, stateMachine, playerData, animationName)
    {
        jumpsLeft = playerData.numberOfJumps;
    }


    public override void Enter()
    {
        base.Enter();
        player.InputHandler.UseJumpImput();
        player.SetVelocityY(playerData.jumpVelocity);
        AbilityDone = true;
        jumpsLeft--;
        player.PlayerInAirState.SetIsJumping();
    }

    public bool CheckIfCanJump()
    {
        if (jumpsLeft > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void ResetAmountOfJumps() => jumpsLeft = playerData.numberOfJumps;
    public void DecreaseAmountOfJumps() => jumpsLeft--;

}
