using System;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    private static DeliveryManager instance;
    private void Awake()
    {
        instance = this;
    }

    bool hasPackage;
    bool deliveredPackage;

    public static bool HasPackage => instance.hasPackage;
    public static bool DeliveredPackage => instance.deliveredPackage;

    public static void OnPackagePickedUp()
    {
        // TODO: Highlight delivery point?
        throw new NotImplementedException();
    }

    public static void OnPackageDelivered()
    {
        throw new NotImplementedException();
    }
}
