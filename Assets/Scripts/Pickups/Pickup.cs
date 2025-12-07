using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    readonly string PlayerTag = "Player";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(PlayerTag))
        {
            bool shouldDestroy = OnPickedUp();
            if (shouldDestroy)
                Destroy(gameObject);
        }
    }

    /// <summary>
    /// Called when the player runs over this object. Return whether the object should be destroyed.
    /// </summary>
    /// <returns></returns>
    protected abstract bool OnPickedUp();
}
