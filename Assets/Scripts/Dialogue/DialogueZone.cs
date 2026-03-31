using UnityEngine;

public class DialogueZone : Trigger
{
    public Dialogue dialogue;
    public DialogueBox box;

    int currentLine;

    const int MaxLine = int.MaxValue;

    [SerializeField] private GameObject packageRef;

    private void Update()
    {
        if (PlayerInputs.Interact.WasPressedThisFrame() && HasPlayer && box.IsVisible())
        {
            Telemetry.Log("DialogueInteraction", dialogue.name);

            // If the box is typing, skip to the end of the line
            if (!box.IsFinished)
                box.Finish();
            else
            {
                // Show next line
                currentLine++;
                UpdateCurrentLine();
            }
                
        }

        if (box.IsVisible() && !DeliveryManager.HasPackage) box.Hide();
    }

    public void AssignPackageToTriggerDialogue(GameObject package)
    {
        packageRef = package;
    }

    protected override void OnPlayerEnter()
    {
        if (packageRef != null && DeliveryManager.HasPackage && !box.IsVisible())
        {
            Telemetry.Log("DialogueStart", dialogue.name);
            SetDialogue();
        }
    }

    protected override void OnPlayerExit()
    {
        if (packageRef != null && box.IsVisible())
        {
            Telemetry.Log("DialogueLeave", dialogue.name);
            box.Hide();
        }
    }

    public void SetDialogue()
    {
        UpdateCurrentLine();
    }

    void UpdateCurrentLine()
    {
        if (dialogue == null || dialogue.lines == null)
        {
            box.Hide();
            return;
        }

        // If we are at the end of the conversation, close the box
        if (currentLine >= dialogue.lines.Length)
        {
            box.Hide();
            // TODO: Makes sure the box is visible before logging to telemetry
            Telemetry.Log("DialogueFinishConversation", dialogue.name);
        }
        // Set the current line
        else
        {
            box.SetText(dialogue.lines[currentLine]);
        }
    }
}
