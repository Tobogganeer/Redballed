using UnityEngine;

public class GetRedBalls : MonoBehaviour
{
    private void OnEnable()
    {
        // Subscribe to the event
        World.OnRedBallCollected += RedBallCountUpdated;
    }

    // Called when red balls increase OR decrease
    private void RedBallCountUpdated(int numRedBalls)
    {
        Debug.Log($"Current red balls: {numRedBalls}");
    }

    private void OnDisable()
    {
        // Unsubscribe from the event
        World.OnRedBallCollected -= RedBallCountUpdated;
    }
}
