using System;
using UnityEngine;

public class DeliveryDoor : MonoBehaviour
{
    [SerializeField] private GameObject doorHighlight;
    [SerializeField] private GameObject keyReminder;

    [SerializeField] private GameObject pickedUpPackage;

    private bool inDoorTrigger = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (doorHighlight != null) doorHighlight.SetActive(false);
        if (keyReminder != null) keyReminder.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !inDoorTrigger && 
            DeliveryManager.HasPackage && pickedUpPackage != null)
        {
            if (keyReminder != null) keyReminder.SetActive(true);
            inDoorTrigger = true;

            Debug.Log("In Door with Package");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && inDoorTrigger)
        {
            if (keyReminder != null) keyReminder.SetActive(false);
            inDoorTrigger = false;
        }
    }

    public void SetDoorHighlightActive(bool active, GameObject package)
    {
        if (doorHighlight != null) doorHighlight.SetActive(active);
        if (!active && keyReminder != null) keyReminder.SetActive(active);
        pickedUpPackage = package;
    }

    public bool GetInDoorTrigger()
    {
        return inDoorTrigger;
    }
}
