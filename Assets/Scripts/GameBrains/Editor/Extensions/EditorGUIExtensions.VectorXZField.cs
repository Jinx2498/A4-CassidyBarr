#if UNITY_EDITOR
using GameBrains.Extensions.Vectors;
using UnityEditor;
using UnityEngine;

namespace GameBrains.Editor.Extensions
{
    public static partial class EditorGUIExtensions
    {
        // Makes an X and Z field for entering a VectorXZ.
        public static VectorXZ VectorXZField(Rect position, string label, VectorXZ value)
            => VectorXZField(position, new GUIContent(label), value);
        
        // Makes an X and Y field for entering a Vector2.
        public static VectorXZ VectorXZField(Rect position, GUIContent label, VectorXZ value)
        {
            int controlId = GUIUtility.GetControlID(FoldoutHash, FocusType.Keyboard, position);
            position = MultiFieldPrefixLabel(position, controlId, label, 2);
            position.height = 18f;
            return VectorXZField(position, value);
        }

        public static VectorXZ VectorXZField(Rect position, VectorXZ value)
        {
            VectorXZFloats[0] = value.x;
            VectorXZFloats[1] = value.z;
            position.height = 18f;
            EditorGUI.BeginChangeCheck();
            EditorGUI.MultiFloatField(position, XZLabels, VectorXZFloats);
            if (EditorGUI.EndChangeCheck())
            {
                value.x = VectorXZFloats[0];
                value.z = VectorXZFloats[1];
            }
            return value;
        }
        
        public static void VectorXZField(Rect position, SerializedProperty property, GUIContent label)
        {
            int controlId = GUIUtility.GetControlID(FoldoutHash, FocusType.Keyboard, position);
            position = MultiFieldPrefixLabel(position, controlId, label, 2);
            position.height = 18f;
            SerializedProperty valuesIterator = property.Copy();
            valuesIterator.Next(true);
            MultiPropertyFieldInternal(
                position, XZLabels, valuesIterator, PropertyVisibility.All);
        }
    }
}
#endif