using UnityEngine;

namespace Tobo.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class SceneAttribute : PropertyAttribute { }
}