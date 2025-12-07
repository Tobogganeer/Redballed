using UnityEngine;

public class RedBall : Pickup
{
    protected override void OnPickedUp()
    {
        HUD.IncreaseRedBalls();
    }
}
