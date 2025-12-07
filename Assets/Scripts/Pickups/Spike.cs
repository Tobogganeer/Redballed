using UnityEngine;

public class Spike : Pickup
{
    protected override bool OnPickedUp()
    {
        if (!Checkpoint.Exists)
            throw new System.NullReferenceException("No checkpoint to restore to!");

        Checkpoint cp = Checkpoint.Current;
        PlayerController.instance.transform.position = cp.transform.position;

        return false;
    }
}
