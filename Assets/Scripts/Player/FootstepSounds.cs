using Tobo.Audio;
using UnityEngine;

public class FootstepSounds : MonoBehaviour
{
    public float footstepSpeed = 2.0f;

    float lastPosition;
    float distanceTravelled;

    private void Start()
    {
        ResetFootstepPosition();
    }

    private void Update()
    {
        if (Player.Movement.Grounded)
        {
            distanceTravelled += Mathf.Abs(Player.Position.x - lastPosition);

            if (distanceTravelled > 1f / footstepSpeed)
            {
                ResetFootstepPosition();
                Sound.Step.PlayDirect();
            }
            else
                lastPosition = Player.Position.x;
        }
        else
            ResetFootstepPosition();
    }

    void ResetFootstepPosition()
    {
        lastPosition = Player.Position.x;
        distanceTravelled = 0f;
    }
}
