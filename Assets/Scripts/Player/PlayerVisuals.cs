using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer bodyRenderer;
    public PlayerController playerController;

    private int isWalkingHash = Animator.StringToHash("IsWalking");
    private int isGroundedHash = Animator.StringToHash("IsGrounded");
    private int isDeadHash = Animator.StringToHash("IsDead");

    // Essentially an epsilon
    const float MinVelocityForWalkingAnim = 0.01f;

    void Update()
    {
        VisualsUpdate();
    }

    public bool IsWalking()
    {
        // Make sure we are trying to move and not up against a wall
        bool tryingToMove = playerController.PlayerInput != 0f;
        bool actuallyMoving = Mathf.Abs(playerController.ActualVelocity.x) > MinVelocityForWalkingAnim;
        return tryingToMove && actuallyMoving;
    }

    private void VisualsUpdate()
    {
        animator.SetBool(isWalkingHash, IsWalking());
        animator.SetBool(isGroundedHash, playerController.Grounded);

        //if (playerController.IsDead())
        //    animator.SetTrigger(isDeadHash);

        // Facing right by default - flip the X if we are facing left
        bodyRenderer.flipX = playerController.Facing == PlayerController.FacingDirection.Left;
    }
}
