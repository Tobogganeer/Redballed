using UnityEngine;

public class TestNextDayButton : MonoBehaviour
{
    public void LoadNextDay()
    {
        World.DayManager.LoadDay(World.DayManager.CurrentDay.GetNextDay());
    }
}
