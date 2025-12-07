using UnityEngine;

public class Checkpoint : Pickup
{
    static Checkpoint lastCheckpoint;

    public static bool Exists => lastCheckpoint != null;
    public static Checkpoint Current => Exists ? lastCheckpoint : null;

    protected override bool OnPickedUp()
    {
        lastCheckpoint = this;
        return false;
    }
}
