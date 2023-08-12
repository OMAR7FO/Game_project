using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class Player : MonoBehaviour
{
    #region State Variables
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState PlayerIdleState { get; private set; }
    public PlayerMoveState PlayerMoveState { get; private set; }
    public PlayerJumpState PlayerJumpState { get; private set; }
    public PlayerInAirState PlayerInAirState { get; private set; }
    public PlayerDieState PlayerDieState { get; private set; }
    public PlayerLandState PlayerLandState { get; private set; }    

    public PlayerWallSlideState PlayerWallSlideState { get; private set; }
    public PlayerWallClimpState PlayerWallClimbState { get; private set; } 
    public PlayerWallGrabState PlayerWallGrabState { get; private set; }
    public PlayerWallJumpState PlayerWallJumpState { get; private set; }

    public PlayerDashState PlayerDashState { get; private set; }
    public PlayerDashStateTwo PlayerDashStateTwo { get; private set; }
    [SerializeField] private PlayerData playerData;
    #endregion

    #region Components
    public PlayerInputHandler InputHandler { get; private set; }

    public Rigidbody2D RB { get; private set; }
    public Animator Anim { get; private set; }
    public CapsuleCollider2D BoxCollider { get; private set; }

    public Transform DashDirectionIndicator { get; private set; }
    #endregion

    #region Check Transform

    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    #endregion

    #region Other Variables

    public Vector2 CurrentVelocity { get; private set; }

    public int FacingDirection { get; private set; }
    public bool isAlive { get; private set; } = true;

    private Vector2 workSpace;
    private float currentSpeed;
    public bool IsHoldingMovingPlatform { get; private set; }
    public Vector3 MovingPlatformPosition { get; private set; }
    
    #endregion

    #region Unity Callback Functions
    private void Awake()
    {
        
        StateMachine = new PlayerStateMachine();
        PlayerIdleState = new PlayerIdleState(this, StateMachine, playerData,"idle");
        PlayerMoveState = new PlayerMoveState(this, StateMachine, playerData,"move");
        PlayerJumpState = new PlayerJumpState(this, StateMachine, playerData,"move");
        PlayerInAirState = new PlayerInAirState(this, StateMachine, playerData,"jump");
        PlayerLandState = new PlayerLandState(this, StateMachine, playerData,"idle");
        PlayerWallSlideState = new PlayerWallSlideState(this, StateMachine, playerData,"wallSlide");
        PlayerWallGrabState = new PlayerWallGrabState(this, StateMachine, playerData,"wallGrab");
        PlayerWallClimbState = new PlayerWallClimpState(this, StateMachine, playerData,"wallClimp");
        PlayerWallJumpState = new PlayerWallJumpState(this, StateMachine, playerData,"jump");
        PlayerDashState = new PlayerDashState(this, StateMachine, playerData, "dash");
        PlayerDashStateTwo = new PlayerDashStateTwo(this, StateMachine, playerData, "dash");
        PlayerDieState = new PlayerDieState(this, StateMachine, playerData, "die");
    }
    private void Start()
    {
        currentSpeed = 0.0f;
        InputHandler = GetComponent<PlayerInputHandler>();
        RB = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        BoxCollider = GetComponent<CapsuleCollider2D>();

        DashDirectionIndicator = transform.Find("DashDirectionIndicator");
        FacingDirection = 1;
        StateMachine.Initalize(PlayerIdleState);
    }
    private void Update()
    {
        if (!isAlive)
            return;
        CurrentVelocity = RB.velocity;
        Die();
        StateMachine.CurrentState.LogicalUpdate();
    }
    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicalUpdate();
    }
    #endregion

    #region Set Functions

    public void SetVelocity(float velocity, Vector2 angle, float direction)
    {
        angle.Normalize();
        workSpace.Set(velocity * angle.x * direction, velocity * angle.y);
        RB.velocity = workSpace;
        CurrentVelocity = RB.velocity;
    }
    public void SetVelocity (float velocity, Vector2 direction)
    {
        workSpace = direction * velocity;
        RB.velocity = workSpace;
        CurrentVelocity = workSpace;
    }
    public void SetVelocityX(float velocity)
    {
        workSpace.Set(velocity, CurrentVelocity.y);
        RB.velocity = workSpace;
        CurrentVelocity = workSpace;
    }

    public void SetVelocityY (float velocity)
    {
        workSpace.Set(CurrentVelocity.x, velocity);
        RB.velocity = workSpace;
        CurrentVelocity = RB.velocity;
    }
    public void AddForceX (Vector2 force)
    {
        
        if (Mathf.Abs(RB.velocity.x) <= playerData.maxSpeed )
        {
            RB.AddForce(force, ForceMode2D.Force);
        }
        /*else
        {
            RB.velocity = new Vector2(playerData.maxSpeed,0);
        }*/
    }
    #endregion

    #region Check Functions
    public void CheckIfCanFlip(int xInput)
    {
        if (xInput!=0 && xInput != FacingDirection)
        {
            Flip();
        }
    }
    public bool CheckIfGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, playerData.groundCheckRadius, playerData.groundMask);
    }
    public bool CheckIfTouchingWall()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDirection, playerData.tallOfLineWallCheck, playerData.groundMask);
    }
    public bool CheckIfTouchingWallBack()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * -FacingDirection, playerData.tallOfLineWallCheck, playerData.groundMask);
    }
    #endregion

    #region Other Functions
    public void Flip()
    {
        FacingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }
    private void AnimationTrigger() => StateMachine.CurrentState.AnimationTriggers();
    // trigger this function if you want to wait the animation to end before moving to next state 
    // you can trigger this animation by animation event
    private void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, playerData.groundCheckRadius);
        Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + playerData.tallOfLineWallCheck, wallCheck.position.y));
        Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x - playerData.tallOfLineWallCheck, wallCheck.position.y));
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            IsHoldingMovingPlatform = true;
            MovingPlatformPosition = collision.transform.position;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        IsHoldingMovingPlatform = false;
    }
    private void Die()
    {
        if (BoxCollider.IsTouchingLayers(LayerMask.GetMask("Hazard")))
        {
            isAlive = false;
            StateMachine.ChangeState(PlayerIdleState);
            SetVelocityX(0);
            SetVelocityY(0);
        }
    }
    #endregion
}