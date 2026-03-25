using UnityEngine;

public class Spikes : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision2D) //Collision function
    {
        if (collision2D.gameObject.CompareTag("Player")) //dectects if player is on collider
        {
            collision2D.transform.position = CheckpointManager.instance.lastCheckpoint; //Respawns the player on the last checkpoint
            //Debug.Log("Respawn"); //Writes Respawn in the console
        }
    }
}
