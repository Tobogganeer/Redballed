using UnityEngine;

public class Spike : Trigger
{
    protected override void OnPlayerEnter()
    {
        if (!Checkpoint.Exists)
            throw new System.NullReferenceException("No checkpoint to restore to!");

        Checkpoint cp = Checkpoint.Current;
        //PlayerController.instance.transform.position = cp.transform.position;
        HUD.Time += 10;
    }
}
