using UnityEngine;

public abstract class Pickup : Trigger
{
    protected abstract bool DestroyedOnPickup { get; }

    protected override void OnPlayerEnter(Collider2D collision)
    {
        OnPickedUp();
        if (DestroyedOnPickup)
            Destroy(gameObject);
    }

    protected abstract void OnPickedUp();
}
