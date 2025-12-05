using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-10)]
public class MovingPlatform : MonoBehaviour, IStompable
{
    public enum SpeedMode
    {
        FixedTime,
        FixedSpeed
    }

    public SpeedMode speedMode = SpeedMode.FixedTime;
    public float travelTime = 5f; // Seconds
    public float travelSpeed = 1f; // M/s

    [Range(0f, 1f)]
    public float startingFac = 0.5f;

    [Space]
    public Transform start;
    public Transform end;

    float travelFac;

    PlayerController currentPlayer;
    Vector3 previousPosition;

    Rigidbody2D rb;


    #region Boring Old Platform Code
    private void Start()
    {
        travelFac = startingFac;
        rb = GetComponent<Rigidbody2D>();
        transform.position = GetCurrentPosition(travelFac);
    }

    private void Update()
    {
        float distance = Vector3.Distance(start.position, end.position);
        // 1 / time for both cases
        float increase = speedMode == SpeedMode.FixedTime ? 1f / travelTime : 1f / (distance / travelSpeed);
        travelFac += increase * Time.deltaTime;

        // How much the platform has moved since last frame
        Vector2 delta = CalculateDelta();

        // Tell the player how much we've moved so it can follow
        if (currentPlayer != null)
            currentPlayer.ExtraVelocity = delta / Time.deltaTime;
    }

    private void FixedUpdate()
    {
        // Apply our current movement
        rb.MovePosition(GetCurrentPosition(Mathf.PingPong(travelFac, 1f)));
    }

    private void OnValidate()
    {
        // Change position when changing starting fac in inspector
        transform.position = GetCurrentPosition(startingFac);
    }

    private void OnDrawGizmos()
    {
        if (start)
            Gizmos.DrawWireSphere(start.position, 0.2f);
        if (end)
            Gizmos.DrawWireSphere(end.position, 0.2f);
        if (start && end)
            Gizmos.DrawLine(start.position, end.position);
    }

    Vector3 GetCurrentPosition(float fac)
    {
        return Vector3.Lerp(start.position, end.position, fac);
    }
    #endregion

    Vector2 CalculateDelta()
    {
        Vector2 delta = transform.position - previousPosition; // target - origin
        previousPosition = transform.position; // Reset for next time
        return delta;
    }


    public void PlayerLanded(PlayerController player)
    {
        currentPlayer = player;
    }

    public void PlayerLeft(PlayerController player)
    {
        // Make sure player doesn't keep sliding or flying
        currentPlayer.ExtraVelocity = Vector2.zero;
        currentPlayer = null;
    }
}
