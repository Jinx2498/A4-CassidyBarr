#if UNITY_EDITOR
using GameBrains.Extensions.Vectors;
using UnityEditor;
using UnityEngine;

namespace GameBrains.Editor.Extensions
{
    public static partial class EditorGUIExtensions
    {
        // Makes an X, Y, and Z field for entering a VectorXYZ.
        public static VectorXYZ VectorXYZField(Rect position, string label, VectorXYZ value)
            => VectorXYZField(position, new GUIContent(label), value);
        
        // Makes an X, Y, and Z field for entering a VectorXYZ.
        public static VectorXYZ VectorXYZField(Rect position, GUIContent label, VectorXYZ value)
        {
            int controlId = GUIUtility.GetControlID(FoldoutHash, FocusType.Keyboard, position);
            position = MultiFieldPrefixLabel(position, controlId, label, 3);
            position.height = 18f;
            return VectorXYZField(position, value);
        }

        public static VectorXYZ VectorXYZField(Rect position, VectorXYZ value)
        {
            VectorXYZFloats[0] = value.x;
            VectorXYZFloats[1] = value.y;
            VectorXYZFloats[2] = value.z;
            position.height = 18f;
            EditorGUI.BeginChangeCheck();
            EditorGUI.MultiFloatField(position, XYZLabels, VectorXYZFloats);
            if (EditorGUI.EndChangeCheck())
            {
                value.x = VectorXYZFloats[0];
                value.y = VectorXYZFloats[1];
                value.z = VectorXYZFloats[2];
            }

            return value;
        }

        public static void VectorXYZField(Rect position, SerializedProperty property, GUIContent label)
        {
            int controlId = GUIUtility.GetControlID(FoldoutHash, FocusType.Keyboard, position);
            position = MultiFieldPrefixLabel(position, controlId, label, 3);
            position.height = 18f;
            SerializedProperty valuesIterator = property.Copy();
            valuesIterator.Next(true);
            MultiPropertyFieldInternal(position, XYZLabels, valuesIterator, PropertyVisibility.All);
        }
    }
}
#endif