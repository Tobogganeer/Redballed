using System.Collections;
using System.Collections.Generic;
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
    public float followSmoothing = 2f;

    Vector3 shakeOffset;

    private void LateUpdate()
    {
        Vector3 desiredPosition = target.position + new Vector3(offset.x, offset.y, transform.position.z) + shakeOffset;
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
