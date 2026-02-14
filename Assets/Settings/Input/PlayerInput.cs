using UnityEngine;
using UnityEngine.InputSystem;

// This class should live persistently
public class PlayerInputs : MonoBehaviour
{
    private Actions inputs;
#pragma warning disable UDR0001
    private static Actions.GameplayActions actions;
#pragma warning restore

    public static float Movement { get; private set; }
    public static InputAction Jump => actions.Jump;
    public static InputAction Dash => actions.Dash;
    public static InputAction Interact => actions.Interact;

    void Awake()
    {
        inputs = new Actions();
        actions = inputs.Gameplay;

        actions.Horizontal.performed += ctx => Movement = ctx.ReadValue<float>();
        actions.Horizontal.canceled += ctx => Movement = 0;
    }

    private void OnEnable()
    {
        inputs.Gameplay.Enable();
    }

    private void OnDisable()
    {
        inputs.Gameplay.Disable();
    }
}
