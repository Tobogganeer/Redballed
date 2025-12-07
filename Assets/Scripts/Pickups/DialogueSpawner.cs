using UnityEngine;

public class DialogueSpawner : MonoBehaviour, IStompable
{
    public GameObject dialogue;

    private void Start()
    {
        dialogue.SetActive(false);
    }

    public void PlayerLanded(PlayerController player)
    {
        dialogue.SetActive(true);
    }

    public void PlayerLeft(PlayerController player)
    {
        dialogue.SetActive(false);
    }
}
