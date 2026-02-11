using System;
using System.Collections;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public GameObject doubleJumpIcon;
    public GameObject dashIcon;
    public TMPro.TMP_Text redBallCount;

    private void OnEnable()
    {
        DayManager.OnDayLoaded += OnDayLoaded;
        World.OnRedBallCollected += OnRedBallCollected;

        // Disable icons until the day starts
        doubleJumpIcon.SetActive(false);
        dashIcon.SetActive(false);

        redBallCount.text = "0";
    }

    private void OnRedBallCollected(int num)
    {
        redBallCount.text = num.ToString();
    }

    private void OnDayLoaded(Days day)
    {
        // We can't guarantee we would get OnDayLoaded called after upgrades are applied (StartOfDay.cs),
        //  so we need to wait a frame before we update the HUD
        StartCoroutine(UpdateUpgradeIcons());
    }

    IEnumerator UpdateUpgradeIcons()
    {
        yield return null; // Wait one frame
        doubleJumpIcon.SetActive(Player.Upgrades.Has(Upgrades.DoubleJump));
        dashIcon.SetActive(Player.Upgrades.Has(Upgrades.Dash));
    }

    private void OnDisable()
    {
        DayManager.OnDayLoaded -= OnDayLoaded;
        World.OnRedBallCollected -= OnRedBallCollected;
    }
}
