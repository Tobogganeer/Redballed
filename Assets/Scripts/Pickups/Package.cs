using UnityEngine;

public class Package : Pickup
{
    protected override bool OnPickedUp()
    {
        DeliveryManager.OnPackagePickedUp();
        return true;
    }
}
