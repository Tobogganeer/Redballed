using UnityEngine;

public class Home : Pickup
{
    protected override bool OnPickedUp()
    {
        if (DeliveryManager.DeliveredPackage)
        {
            // TODO: Load next day
            return true;
        }

        return false;
    }
}
