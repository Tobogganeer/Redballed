using UnityEngine;
using UnityEngine.UI;

public class SmartPad : MonoBehaviour
{
    public Toggle socialMediaToggle;

    [Space]
    public GameObject socialMediaTab;
    public GameObject deliveryTab;

    private void Start()
    {
        socialMediaTab.SetActive(socialMediaToggle.isOn);
        deliveryTab.SetActive(!socialMediaToggle.isOn);
    }

    public void ToggleSelected(bool _)
    {
        socialMediaTab.SetActive(socialMediaToggle.isOn);
        deliveryTab.SetActive(!socialMediaToggle.isOn);
    }
}
