using UnityEngine;

public class DialogueSpawner : Pickup
{
    public void Show()
    {

    }

    public void Hide()
    {

    }

    protected override bool OnPickedUp()
    {
        Show();
        return false;
    }
}
