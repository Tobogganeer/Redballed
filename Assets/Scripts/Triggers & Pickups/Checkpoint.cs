using UnityEngine;

public class Checkpoint : Trigger
{
    public bool startingCheckpoint;

    static Checkpoint lastCheckpoint;

    public static bool Exists => lastCheckpoint != null;
    public static Checkpoint Current => Exists ? lastCheckpoint : throw new System.NullReferenceException("No starting checkpoint in level!");
    public static Vector3 CurrentRespawnPoint => Current.transform.position;

    private void Start()
    {
        if (startingCheckpoint)
            lastCheckpoint = this;
    }

    protected override void OnPlayerEnter()
    {
        lastCheckpoint = this;
    }
}
