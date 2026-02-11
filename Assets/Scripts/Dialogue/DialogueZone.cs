using UnityEngine;

public class DialogueZone : Trigger
{
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

    }
}
