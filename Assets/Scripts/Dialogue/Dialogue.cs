using UnityEngine;

/// <summary>
/// A series of sentences
/// </summary>
[CreateAssetMenu(menuName = "Scriptable Objects/Dialogue")]
public class Dialogue : ScriptableObject
{
    public string[] lines;
}
