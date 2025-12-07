using UnityEngine;

public class RedBall : Pickup
{
    protected override bool OnPickedUp()
    {
        HUD.IncreaseRedBalls();
        return true;
    }
}
