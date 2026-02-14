using UnityEngine;

public class DialogueZone : Trigger
{
    public Dialogue dialogue;
    public DialogueBox box;

    int currentLine;

    const int MaxLine = int.MaxValue;

    private void Update()
    {
        if (PlayerInputs.Interact.WasPressedThisFrame() && HasPlayer)
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
    }

    protected override void OnPlayerEnter()
    {
        Telemetry.Log("DialogueStart", dialogue.name);
        SetDialogue(true);
    }

    protected override void OnPlayerExit()
    {
        Telemetry.Log("DialogueLeave", dialogue.name);
        SetDialogue(false);
    }

    public void SetDialogue(bool visible)
    {
        if (visible)
            currentLine = 0;
        else
            currentLine = MaxLine;

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
            Telemetry.Log("DialogueFinishConversation", dialogue.name);
        }
        // Set the current line
        else
        {
            box.SetText(dialogue.lines[currentLine]);
        }
    }
}
