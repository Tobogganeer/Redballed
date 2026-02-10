using UnityEngine;

// Scuffed way storing data in multiple places (e.g. for settings)
[CreateAssetMenu(menuName = "Scriptable Objects/Parameter")]
public class Parameter : ScriptableObject
{
    [SerializeField] private float value;
    public float Value => value; // Read-only
}
