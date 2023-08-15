
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private Camera cam;
    public Vector2 MovementInput { get; private set; }

    public Vector2 DashDirection { get; private set; }
    public Vector2 RawDashDirection { get; private set; }
    public int NormInputX  { get; private set; }    
    public int NormInputY { get; private set; }
    public bool JumpInput { get; private set; }
    public bool JumpInputStop { get; private set; }

    public bool DashInput { get; private set; }
    public bool DashInputStop { get; private set; }
    public bool GrapInput { get; private set; }
    [SerializeField] private float bufferJumpTimer = 0.2f;
    private float jumpInputStartTime;
    private float dashInputStartTime;
    private void Start()
    {
        cam = Camera.main;
    }
    private void Update()
    {
        CheckBufferInputTime();
        CheckDashInputHoldTime();
        
    }
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        MovementInput = context.ReadValue<Vector2>();
        NormInputX = Mathf.RoundToInt(MovementInput.x);
        NormInputY = Mathf.RoundToInt(MovementInput.y);
    }
    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            jumpInputStartTime = Time.time;
            JumpInput = true;
            JumpInputStop = false;
        }
        if (context.canceled)
        {
            JumpInputStop = true;
        }
    }
    // doesn't work I don't know why 
   /* public void OnDashDirectionInput(InputAction.CallbackContext context)
    {
        DashDirection = Input.mousePosition;
        DashDirection = Camera.main.ScreenToWorldPoint((Vector3)DashDirection - transform.position);
    }*/
   public void OnDashDirectionInput(InputAction.CallbackContext context)
    {
        RawDashDirection = context.ReadValue<Vector2>();
        RawDashDirection = cam.ScreenToWorldPoint((Vector3)RawDashDirection) - transform.position;
    }
    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            DashInput = true;
            DashInputStop = false;
            dashInputStartTime = Time.time;
        }
        else if (context.canceled)
        {
            DashInputStop = true;
        }
    }
    public void OnGrapInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("Ctrl has been pressed");
            GrapInput = true;
        }
        if (context.canceled)
        {
            GrapInput = false;
        }
    }

    //we made this function because it can turn to false before we make the jump action
    public void UseJumpImput() { JumpInput = false; JumpInputStop = false; }
    public void UseDashInput() { DashInput = false; DashInputStop = false; }

    private void CheckBufferInputTime()
    {
        if (Time.time > jumpInputStartTime + bufferJumpTimer)
        {
            JumpInput = false;
        }
    }
    private void CheckDashInputHoldTime()
    {
        if (Time.time > dashInputStartTime + bufferJumpTimer)
        {
            DashInput = false;
        }
    }
}
