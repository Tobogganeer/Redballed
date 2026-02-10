using Tobo.Audio;
using UnityEngine;

public class RedBall : Pickup
{
    protected override bool DestroyedOnPickup => true;

    protected override void OnPickedUp()
    {
        World.AddRedBall();
        Sound.Pickup.PlayDirect();
        FX.SpawnParticles(ParticleType.RedBall, transform.position);

        Telemetry.LogRaw(new RedBallTelemetry { time = Time.time, type = "RedBall", position = transform.position });
    }

    [System.Serializable]
    struct RedBallTelemetry
    {
        public float time;
        public string type;
        public Vector3 position;
    }
}
