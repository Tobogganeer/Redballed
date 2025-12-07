using System;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    private static DeliveryManager instance;
    private void Awake()
    {
        instance = this;
    }

    public GameObject deliveryPointHighlight;

    bool hasPackage;
    bool deliveredPackage;

    public static bool HasPackage => instance.hasPackage;
    public static bool DeliveredPackage => instance.deliveredPackage;

    private void Start()
    {
        deliveryPointHighlight.SetActive(false);
    }

    public static void OnPackagePickedUp()
    {
        // TODO: Highlight delivery point?
        instance.hasPackage = true;
        instance.deliveryPointHighlight.SetActive(true);
        HUD.ShowPackage();
    }

    public static void OnPackageDelivered()
    {
        instance.deliveredPackage = true;
        instance.deliveryPointHighlight.SetActive(false);
        HUD.HidePackage();
    }

    public static void SecondDayLoaded()
    {
        instance.deliveredPackage = false;
    }
}
