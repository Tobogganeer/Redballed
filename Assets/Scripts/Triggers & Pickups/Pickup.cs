using UnityEngine;

public abstract class Pickup : Trigger
{
    protected abstract bool DestroyedOnPickup { get; }

    protected override void OnPlayerEnter()
    {
        OnPickedUp();
        if (DestroyedOnPickup)
            Destroy(gameObject);
    }

    protected abstract void OnPickedUp();
}
