using UnityEngine;

public class Package : Pickup
{
    protected override void OnPickedUp()
    {
        DeliveryManager.OnPackagePickedUp();
    }
}
