using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadGame : Pickup
{
    protected override bool OnPickedUp()
    {
        if (DeliveryManager.DeliveredPackage)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return false;
        }

        return false;
    }
}
