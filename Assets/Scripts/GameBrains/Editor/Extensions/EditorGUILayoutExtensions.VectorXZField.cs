#if UNITY_EDITOR
using GameBrains.Extensions.Vectors;
using UnityEditor;
using UnityEngine;

namespace GameBrains.Editor.Extensions
{
    public static partial class EditorGUILayoutExtensions
    {
        // Make an X and Z field for entering a VectorXZ.
        public static VectorXZ VectorXZField(
            string label,
            VectorXZ value,
            params GUILayoutOption[] options)
        {
            return VectorXZField(new GUIContent(label), value, options);
        }

        // Make an X and Z field for entering a VectorXZ.
        public static VectorXZ VectorXZField(
            GUIContent label,
            VectorXZ value,
            params GUILayoutOption[] options)
        {
            return EditorGUIExtensions.VectorXZField(
                LastRect = EditorGUILayout.GetControlRect(
                    true,
                    EditorGUI.GetPropertyHeight(SerializedPropertyType.Vector2, label),
                    EditorStyles.numberField,
                    options),
                label,
                value);
        }
    }
}
#endif