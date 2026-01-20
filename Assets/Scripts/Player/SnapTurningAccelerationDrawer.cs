using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;

[CustomPropertyDrawer(typeof(SnapTurningAccelerationAttribute))]
public class SnapTurningAccelerationDrawer : PropertyDrawer
{
    // Get FieldInfo for mode
    readonly Type PlayerControllerType = typeof(PlayerController);
    readonly FieldInfo SnapTurningModeFieldInfo = typeof(PlayerController).GetField(nameof(PlayerController.snapTurningMode));

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        
        // Make sure this is in the correct script at all
        if (fieldInfo.DeclaringType != PlayerControllerType)
            return;

        // Make sure it's the acceleration float
        if (property.propertyType != SerializedPropertyType.Float)
            return;

        // Get that shi with reflection
        object rawSnapTurningMode = SnapTurningModeFieldInfo.GetValue(property.serializedObject.targetObject);
        PlayerController.SnapTurningMode snapTurningMode = (PlayerController.SnapTurningMode)rawSnapTurningMode;

        // At long last disable the gui
        bool guiState = GUI.enabled;
        if (snapTurningMode == PlayerController.SnapTurningMode.Instantaneous)
            GUI.enabled = false;

        property.floatValue = EditorGUI.FloatField(position, property.displayName, property.floatValue);

        GUI.enabled = guiState;
    }
}
#endif

/// <summary>
/// Greys out this value if snapTurningMode is set to <see cref="PlayerController.SnapTurningMode.Instantaneous"/> 
/// </summary>
public class SnapTurningAccelerationAttribute : PropertyAttribute { }