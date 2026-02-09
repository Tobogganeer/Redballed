using UnityEngine;

public class TestNextDayButton : MonoBehaviour
{
    public void LoadNextDay()
    {
        World.Days.LoadDay(World.Days.CurrentDay.GetNextDay());
    }
}
