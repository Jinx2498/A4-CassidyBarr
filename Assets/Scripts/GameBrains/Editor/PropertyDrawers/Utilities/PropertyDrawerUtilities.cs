#if UNITY_EDITOR
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace GameBrains.Editor.PropertyDrawers.Utilities
{
    public static class PropertyDrawerUtilities
    {
        public static T GetActualObjectForSerializedProperty<T>(
            FieldInfo fieldInfo,
            SerializedProperty property) where T : class
        {
            var obj = fieldInfo.GetValue(property.serializedObject.targetObject);
            if (obj == null) { return null; }

            T actualObject;
            if (obj.GetType().IsArray)
            {
                var index =
                    System.Convert.ToInt32(
                        new string(property.propertyPath.Where(char.IsDigit).ToArray()));
                actualObject = ((T[]) obj)[index];
            }
            else
            {
                actualObject = obj as T;
            }

            return actualObject;
        }
    }
}
#endif