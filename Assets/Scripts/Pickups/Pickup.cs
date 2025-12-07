using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    readonly string PlayerTag = "Player";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(PlayerTag))
        {
            OnPickedUp();
            Destroy(gameObject);
        }
    }

    protected abstract void OnPickedUp();
}
