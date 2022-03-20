#if UNITY_EDITOR
using GameBrains.Extensions.Vectors;
using UnityEditor;
using UnityEngine;

namespace GameBrains.Editor.Extensions
{
    public static partial class EditorGUILayoutExtensions
    {
        // Make an X, Y and Z field for entering a VectorXYZ.
        public static VectorXYZ VectorXYZField(
            string label,
            VectorXYZ value,
            params GUILayoutOption[] options)
        {
            return VectorXYZField(new GUIContent(label), value, options);
        }

        // Make an X, Y and Z field for entering a VectorXYZ.
        public static VectorXYZ VectorXYZField(
            GUIContent label,
            VectorXYZ value,
            params GUILayoutOption[] options)
        {
            return EditorGUIExtensions.VectorXYZField(
                LastRect = EditorGUILayout.GetControlRect(
                    true,
                    EditorGUI.GetPropertyHeight(SerializedPropertyType.Vector3, label),
                    EditorStyles.numberField, options),
                label,
                value);
        }
    }
}
#endif