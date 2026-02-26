using UnityEngine;

public class Checkpoint : Trigger
{
    static Checkpoint lastCheckpoint;

    public static bool Exists => lastCheckpoint != null;
    public static Checkpoint Current => Exists ? lastCheckpoint : null;

    protected override void OnPlayerEnter()
    {
        lastCheckpoint = this;
    }
}
