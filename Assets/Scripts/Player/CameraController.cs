using System.Collections;
using System.Collections.Generic;
using Tobo.Util;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private static CameraController instance;
    private void Awake()
    {
        instance = this;
    }

    public Transform target;
    public Vector2 offset;
    public Camera followCamera;

    [Space]
    public Vector2 velocityPrediction = new Vector2(1.0f, 0.2f);
    public Vector2 maxVelocityPrediction = new Vector2(5.0f, 2.0f);
    public float velocitySmoothing = 0.4f;

    [Space]
    public float followSmoothing = 2f;

    Vector3 shakeOffset;
    Vector3 velocity;

    private void LateUpdate()
    {
        Vector3 playerVelocity = Player.Movement.ActualVelocity;
        playerVelocity *= velocityPrediction;
        playerVelocity.x = Mathf.Clamp(playerVelocity.x, -maxVelocityPrediction.x, maxVelocityPrediction.x);
        playerVelocity.y = Mathf.Clamp(playerVelocity.y, -maxVelocityPrediction.y, maxVelocityPrediction.y);

        velocity = Vector3.Lerp(velocity, playerVelocity, Time.deltaTime * velocitySmoothing);

        Vector3 desiredPosition = target.position + new Vector3(offset.x, offset.y, transform.position.z) + shakeOffset;
        desiredPosition -= velocity;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, 1f - Mathf.Exp(-followSmoothing * Time.deltaTime));

        transform.position = smoothedPosition;
    }

    public static void Shake(float intensity, float duration)
    {
        instance.StartCoroutine(instance.ShakeCoroutine(intensity, duration));
    }

    private IEnumerator ShakeCoroutine(float intensity, float duration)
    {
        // TODO: Better shake
        float timer = 0f;
        while (timer < duration)
        {
            // Lower intensity over time
            shakeOffset = Random.insideUnitCircle * (intensity * (1f - timer / duration));
            timer += Time.deltaTime;
            yield return null;
        }

        shakeOffset = Vector3.zero;
    }
}
