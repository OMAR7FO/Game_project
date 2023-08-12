using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class PlayerDashState : PlayerAbilityState
{
    public bool CanDash { get; private set; }
    private bool isHolding;
    private float lastDashTime;
    private bool dashInputStop;
    private Vector2 direction;
    public PlayerDashState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animationName) : base(player, stateMachine, playerData, animationName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        CanDash = false;
        player.InputHandler.UseDashInput();
        isHolding = true;
        Time.timeScale = playerData.holdTimeScale;
        startedTime = Time.unscaledTime;
        player.DashDirectionIndicator.gameObject.SetActive(true);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();
        dashInputStop = player.InputHandler.DashInputStop;
        if (!isExitingState)
        {
            if (isHolding)
            {
                
                Vector2 moustPosition = Input.mousePosition;
                Vector2 lookAtPoistion = Camera.main.ScreenToWorldPoint(moustPosition);
                direction = ((Vector3) lookAtPoistion -  player.DashDirectionIndicator.transform.position).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                player.DashDirectionIndicator.rotation = Quaternion.AngleAxis(angle - 45,Vector3.forward);
                player.CheckIfCanFlip(Mathf.RoundToInt(direction.x));
                if (dashInputStop || Time.unscaledTime >= startedTime + playerData.maxHoldTime)
                {
                    isHolding = false;
                    startedTime = Time.time;
                }
            }
            else
            {
                Time.timeScale = 1;
                player.DashDirectionIndicator.gameObject.SetActive(false);
                player.SetVelocity(playerData.dashVelocity, direction);
                if (Time.time >= startedTime + playerData.dashTime)
                {
                    AbilityDone = true;
                    lastDashTime = Time.time;
                }
            }
        }
    }

    public bool CheckIfCanDash()
    {
        return CanDash && Time.time > lastDashTime + playerData.dashCoolDown;
    }
    public void ResetDashState() => CanDash = true;

}
