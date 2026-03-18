using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestNextDayButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nextDayText;

    private void Start()
    {
        nextDayText = gameObject.GetComponentInChildren<TextMeshProUGUI>();

        if (World.DayManager.CurrentDay == Days.DayThree && nextDayText != null)
        {
            nextDayText.text = "Ending";
        }
    }

    public void LoadNextDay()
    {
        World.DayManager.LoadDay(World.DayManager.CurrentDay.GetNextDay());
    }
}
