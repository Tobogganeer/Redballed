using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{
    private static CameraController instance;
    private void Awake()
    {
        instance = this;
    }

    public Transform target;
    public Tilemap tilemap;
    public Vector2 offset;
    public Camera followCamera;

    [Space]
    public float followSmoothing = 2f;

    Vector2 viewportHalfSize;

    float leftBoundary;
    float rightBoundary;
    float bottomBoundary;

    Vector3 shakeOffset;

    void Start()
    {
        // Recalculate bounds
        tilemap.CompressBounds();
        CalculateBounds();
    }

    private void CalculateBounds()
    {
        viewportHalfSize = new Vector2(followCamera.aspect * followCamera.orthographicSize, followCamera.orthographicSize);

        leftBoundary = tilemap.transform.position.x + (tilemap.cellBounds.min.x * tilemap.transform.localScale.x) + viewportHalfSize.x;
        rightBoundary = tilemap.transform.position.x + (tilemap.cellBounds.max.x * tilemap.transform.localScale.x) - viewportHalfSize.x;
        bottomBoundary = tilemap.transform.position.y + (tilemap.cellBounds.min.y * tilemap.transform.localScale.y) + viewportHalfSize.y;
    }

    private void LateUpdate()
    {
        Vector3 desiredPosition = target.position + new Vector3(offset.x, offset.y, transform.position.z) + shakeOffset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, 1f - Mathf.Exp(-followSmoothing * Time.deltaTime));

        smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, leftBoundary, rightBoundary);
        smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, bottomBoundary, smoothedPosition.y);

        transform.position = smoothedPosition;
    }

    public static void Shake(float intensity, float duration)
    {
        instance.StartCoroutine(instance.ShakeCoroutine(intensity, duration));
    }

    private IEnumerator ShakeCoroutine(float intensity, float duration)
    {
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
