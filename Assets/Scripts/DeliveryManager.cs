using System;
using System.Collections;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    //private static DeliveryManager instance;
    private void Awake()
    {
        //instance = this;
    }

    //public GameObject deliveryPointHighlight;

    //bool hasPackage;
    //bool deliveredPackage;

    //public static bool HasPackage => instance.hasPackage;
    //public static bool DeliveredPackage => instance.deliveredPackage;

    public static bool HasPackage = false;
    public static bool DeliveredPackage = false;

    [Header("Package Delivery Parameters")]
    private int PackagesDelivered = 0;
    [SerializeField] private int PackagesToDeliver = 1;

    [Header("Load Time")]
    [SerializeField] private float timeToShowEndDayScreen = 1.0f;

    //public static bool HasPackage => instance.hasPackage;
    //public static bool DeliveredPackage => instance.deliveredPackage;

    private void Update()
    {
        // Check if the package is delivered to increment the packages delivered amount
        if (DeliveredPackage)
        {
            PackagesDelivered++;

            // If the player reaches the packages to deliver amount for that day, wait for some time to show end day screen
            if (PackagesDelivered >= PackagesToDeliver) StartCoroutine(WaitForEndDayToShow());

            DeliveredPackage = false; // Set back to false
        }
    }

    IEnumerator WaitForEndDayToShow()
    {
        yield return new WaitForSeconds(timeToShowEndDayScreen);

        World.DayManager.EndDay();
    }
}
