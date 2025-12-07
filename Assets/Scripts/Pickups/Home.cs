using UnityEngine;

public class Home : Pickup
{
    // This is so scuffed
    public GameObject day1;
    public GameObject day2;

    [Space]
    public GameObject player;
    public GameObject cam;

    protected override bool OnPickedUp()
    {
        if (DeliveryManager.DeliveredPackage)
        {
            // Teleport up into the black "loading" screen
            player.transform.position += Vector3.up * 15;
            cam.transform.position += Vector3.up * 15;

            DeliveryManager.SecondDayLoaded();

            day1.SetActive(false);
            day2.SetActive(true);

            return true;
        }

        return false;
    }
}
