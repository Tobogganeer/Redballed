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

        Telemetry.LogRaw("RedBall", transform.position);
    }
}
