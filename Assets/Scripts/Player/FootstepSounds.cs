using Tobo.Audio;
using UnityEngine;

public class FootstepSounds : MonoBehaviour
{
    public float footstepSpeed = 2.0f;
    public ParticleSystem particles;

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
            float delta = Player.Position.x - lastPosition;
            delta -= Player.Movement.ExtraVelocity.x * Time.deltaTime; // Handle moving platforms
            distanceTravelled += Mathf.Abs(delta);

            if (distanceTravelled > 1f / footstepSpeed)
            {
                ResetFootstepPosition();
                Sound.Step.PlayDirect();
                particles.Play();
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
