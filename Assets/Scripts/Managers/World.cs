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
    [SerializeField] DayManager days;

    public static int CurrentRedBalls => instance.currentRedBalls;
    public static DayManager DayManager => instance.days;

    public static event Action<int> OnRedBallCollected;

    public static void AddRedBall(int num = 1)
    {
        instance.currentRedBalls += num;
        OnRedBallCollected?.Invoke(CurrentRedBalls);
    }

    public static void ResetRedBalls()
    {
        instance.currentRedBalls = 0;
        OnRedBallCollected?.Invoke(CurrentRedBalls);
    }
}
