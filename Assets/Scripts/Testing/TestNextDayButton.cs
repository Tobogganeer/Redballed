using System.Collections;
using UnityEngine;

public class TestNextDayButton : MonoBehaviour
{
    public void LoadNextDay()
    {
        StartCoroutine(World.Days.StartLoadingDay(World.Days.CurrentDay.GetNextDay()));
    }
}
