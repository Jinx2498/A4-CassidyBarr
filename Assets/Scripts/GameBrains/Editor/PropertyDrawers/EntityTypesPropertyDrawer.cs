#if UNITY_EDITOR
using GameBrains.Editor.PropertyDrawers.Utilities;
using GameBrains.Entities.Types;
using UnityEditor;
using UnityEngine;

namespace GameBrains.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(EntityTypes))]
    public class EntityTypesPropertyDrawer : PropertyDrawer
    {
        protected static GUIContent addButtonContent = new GUIContent("+", "add element");
        protected static GUIContent deleteButtonContent = new GUIContent("\u2012", "delete");
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.serializedObject.isEditingMultipleObjects)
            {
                EditorGUILayout.LabelField(label.text + " (multiple objects selected)",
                    EditorStyles.boldLabel);
                return;
            }

            EntityTypes entityTypes =
                PropertyDrawerUtilities.GetActualObjectForSerializedProperty<EntityTypes>(
                    fieldInfo, property);
            RemoveDuplicates(entityTypes);


            var typesProperty = property.FindPropertyRelative("types");
            SerializedProperty typesArraySizeProperty
                = typesProperty.FindPropertyRelative("Array.size");

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(addButtonContent, EditorStyles.miniButton))
            {
                typesProperty.arraySize += 1;
            }

            EditorGUI.indentLevel += 1;
            if (typesProperty.arraySize == 0)
            {
                EditorGUILayout.Space(0.5f);

                EditorGUILayout.LabelField(
                    label,
                    EditorStyles.boldLabel);//,
                //GUILayout.MaxWidth(150));
            }
            else
            {
                property.isExpanded
                    = EditorGUILayout.Foldout(property.isExpanded, label,
                        EditorStyles.foldoutHeader);
            }

            GUILayout.FlexibleSpace();
            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUILayout.PropertyField(
                    typesArraySizeProperty,
                    GUIContent.none,
                    GUILayout.MaxWidth(50));
            }

            EditorGUI.indentLevel -= 1;

            EditorGUILayout.EndHorizontal();

            if (property.isExpanded)
            {
                EditorGUI.indentLevel += 2;

                DrawTypes(typesArraySizeProperty, typesProperty);

                EditorGUI.indentLevel -= 2;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 0;
        }

        void RemoveDuplicates(EntityTypes entityTypes)
        {
            // Since EntityTypes are supposed to be a set, we remove duplicates
            // except we leave a null for adding new elements.
            if (entityTypes.Count > 1) { entityTypes.RemoveDuplicates(); }
        }

        protected virtual void DrawTypes(
            SerializedProperty typesArraySizeProperty,
            SerializedProperty typesProperty)
        {
            for (int i = 0; i < typesArraySizeProperty.intValue; i++)
            {
                EditorGUILayout.BeginHorizontal();

                ShowButtons(typesProperty, i);
                EditorGUILayout.BeginVertical();

                float saveLabelWidth = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = 80;
                EditorGUILayout.PropertyField(typesProperty.GetArrayElementAtIndex(i),
                    new GUIContent($"Type {i}"), false);
                EditorGUIUtility.labelWidth = saveLabelWidth;

                EditorGUILayout.EndVertical();

                EditorGUILayout.EndHorizontal();
            }
        }

        protected virtual void ShowButtons(SerializedProperty typesProperty, int i)
        {
            var typesObject = fieldInfo.GetValue(typesProperty.serializedObject.targetObject);
            var listType = typesObject.GetType();
            var removeMethod
                = listType.GetMethod("RemoveAt", new[] { typeof(int) });

            if (removeMethod != null &&
                GUILayout.Button(
                    deleteButtonContent,
                    EditorStyles.miniButton,
                    GUILayout.ExpandWidth(false)))
            {
                removeMethod.Invoke(
                    typesObject,
                    new object[] { typesProperty.GetArrayElementAtIndex(i).intValue });
            }
        }
    }
}
#endif