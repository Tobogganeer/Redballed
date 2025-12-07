using UnityEngine;

public class DeliveryPoint : Pickup
{
    protected override bool OnPickedUp()
    {
        // If we have a package, deliver it
        if (DeliveryManager.HasPackage)
            DeliveryManager.OnPackageDelivered();

        return false;
    }
}
