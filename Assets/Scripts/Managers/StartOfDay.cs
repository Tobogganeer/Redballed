using UnityEngine;

/// <summary>
/// Gives upgrades and resets red balls
/// </summary>
public class StartOfDay : MonoBehaviour
{
    public Parameter doubleJumpCost;
    public Parameter dashCost;

    private void OnEnable()
    {
        DayManager.OnDayLoaded += OnDayLoaded;
    }

    private void OnDayLoaded(Days day)
    {
        int redBalls = World.CurrentRedBalls;
        bool hasDoubleJump = redBalls >= doubleJumpCost.Value;
        bool hasDash = redBalls >= dashCost.Value;

        Player.Upgrades.Reset();
        if (hasDoubleJump)
            Player.Upgrades.Give(Upgrades.DoubleJump);
        if (hasDash)
            Player.Upgrades.Give(Upgrades.Dash);

        World.ResetRedBalls();

        Telemetry.Log("Day", day.ToString());
        Telemetry.Log("Upgrades", Player.Upgrades.Current.ToString());
    }

    private void OnDisable()
    {
        DayManager.OnDayLoaded -= OnDayLoaded;
    }
}
