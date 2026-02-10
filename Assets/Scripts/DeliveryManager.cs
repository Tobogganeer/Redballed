using System;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    private static DeliveryManager instance;
    private void Awake()
    {
        instance = this;
    }

    //public GameObject deliveryPointHighlight;

    bool hasPackage;
    bool deliveredPackage;

    public static bool HasPackage => instance.hasPackage;
    public static bool DeliveredPackage => instance.deliveredPackage;
}
