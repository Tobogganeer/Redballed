using UnityEngine;

public class TestDaySpawnOrder : MonoBehaviour
{
    private void OnEnable()
    {
        DayManager.OnDayLoaded += DayManager_OnDayLoaded;
    }

    private void DayManager_OnDayLoaded(Days obj)
    {
        Debug.Log("Day loaded. Player exists? " + Player.Position);
    }

    private void OnDisable()
    {
        DayManager.OnDayLoaded -= DayManager_OnDayLoaded;
    }
}
