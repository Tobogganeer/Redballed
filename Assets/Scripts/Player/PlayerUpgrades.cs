using UnityEngine;

public class PlayerUpgrades : MonoBehaviour
{
    [SerializeField]
    Upgrades current;

    public Upgrades Current => current;

    public bool Has(Upgrades upgrade) => current.HasFlag(upgrade);
    public void Give(Upgrades upgrade) => current |= upgrade;
    public void Remove(Upgrades upgrade) => current &= ~upgrade;
    public void Reset() => current = Upgrades.None;
}

[System.Flags]
public enum Upgrades
{
    None = 0,
    DoubleJump = 1 << 0,
    Dash = 1 << 1,
}
