using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player instance;
    private void Awake()
    {
        instance = this;
    }

    [SerializeField] private PlayerController movement;
    [SerializeField] private PlayerVisuals visuals;
    [SerializeField] private PlayerUpgrades upgrades;

    public static PlayerController Movement => instance.movement;
    public static PlayerVisuals Visuals => instance.visuals;
    public static PlayerUpgrades Upgrades => instance.upgrades;
    public static Vector3 Position => instance.transform.position; // TODO: rb.position?
    public static bool Exists => instance != null;
}
