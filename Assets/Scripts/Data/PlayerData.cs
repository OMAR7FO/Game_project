using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData",menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("Move State")]
    public float movementVelocity = 10f;
    public float maxSpeed = 15f;
    public float accelerationTime = 3.0f;

    [Header("Jump State")]
    public float jumpVelocity = 15f;
    public int numberOfJumps = 1;
    public float jumpHeightMultiplier = 0.5f;

    [Header("In Air State")]
    public float coyoteTime = 0.2f;

    [Header("Wall Jump State")]
    public float wallJumpVelocity = 20f;
    public float wallJumpTimer = 0.4f;
    public Vector2 wallJumpAngle = new Vector2(1, 2);

    [Header("Wall Slide State")]
    public float wallSlideVelocity = 3f;

    [Header("Wall Climb State")]
    public float wallClimbVelocity = 3f;
    [Header("Dash State")]
    public float dashCoolDown = 0.5f;
    public float maxHoldTime = 1f;
    public float holdTimeScale = 0.25f;
    public float dashTime = 0.2f;
    public float dashVelocity = 30f;
    public float drag = 10f;
    public float dashEndYMultiplier = 0.2f;
    public float distanceBetweenAfterImage = 0.5f;

    [Header("Check Variable")]
    public float groundCheckRadius = 0.2f;
    public float tallOfLineWallCheck = 0.6f;
    public LayerMask groundMask;

    
}
