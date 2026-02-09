using UnityEngine;
using TMPro;

public class UpgradeMenu : MonoBehaviour
{
    public TMP_Text redBallCountText;
    public TMP_Text nextDayText;
    public TMP_Text[] doubleJumpCostTexts;
    public TMP_Text[] dashCostTexts;

    [Space]
    public GameObject doubleJumpUnlocked;
    public GameObject doubleJumpLocked;
    public GameObject dashUnlocked;
    public GameObject dashLocked;

    [Space]
    public int doubleJumpCost = 10;
    public int dashCost = 20;

    private void Start()
    {
        int redBalls = World.CurrentRedBalls;
        redBallCountText.text = redBalls.ToString();
        nextDayText.text = World.DayManager.CurrentDay.GetNextDay().GetDayString();

        foreach (TMP_Text text in doubleJumpCostTexts)
            text.text = doubleJumpCost.ToString();

        foreach (TMP_Text text in dashCostTexts)
            text.text = dashCost.ToString();

        bool hasDoubleJump = redBalls >= doubleJumpCost;
        bool hasDash = redBalls >= dashCost;

        // Show proper UI depending on whether the upgrade is unlocked or not
        doubleJumpUnlocked.SetActive(hasDoubleJump);
        doubleJumpLocked.SetActive(!hasDoubleJump);
        dashUnlocked.SetActive(hasDash);
        dashLocked.SetActive(!hasDash);

        // TODO: Set these values for the current player
    }
}
