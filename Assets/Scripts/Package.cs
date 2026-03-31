using UnityEngine;

public class Package : MonoBehaviour
{
    [SerializeField] private GameObject deliveryDoor;
    [SerializeField] private GameObject keyReminder;
    [SerializeField] private GameObject dialogueZone;

    private DeliveryDoor deliveryDoorObj;
    private DialogueZone dialogueZoneObj;

    private bool inPackageTrigger = false;

    private GameObject player;

    [SerializeField] private Vector3 packagePositionOffset = Vector3.zero;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (deliveryDoor != null) deliveryDoorObj = deliveryDoor.GetComponent<DeliveryDoor>();
        if (keyReminder != null) keyReminder.SetActive(false);

        if (dialogueZone != null)
        {
            dialogueZoneObj = dialogueZone.GetComponent<DialogueZone>();
            if (dialogueZoneObj != null) dialogueZoneObj.AssignPackageToTriggerDialogue(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if player is in the package trigger, doesn't have the package yet and the interaction key is pressed
        if (!deliveryDoorObj.GetInDoorTrigger() && inPackageTrigger && PlayerInputs.Interact.IsPressed())
        {
            // Activate the door highlight
            if (deliveryDoorObj != null) deliveryDoorObj.SetDoorHighlightActive(true, gameObject);
            if (keyReminder != null) keyReminder.SetActive(false);

            // Attach the package onto the player
            if (player != null)
            {
                gameObject.transform.SetParent(player.transform);
                gameObject.transform.position = player.transform.position + packagePositionOffset;
            }

            // Set has package to true
            DeliveryManager.HasPackage = true;
        }

        // Otherwise, check if player is in the door trigger with the package and the interaction key is pressed
        else if (deliveryDoorObj.GetInDoorTrigger() && PlayerInputs.Interact.IsPressed())
        {
            // Hide the door highlight object
            if (deliveryDoorObj != null) deliveryDoorObj.SetDoorHighlightActive(false, null);

            // Destroy the package
            gameObject.transform.SetParent(null);
            Destroy(gameObject);

            Debug.Log("Package delivered");

            // Set delivered package to true and has package to false (since it's destroyed)
            DeliveryManager.HasPackage = false;
            DeliveryManager.DeliveredPackage = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !inPackageTrigger && !DeliveryManager.HasPackage)
        {
            player = collision.gameObject;

            inPackageTrigger = true;
            if (keyReminder != null) keyReminder.SetActive(true);

            Debug.Log("In Package");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && inPackageTrigger)
        {
            player = null;

            if (keyReminder != null) keyReminder.SetActive(false);
            inPackageTrigger = false;
        }
    }
}
