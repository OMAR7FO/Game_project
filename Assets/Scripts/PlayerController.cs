using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region IDK
    private float movementInputDirection;
    private float gravityScale;
    private float jumpTimer;
    private float turnTimer;
    private float wallJumpTimer = 0;
    private float coyoteTimeCounter;
    private int jumpsLeft;
    private int facingDirection = 1;
    private int lastWallJumpDirection;
    private bool isFacingRight = true;
    private bool isWallSliding;
    private bool isWalking;
    private bool isGrounded;
    private bool isTouchingWall;
    private bool canNormalJump;
    private bool canWallJump;
    private bool canMove;
    private bool canFlip;
    private bool isTryingToJump;
    private bool hasWallJumped;
    #endregion

    #region Config
    [SerializeField] float fallMultiplier;
    [SerializeField] float movementSpeed = 10f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float groundCheckRadius;
    [SerializeField] float coyoteTime = 0.1f; 
    [SerializeField] float turnTimerSet = 0.1f;
    [SerializeField] float wallCheckLineTall;
    [SerializeField] float wallSlidingSpeed;
    [SerializeField] float airDragForce;
    [SerializeField] float movementForceInAir;
    [SerializeField] float wallJumpTimerSet = 0.5f;
    [SerializeField] float jumpHeight = 0.5f;
    [SerializeField] float jumpTimerSet = 0.15f;
    [SerializeField] float wallJumpForce;
    [SerializeField] int numberOfJumps = 1;
    [SerializeField] Vector2 wallJumpDirection;
    [SerializeField] Transform wallCheck;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask whatIsGround;
    #endregion

    #region Cache
    private Rigidbody2D rb;
    #endregion


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gravityScale = rb.gravityScale;
        jumpsLeft = numberOfJumps;
        wallJumpDirection.Normalize();
    }
    void Update()
    {
        CheckInput();
        CheckMovementDirection();
        CheckIfCanJump();
        CheckIfWallSliding();
        CheckJumpTimer();
    }
    private void FixedUpdate()
    {
        CheckSurrounding();
        ApplyMovement();
    }
    private void CheckIfCanJump()
    {
        if (isGrounded && rb.velocity.y <= 0 || isWallSliding)
        {
            jumpsLeft = numberOfJumps;
        }
        if (jumpsLeft <= 0)
        {
            canNormalJump = false;
        }
        else
        {
            canNormalJump = true;
        }
        if (isTouchingWall)
        {
            canWallJump = true;
        }
        else
        {
            canWallJump = false;
        }
    }
    private void CheckJumpTimer()
    {
        if (jumpTimer > 0)
        {
            //wall Jump
            if (!isGrounded && isTouchingWall && movementInputDirection != 0 && movementInputDirection != facingDirection)
            {
                WallJump();
            }
            else if (coyoteTimeCounter > 0f)
            {
                NormalJump();
            }
        }
        // decrease the buffer timer 
        if (isTryingToJump)
        {
            jumpTimer -= Time.deltaTime;
        }
        // prevent the player to go back to the wall after jump and decrease the velocity to down 
        if (wallJumpTimer > 0)
        {
            if (hasWallJumped && movementInputDirection == -lastWallJumpDirection)
            {
                rb.velocity = new Vector2(rb.velocity.x, -2.0f);
                hasWallJumped = false;
            }
            else if (wallJumpTimer <= 0)
            {
                hasWallJumped = false;
            }
            else
            {
                wallJumpTimer -= Time.deltaTime;
            }
        }
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else 
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
        
    }
    private void NormalJump()
    {
        if (canNormalJump)
        {
            jumpTimer = 0;
            isTryingToJump = false;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpsLeft--;
            coyoteTimeCounter = 0f;
            //rb.AddForce(Vector2.up * jumpForce,ForceMode2D.Impulse);
        }
    }
    private void WallJump()
    {
        if (canWallJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            jumpTimer = 0;
            isTryingToJump = false;
            isWallSliding = false;
            jumpsLeft--;
            Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDirection.x * movementInputDirection, wallJumpForce * wallJumpDirection.y);
            rb.AddForce(forceToAdd, ForceMode2D.Impulse);
            turnTimer = 0;
            canMove = true;
            canFlip = true;
            hasWallJumped = true;
            lastWallJumpDirection = -facingDirection;
            wallJumpTimer = wallJumpTimerSet;
        }
    }
    private void CheckIfWallSliding()
    {
        if (!isGrounded && movementInputDirection == facingDirection && isTouchingWall && rb.velocity.y < 0)
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }
    }
    private void CheckInput()
    {
        movementInputDirection = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump"))
        {
            if (coyoteTimeCounter > 0f || jumpsLeft > 0)
            {
                NormalJump();
            }
            else
            {
                //initialize the jump buffer timer
                jumpTimer = jumpTimerSet;
                isTryingToJump = true;
            }
        }
        if (Input.GetButtonUp("Jump"))
        {
            // the jump height 
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpHeight);
        }
        if (Input.GetButtonDown("Horizontal") && isTouchingWall)
        {
            if (!isGrounded && movementInputDirection != facingDirection)
            {
                canMove = false;
                canFlip = false;
                turnTimer = turnTimerSet;
            }
        }
        if (!canMove)
        {
            turnTimer -= Time.deltaTime;
            if (turnTimer <= 0)
            {
                canMove = true;
                canFlip = true;
            }
        }
    }
    private void CheckMovementDirection()
    {
        if (isFacingRight && movementInputDirection < 0)
        {
            Flip();
        }
        else if (!isFacingRight && movementInputDirection > 0)
        {
            Flip();
        }
        if (rb.velocity.x != 0)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
    }
    private void Flip()
    {
        // not fall while wallSliding
        if (!isWallSliding && canFlip)
        {
            facingDirection *= -1;
            isFacingRight = !isFacingRight;
            transform.Rotate(0, 180, 0);
        }
        // we can't use local scale because wall checking don't change the direction 
        //transform.localScale = new Vector3(transform.localScale.x * (-1), transform.localScale.y , transform.localScale.z ); 
    }
    private void ApplyMovement()
    {
        // apply the air force while jumping
        if (!isGrounded && !isWallSliding && movementInputDirection == 0)
        {
            rb.velocity = new Vector2(rb.velocity.x * airDragForce, rb.velocity.y);
        }
        // basic movement in ground
        else if (canMove)
        {
            rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);
        }
        
        if (isWallSliding)
        {
            if (rb.velocity.y < -wallSlidingSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlidingSpeed);
            }
        }
        #region Increase Gravity While Falling
        if (rb.velocity.y < Mathf.Epsilon && !isWallSliding)
        {
            rb.gravityScale = gravityScale * fallMultiplier;
        }
        else
        {
            rb.gravityScale = gravityScale;
        }
        #endregion
    }
    private void CheckSurrounding()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckLineTall, whatIsGround);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckLineTall, wallCheck.position.y, wallCheck.position.z));
    }
}
