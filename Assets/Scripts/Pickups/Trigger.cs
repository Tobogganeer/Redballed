using UnityEngine;

public class Trigger : MonoBehaviour
{
    readonly string PlayerTag = "Player";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(PlayerTag))
            OnPlayerEnter();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(PlayerTag))
            OnPlayerExit();
    }

    protected virtual void OnPlayerEnter() { }
    protected virtual void OnPlayerExit() { }
}
