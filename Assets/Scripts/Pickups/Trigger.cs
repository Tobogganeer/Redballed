using UnityEngine;

public class Trigger : MonoBehaviour
{
    readonly string PlayerTag = "Player";
    bool hasPlayer;

    public bool HasPlayer => hasPlayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(PlayerTag))
        {
            hasPlayer = true;
            OnPlayerEnter();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(PlayerTag))
        {
            hasPlayer = false;
            OnPlayerExit();
        }
    }

    protected virtual void OnPlayerEnter() { }
    protected virtual void OnPlayerExit() { }
}
