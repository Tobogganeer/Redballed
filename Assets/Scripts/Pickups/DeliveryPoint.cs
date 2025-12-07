using UnityEngine;

public class DeliveryPoint : Pickup
{
    protected override bool OnPickedUp()
    {
        // If we have a package, deliver it and destroy ourselves
        if (DeliveryManager.HasPackage)
        {
            DeliveryManager.OnPackageDelivered();
            return true;
        }

        // If we have no package, do nothing
        return false;
    }
}
