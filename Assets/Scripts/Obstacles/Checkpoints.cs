using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    public GameObject spawnPoint; //Public gameobject for spawnpoint
    private void OnTriggerEnter2D(Collider2D other) //Trigger enter
    {
        if (other.CompareTag("Player")) //Detects the player
        {
            CheckpointManager.instance.lastCheckpoint = spawnPoint.transform.position; //Makes the point the spawnpoint for the player
            //Debug.Log("Checkpoint"); //Writes Checkpoint in the console
        }
    }
}
