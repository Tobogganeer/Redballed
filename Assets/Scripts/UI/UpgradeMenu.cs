using UnityEngine;

public class UpgradeMenu : MonoBehaviour
{
    public TMPro.TMP_Text redBallCountText;
    public TMPro.TMP_Text nextDayText;

    private void OnEnable()
    {
        DayManager.OnInterdaySceneLoaded += OnLoaded;
    }

    private void OnLoaded()
    {
        redBallCountText.text = World.CurrentRedBalls.ToString();
        nextDayText.text = World.DayManager.CurrentDay.GetDayString();
    }

    private void OnDisable()
    {
        DayManager.OnInterdaySceneLoaded -= OnLoaded;
    }
}
