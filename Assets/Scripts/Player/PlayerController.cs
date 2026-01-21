using System.Collections.Generic;
using UnityEngine;
using System;
using Tobo.Audio;

public class PlayerController : MonoBehaviour
{
    #region Enums
    public enum FacingDirection
    {
        Left, Right
    }

    public enum SnapTurningMode
    {
        Instantaneous,
        IncreasedAcceleration
    }

    public enum VariableJumpGravityMode
    {
        ConstantApexTime,
        GravityAffectsApexTime
    }
    #endregion

    #region Public Variables
    [Header("Horizontal Movement")]
    public float speed = 3f;
    public float acceleration = 10f;
    public float airAcceleration = 2f; // Lower by default, like mario

    [Space]
    public bool enableSnapTurning = true;
    public bool allowSnapTurningInAir;
    public SnapTurningMode snapTurningMode; // Defaults to instantaneous
    [SnapTurningAcceleration]
    public float snapTurningAcceleration = 25f; // Very fast, nearly instant


    [Header("Groundedness")]
    public float groundCheckDistance = 0.1f;
    public float maxGroundAngle = 80f; // To prevent wall-jumping and climbing steep hills (if applicable)
    public LayerMask groundLayermask;

    [Header("Jumping")]
    public float apexHeight = 3.2f; // Units
    public float apexTime = 1f; // Seconds to reach apex height
    public float terminalSpeed = 3f;
    [Space]
    public float doubleJumpApexHeight = 2f;

    [Space]
    public bool enableVariableJumpHeight;
    public VariableJumpGravityMode variableJumpMode;
    public float variableJumpExtraGravity = 30f;

    [Space]
    [Tooltip("Let the player jump if they left the ground this many seconds ago")]
    public float coyoteTime = 0.05f; // Seconds
    [Tooltip("Let the player jump if they pressed jump this many seconds before becoming grounded")]
    public float jumpBufferingTime = 0.1f; // Seconds

    [Space]
    public float landingCameraShake = 1f;

    [Header("Step-up")]
    public bool enableStepUp = true;
    public float maxStepHeight = 0.3f;
    public float maxStepDistance = 0.3f;
    public float growBackTime = 0.3f; // Seconds

    [Header("Dash")]
    public KeyCode dashKey = KeyCode.LeftShift;
    public float dashDistance = 3f;
    public float dashTime = 1f;

    [Header("Snapshots")]
    [Range(1, 120)]
    public int capturesPerSecond = 30;
    public float secondsCaptured = 5f;

    [Header("Gizmos")]
    public bool drawGizmos;
    public GizmoDrawMode drawMode;
    public bool drawStepUpGizmos = true;
    #endregion

    #region Properties
    public Rigidbody2D RB => rb;
    public Vector2 ExtraVelocity { get; set; } // Used for moving platforms
    public Vector2 PlayerInput => playerInput;
    public Vector2 ActualVelocity => actualVelocity;
    public bool Grounded => grounded;
    public FacingDirection Facing => direction;
    #endregion

    #region Private Variables
    // Coyote time and jump buffering
    float airTime;
    float timeSinceJumpPressed = 1f; // Start at a high value so we don't jump on game start

    // Jump height
    float gravity;
    float initialJumpVelocity;
    float airJumpVelocity;
    int numAirJumps;

    Rigidbody2D rb;
    new BoxCollider2D collider;

    FacingDirection direction;
    Vector2 facingDirection => direction == FacingDirection.Left ? Vector2.left : Vector2.right;
    float desiredXVelocity;
    Vector2 playerInput;

    Vector2 lastPosition;
    Vector2 actualVelocity;

    // Used for step-up
    Vector2 baseColliderSize;
    Vector2 baseColliderOffset;
    float edgeRadius;

    Vector2 smallColliderSize => new Vector2(baseColliderSize.x + edgeRadius, baseColliderSize.y - maxStepHeight);
    Vector2 smallColliderOffset => new Vector2(baseColliderOffset.x, baseColliderOffset.y + maxStepHeight / 2f);

    Vector2 stepDetectSize => new Vector2(baseColliderSize.x, maxStepHeight);
    Vector2 stepDetectOffset => new Vector2(baseColliderOffset.x, baseColliderOffset.y - smallColliderSize.y / 2f - edgeRadius);

    bool grounded;
    IStompable currentlyStandingOn;

    bool dashing;
    float dashTimer;
    float currentDashSpeed;
    float horizontalVelocityBeforeDash;
    FacingDirection dashDirection;
    bool hasTouchedGroundSinceDashing;

    Queue<Snapshot> snapshots = new Queue<Snapshot>();
    float snapshotTimer;
    #endregion


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();

        rb.gravityScale = 0f;
        // TODO: Calculate values on the fly to allow tweaking?

        // eqs taken from lecture notes (could rearrange myself but it's already been done)
        // d = v0*dt + 1/2a * dt^2 rearranged?
        gravity = -2f * apexHeight / (apexTime * apexTime);
        initialJumpVelocity = 2f * apexHeight / apexTime;

        // TODO: This isn't correct right now (fix gravity calculation?)
        airJumpVelocity = 2f * doubleJumpApexHeight / apexTime;

        baseColliderSize = collider.size;
        baseColliderOffset = collider.offset;
        edgeRadius = collider.edgeRadius;
    }

    void Update()
    {
        //The input from the player needs to be determined and then passed in the to the MovementUpdate which should
        //manage the actual movement of the character.
        playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        TryDash();
        UpdateGrounded();
        if (!dashing)
        {
            MovementUpdate(playerInput);
            HandleVerticalVelocity();
        }
        HandleStepUp();
        CaptureSnapshots();
    }

    private void MovementUpdate(Vector2 playerInput)
    {
        // Update facing direction only if we are currently inputting
        if (playerInput.x < 0f) direction = FacingDirection.Left;
        else if (playerInput.x > 0f) direction = FacingDirection.Right;

        // Physically accurate & correct usage of lerp
        // TODO: Test out SmoothDamp or MoveTowards? Map those velocity curves?

        // Returns null if we should instantaneously snap-turn (go to 0 x velocity)
        float? currentAcceleration = CalculateAcceleration();
        if (currentAcceleration.HasValue)
            desiredXVelocity = Mathf.Lerp(desiredXVelocity, playerInput.x * speed, Time.deltaTime * currentAcceleration.Value);
        else
            desiredXVelocity = 0f;

        // Set velocity, including any extras (like moving platforms)
        // Only add the extra horizontal velocity to prevent jitters
        rb.linearVelocity = new Vector2(desiredXVelocity, rb.linearVelocity.y) + new Vector2(ExtraVelocity.x, 0);
    }

    /// <summary>
    /// Takes snap-turning and air acceleration into account
    /// </summary>
    /// <returns>Null if velocity should snap to 0, or the current acceleration otherwise</returns>
    private float? CalculateAcceleration()
    {
        // Old accel code
        //grounded ? acceleration : airAcceleration

        if (enableSnapTurning)
        {
            // Account for extra velocity like moving platforms
            float localRBXVelocity = rb.linearVelocity.x - ExtraVelocity.x;

            // Make sure we are trying to move and actually moving in opposite directions
            bool tryingToMove = playerInput.x != 0;
            bool actuallyMoving = localRBXVelocity != 0;
            bool inputDirOppositeToVelocity = Mathf.Sign(localRBXVelocity) != Mathf.Sign(playerInput.x);
            // Don't allow snap-turning in air if the option is left off
            bool validGroundedStateToSnapTurn = grounded || allowSnapTurningInAir;
            bool shouldSnapTurn = tryingToMove && actuallyMoving && inputDirOppositeToVelocity && validGroundedStateToSnapTurn;

            if (shouldSnapTurn)
            {
                return snapTurningMode == SnapTurningMode.Instantaneous ? null : snapTurningAcceleration;
            }
        }
        
        return grounded ? acceleration : airAcceleration;
    }

    private void HandleVerticalVelocity()
    {
        // Increase airtime if we are in the air (shocking)
        airTime = grounded ? 0f : airTime + Time.deltaTime;
        timeSinceJumpPressed += Time.deltaTime;

        // Reset the timer if we press space
        if (Input.GetKeyDown(KeyCode.Space))
            timeSinceJumpPressed = 0f;

        // Add normal gravity, modified by whether or not we are holding space
        float totalGravity = gravity + GetVariableJumpGravity();

        // Apply gravity, which is a negative number
        rb.linearVelocity += Vector2.up * totalGravity * Time.deltaTime;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -terminalSpeed));

        // If grounded and pressing space, jump
        //if (grounded && Input.GetKeyDown(KeyCode.Space))
        bool canGroundJump = airTime < coyoteTime;
        bool canAirJump = Player.Upgrades.Has(Upgrades.DoubleJump) && numAirJumps == 0;
        bool pressingJump = timeSinceJumpPressed < jumpBufferingTime;
        if ((canGroundJump || canAirJump) && pressingJump)
        {
            timeSinceJumpPressed = 1f; // Reset so we don't jump a million times

            float velocity = airTime < coyoteTime ? initialJumpVelocity : airJumpVelocity;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, velocity);
            Sound.Jump.PlayDirect(); // TODO: Double jump sound
            //Sound.Jump.Override().SetVolume(0.5f).Pl

            // If air jump, increment
            if (canAirJump && !canGroundJump)
                numAirJumps++;
        }
    }

    private void TryDash()
    {
        if (!Player.Upgrades.Has(Upgrades.Dash))
        {
            dashing = false;
            return;
        }

        if (dashing)
        {
            float sign = dashDirection == FacingDirection.Left ? -1f : 1f;
            rb.linearVelocity = new Vector2(sign * currentDashSpeed, 0f);
            desiredXVelocity = rb.linearVelocityX;
            dashTimer += Time.deltaTime;
            if (dashTimer > dashTime)
                dashing = false;
        }
        else if (Input.GetKeyDown(dashKey) && hasTouchedGroundSinceDashing)
        {
            dashing = true;
            dashDirection = direction;
            dashTimer = 0f;

            // Store now, we will apply after in whatever direction we are going
            //horizontalVelocityBeforeDash = Mathf.Abs(rb.linearVelocityX);
            currentDashSpeed = dashDistance / dashTime;
            // UpdateGrounded() doesn't touch this if we are dashing
            hasTouchedGroundSinceDashing = false;

            Sound.Dash.PlayDirect();
        }
    }

    private float GetVariableJumpGravity()
    {
        // Nothing if it's turned off
        if (!enableVariableJumpHeight)
            return 0f;

        if (variableJumpMode == VariableJumpGravityMode.ConstantApexTime)
        {
            // If we are going up and no longer holding jump, fall faster
            bool fallFaster =  !Input.GetKey(KeyCode.Space) && rb.linearVelocity.y > 0f;
            return fallFaster ? -variableJumpExtraGravity : 0f;
        }
        else
        {
            // If we are going up and holding space, reduce gravity
            bool decreaseGravity = Input.GetKey(KeyCode.Space) && rb.linearVelocity.y > 0f;
            // We want to make sure we aren't "boosting" the max jump height,
            //  so instead of giving less gravity, just take more away
            // (Hard to explain but the functionality is different from the other mode)
            // ^^^ This way applies more gravity when falling
            return decreaseGravity ? 0f : -variableJumpExtraGravity;
        }
    }

    private void UpdateGrounded()
    {
        // Calculate where the box cast should come from and its size
        Vector2 origin = (Vector2)transform.position + collider.offset;
        Vector2 size = collider.size + Vector2.one * collider.edgeRadius;
        // Cast it
        Physics2D.queriesHitTriggers = false;
        RaycastHit2D hit = Physics2D.BoxCast(origin, size, 0f, Vector2.down, groundCheckDistance, groundLayermask);
        Physics2D.queriesHitTriggers = true;

        if (hit)
        {
            // Check the angle to make sure we aren't hitting a wall
            float dot = Vector2.Dot(hit.normal, Vector2.up);
            float angle = Mathf.Acos(dot) * Mathf.Rad2Deg; // Get angle of collision

            bool wasGrounded = grounded;

            // Grounded if angle is less than a high number like 80
            grounded = angle < maxGroundAngle;

            if (grounded && !dashing)
            {
                numAirJumps = 0;
                hasTouchedGroundSinceDashing = true;
            }

            if (!wasGrounded && grounded) // We just landed
            {
                // Stronger the longer we are in the air for
                float scaledAirTime = Mathf.Clamp(airTime, 0.3f, 3f) * landingCameraShake;
                // Variable intensity, but always a short duration
                CameraController.Shake(scaledAirTime, 0.3f);

                // Play land sound if we are moving down
                if (rb.linearVelocityY < 0f || airTime > 1f)
                    Sound.Land.Override().SetVolume(Mathf.Clamp01(airTime * 0.5f)).Play();
            }

            // See if what we landed on is stompable
            if (hit.collider.TryGetComponent(out IStompable stompable))
            {
                // We just landed on this object
                if (stompable != currentlyStandingOn)
                {
                    // If we were on something else, we aren't anymore
                    currentlyStandingOn?.PlayerLeft(this);

                    currentlyStandingOn = stompable;
                    currentlyStandingOn.PlayerLanded(this);
                }
            }
        }
        else
        {
            // Not touching anything
            grounded = false;
            // We are outta here
            currentlyStandingOn?.PlayerLeft(this);
            currentlyStandingOn = null;
        }
    }

    /// <summary>
    /// Shrinks the player's collider when we are going to clip our toe on an edge
    /// </summary>
    private void HandleStepUp()
    {
        // If we need to grow 0.3 units in 0.1 seconds, we should grow 3 units per second (0.3 / 0.1f)
        float sizeDelta = Mathf.Abs(baseColliderSize.y - smallColliderSize.y) / growBackTime;
        float offsetDelta = Mathf.Abs(baseColliderOffset.y - smallColliderOffset.y) / growBackTime;

        // Move collider back to default size and offset
        collider.size = Vector2.MoveTowards(collider.size, baseColliderSize, Time.deltaTime * sizeDelta);
        collider.offset = Vector2.MoveTowards(collider.offset, baseColliderOffset, Time.deltaTime * offsetDelta);

        // Make sure we want to check for step ups (falling down and sideways)
        // We can check for equality in collider size because MoveTowards will make them equal
        bool moving = rb.linearVelocity.x != 0 || playerInput.x != 0;
        bool originalSize = collider.size.y == baseColliderSize.y;
        if (!enableStepUp || !moving || !originalSize || grounded)
            return;

        Vector2 position = (Vector2)transform.position;
        // We want to check what direction we are travelling, not facing
        Vector2 checkDirection = Mathf.Sign(rb.linearVelocity.x) * Vector2.right;

        float checkDistance = maxStepDistance + edgeRadius; // How far sideways we need to check

        // Check if there is a step in front of us
        RaycastHit2D stepHit = Physics2D.BoxCast(position + stepDetectOffset, stepDetectSize, 0f,
            checkDirection, checkDistance, groundLayermask);

        // If no step, stop here
        if (!stepHit)
            return;

        // Check if there is room above the step
        RaycastHit2D objectInOurFutureHome = Physics2D.BoxCast(position + baseColliderOffset + Vector2.up * maxStepHeight,
            baseColliderSize + Vector2.up * edgeRadius * 2f, 0f, checkDirection, checkDistance, groundLayermask);

        // Aw :(
        if (objectInOurFutureHome)
            return;

        // There is a step and room for us - shrink us down!
        collider.size = smallColliderSize;
        collider.offset = smallColliderOffset;
    }

    private void CaptureSnapshots()
    {
        snapshotTimer -= Time.deltaTime;

        // Has enough time passed?
        if (snapshotTimer > 0f)
            return;

        // Reset timer
        snapshotTimer = 1f / capturesPerSecond;
        snapshots.Enqueue(new Snapshot(this));

        // Clamp max length
        int maxSnapshots = (int)(secondsCaptured * capturesPerSecond);
        while (snapshots.Count > maxSnapshots)
            snapshots.Dequeue();
    }


    private void FixedUpdate()
    {
        // Calculate our actual velocity - used for walking animation
        // a = vt, v = a/t
        actualVelocity = (lastPosition - rb.position) / Time.deltaTime;
        lastPosition = rb.position;
    }

    #region Gizmos
    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            // Draw snapshots
            if (drawMode != GizmoDrawMode.None)
            {
                foreach (Snapshot snapshot in snapshots)
                {
                    // Check whether the colour should be changed
                    bool stateActive = drawMode switch
                    {
                        GizmoDrawMode.IsGrounded => snapshot.grounded,
                        GizmoDrawMode.IsInCoyoteTime => snapshot.isInCoyoteTime,
                        GizmoDrawMode.JumpIsBuffered => snapshot.jumpIsBuffered,
                        GizmoDrawMode.LeftRight => snapshot.xInput > 0f,
                        GizmoDrawMode.HoldingJump => snapshot.holdingJump,
                        _ => true
                    };

                    Gizmos.color = stateActive ? Color.green : Color.red;
                    Gizmos.DrawWireCube(snapshot.position + snapshot.currentColliderOffset, snapshot.currentColliderSize);

                    if (drawMode == GizmoDrawMode.StepUpColliderSize)
                    {
                        Gizmos.color = Color.red;
                        Gizmos.DrawWireCube(snapshot.position + baseColliderOffset, baseColliderSize);
                    }
                }
            }

            if (drawStepUpGizmos)
            {
                Gizmos.color = Color.red;
                Vector2 pos = transform.position;
                Vector2 stepDetectPos = pos + stepDetectOffset + facingDirection * (maxStepDistance + edgeRadius);
                Gizmos.DrawWireCube(stepDetectPos, stepDetectSize);

                Gizmos.color = Color.yellow;
                Vector2 checkForRoomPos = pos + baseColliderOffset + facingDirection *
                    (maxStepDistance + edgeRadius) + Vector2.up * maxStepHeight;
                Gizmos.DrawWireCube(checkForRoomPos, baseColliderSize + Vector2.up * edgeRadius * 2f);
            }
        }
    }


    public struct Snapshot
    {
        public Vector2 position;
        public Vector2 velocity;
        public float xInput;
        public bool isInCoyoteTime;
        public bool grounded;
        public bool jumpIsBuffered;
        public float time;
        public bool holdingJump;
        public Vector2 currentColliderSize;
        public Vector2 currentColliderOffset;

        public Snapshot(PlayerController player)
        {
            position = player.transform.position;
            velocity = player.rb.linearVelocity; // May not be perfectly accurate?
            xInput = player.playerInput.x;
            isInCoyoteTime = player.airTime < player.coyoteTime;
            grounded = player.grounded;
            jumpIsBuffered = player.timeSinceJumpPressed < player.jumpBufferingTime;
            time = Time.time;
            holdingJump = Input.GetKey(KeyCode.Space);
            currentColliderSize = player.collider.size;
            currentColliderOffset = player.collider.offset;
        }
    }

    public enum GizmoDrawMode
    {
        None,
        Collider,
        IsGrounded,
        IsInCoyoteTime,
        JumpIsBuffered,
        LeftRight,
        HoldingJump,
        StepUpColliderSize
    }
    #endregion
}
