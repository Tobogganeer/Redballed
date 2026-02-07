using UnityEngine;

/// <summary>
/// This object will only exist on the specified day
/// </summary>
public class DayObject : MonoBehaviour
{
    public Day day;

    // TODO: Kill object before Start() etc is called
    private void OnEnable()
    {
        DayManager.OnDayLoaded += DayLoaded;
    }

    private void DayLoaded(Day loadedDay)
    {
        if (day != loadedDay)
            Destroy(gameObject);
    }

    private void OnDisable()
    {
        DayManager.OnDayLoaded -= DayLoaded;
    }
}
