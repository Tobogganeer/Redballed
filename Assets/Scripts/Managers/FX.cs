using UnityEngine;

public class FX : MonoBehaviour
{
    private static FX instance;
    private void Awake()
    {
        instance = this;
    }

    public ParticleSystem redBallParticles;

    public static void SpawnParticles(ParticleType type, Vector3 position)
    {
        // TODO: Properly implement different particle types (if we end up having them)
        if (type == ParticleType.RedBall)
        {
            instance.redBallParticles.transform.position = position;
            instance.redBallParticles.Play();
        }
    }
}

public enum ParticleType
{
    None,
    RedBall
}
