
using UnityEngine;

public class PlayerDashStateTwo : PlayerAbilityState

{
    public bool CanDash { get; private set; }
    private bool isHolding;
    private bool dashInputStop;
    private float lastDashTime;
    private Vector2 dashDirection;
    private Vector2 dashDirectionInput;
    private Camera cam;
    private Vector2 lastAfterImagePosition;

    public override void Enter()
    {
        base.Enter();
        CanDash = false;
        player.InputHandler.UseDashInput();
        isHolding = true;
        dashDirection = Vector2.right * player.FacingDirection;
        Time.timeScale = playerData.holdTimeScale;
        startedTime = Time.unscaledTime;
        cam = Camera.main;
        player.DashDirectionIndicator.gameObject.SetActive(true);
    }

    public override void Exit()
    {
        base.Exit();
        if (player.CurrentVelocity.y > 0)
        {
            player.SetVelocityY(player.CurrentVelocity.y * playerData.dashEndYMultiplier);
        }
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();
        dashDirectionInput = Input.mousePosition;
        dashDirectionInput = cam.ScreenToWorldPoint((Vector3)dashDirectionInput) - player.transform.position;
        if (!isExitingState)
        {
            player.Anim.SetFloat("yVelocity", player.CurrentVelocity.y);
            if (isHolding)
            {
                dashInputStop = player.InputHandler.DashInputStop;
                player.Anim.SetBool("dash", false);
                //dashDirectionInput = Input.mousePosition;
                if (dashDirectionInput != Vector2.zero)
                {
                    dashDirection = dashDirectionInput;
                    dashDirection.Normalize();
                }
                float angle = Vector2.SignedAngle(Vector2.right, dashDirection);
                player.DashDirectionIndicator.rotation = Quaternion.Euler(0f, 0f, angle - 45f);
                player.CheckIfCanFlip(Mathf.RoundToInt(dashDirection.x));
                if (dashInputStop || Time.unscaledTime > startedTime + playerData.maxHoldTime)
                {
                    isHolding = false;
                    Time.timeScale = 1;
                    // to track how time we dash
                    startedTime = Time.time;
                    player.RB.drag = playerData.drag;
                    player.SetVelocity(playerData.dashVelocity, dashDirection);
                    player.Anim.SetBool("dash", true);
                    PlaceAfterImage();
                }
            }
            else
            {
                player.SetVelocity(playerData.dashVelocity, dashDirection);
                player.DashDirectionIndicator.gameObject.SetActive(false);
                player.RB.drag = 0f;
                CheckIfShouldPlaceAfterImage();
                if (Time.time >= startedTime + playerData.dashTime)
                {
                    AbilityDone = true;
                    lastDashTime = Time.time;
                }
            }
        }
    }
    public PlayerDashStateTwo(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animationName) : base(player, stateMachine, playerData, animationName)
    {
    }
    private void CheckIfShouldPlaceAfterImage()
    {
        if (Vector2.Distance(player.transform.position,lastAfterImagePosition) >= playerData.distanceBetweenAfterImage)
        {
            PlaceAfterImage();
        }
    }
    private void PlaceAfterImage()
    {
        PlayerAfterImagePool.Instance.GetFromPool();
        lastAfterImagePosition = player.transform.position;
    }

    public bool CheckIfCanDash()
    {
        return CanDash && Time.time > lastDashTime + playerData.dashCoolDown;
    }
    public void ResetDashState()
    {
        CanDash = true;
    }
}
