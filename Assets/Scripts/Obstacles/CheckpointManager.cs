using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager instance; //Public static for the checkpoint manager
    public Vector2 lastCheckpoint; //Public vector2 for the last checkpoint

    private void Awake() //Awake function
    {
        instance = this; //Finds the point to hook it up to checkpoints 
    }
}
