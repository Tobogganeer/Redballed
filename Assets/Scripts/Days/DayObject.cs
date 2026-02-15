using UnityEngine;

/// <summary>
/// This object will only exist on the specified days
/// </summary>
public class DayObject : MonoBehaviour
{
    public Days days;

    // TODO: Kill object before Start() etc is called
    private void OnEnable()
    {
        DayManager.OnDayLoaded += DayLoaded;
    }

    private void DayLoaded(Days loadedDay)
    {
        if (!days.HasFlag(loadedDay))
            Destroy(gameObject);
    }

    private void OnDisable()
    {
        DayManager.OnDayLoaded -= DayLoaded;
    }
}
