using System;
using UnityEngine;

public class World : MonoBehaviour
{
    private static World instance;
    private void Awake()
    {
        instance = this;
    }

    [SerializeField] int currentRedBalls;

    public static int CurrentRedBalls => instance.currentRedBalls;

    public static event Action<int> OnRedBallCollected;

    public static void AddRedBall(int num = 1)
    {
        instance.currentRedBalls += num;
        OnRedBallCollected?.Invoke(CurrentRedBalls);
    }
}
