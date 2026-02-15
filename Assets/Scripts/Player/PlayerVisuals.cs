using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer bodyRenderer;
    public PlayerController playerController;

    private readonly int MovingHash = Animator.StringToHash("moving");
    private readonly int GroundedHash = Animator.StringToHash("grounded");
    private readonly int SlideHash = Animator.StringToHash("slide");
    private readonly int DashHash = Animator.StringToHash("dash");
    private readonly int JumpHash = Animator.StringToHash("jump");
    private readonly int DownwardsHash = Animator.StringToHash("downwards");

    // Essentially an epsilon
    const float MinVelocityForWalkingAnim = 0.01f;
    const float TimeSlidingThreshold = 0.1f;

    float timeSliding;

    void Update()
    {
        if (IsSliding())
            timeSliding += Time.deltaTime;
        else
            timeSliding = 0f;

            VisualsUpdate();
    }

    public bool IsMoving()
    {
        // Make sure we are trying to move and not up against a wall
        bool tryingToMove = playerController.PlayerInput != 0f;
        bool actuallyMoving = Mathf.Abs(playerController.ActualVelocity.x) > MinVelocityForWalkingAnim;
        return tryingToMove && actuallyMoving;
    }

    public bool IsSliding()
    {
        // See if we are trying to move, but can't
        bool tryingToMove = playerController.PlayerInput != 0f;
        bool actuallyMoving = Mathf.Abs(playerController.ActualVelocity.x) > MinVelocityForWalkingAnim;
        return tryingToMove && !actuallyMoving;

    }

    private void VisualsUpdate()
    {
        animator.SetBool(MovingHash, IsMoving());
        animator.SetBool(GroundedHash, playerController.Grounded);
        animator.SetBool(SlideHash, timeSliding > TimeSlidingThreshold);
        animator.SetBool(DownwardsHash, playerController.RB.linearVelocityY < 0f);
        // TODO: Slide (when pushing up against a wall)

        //if (playerController.IsDead())
        //    animator.SetTrigger(isDeadHash);

        // Facing right by default - flip the X if we are facing left
        bodyRenderer.flipX = playerController.Facing == PlayerController.FacingDirection.Left;
    }

    private void OnEnable()
    {
        playerController.OnDash += PlayerController_OnDash;
        playerController.OnJump += PlayerController_OnJump;
    }

    private void PlayerController_OnJump()
    {
        animator.SetTrigger(JumpHash);
    }

    private void PlayerController_OnDash()
    {
        animator.SetTrigger(DashHash);
    }

    private void OnDisable()
    {
        playerController.OnDash -= PlayerController_OnDash;
        playerController.OnJump -= PlayerController_OnJump;
    }
}
