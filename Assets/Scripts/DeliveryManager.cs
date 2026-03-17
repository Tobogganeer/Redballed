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

    bool hasPackage;
    bool deliveredPackage;

    //public static bool HasPackage => instance.hasPackage;
    //public static bool DeliveredPackage => instance.deliveredPackage;

    public static bool HasPackage = false;
    public static bool DeliveredPackage = false;

    private static int PackagesDelivered = 0;
    public static int PackagesToDeliver = 1;

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

    private void ResetDeliveryManager()
    {
        if (HasPackage) HasPackage = false;
        if (DeliveredPackage) DeliveredPackage = false;

        PackagesDelivered = 0;
    }

    IEnumerator WaitForEndDayToShow()
    {
        yield return new WaitForSeconds(timeToShowEndDayScreen);

        World.DayManager.EndDay();
    }
}
