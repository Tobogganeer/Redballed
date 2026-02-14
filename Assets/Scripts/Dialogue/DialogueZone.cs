using UnityEngine;

public class DialogueZone : Trigger
{
    public Dialogue dialogue;
    public DialogueBox box;

    int currentLine;

    const int MaxLine = int.MaxValue;

    protected override void OnPlayerEnter()
    {
        SetDialogue(true);
    }

    protected override void OnPlayerExit()
    {
        SetDialogue(false);
    }

    public void SetDialogue(bool visible)
    {
        if (visible)
            currentLine = 0;
        else
            currentLine = MaxLine;

        SetCurrentLine();
    }

    void SetCurrentLine()
    {
        if (dialogue == null || dialogue.lines == null)
        {
            box.Hide();
            return;
        }

        // If we are at the end of the conversation, close the box
        if (currentLine >= dialogue.lines.Length)
            box.Hide();
        // Set the current line
        else
            box.SetText(dialogue.lines[currentLine]);
    }
}
