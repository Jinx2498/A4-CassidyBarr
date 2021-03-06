using System.Reflection;
using UnityEngine;

namespace GameBrains.Extensions.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public class VisibleIf : MultiPropertyAttribute
    {
        public string MethodName { get; private set; }
        public bool Negate { get; private set; }

        MethodInfo eventMethodInfo;
        FieldInfo fieldInfo;

        public VisibleIf(string methodName, bool negate = false)
        {
            MethodName = methodName;
            Negate = negate;
        }

#if UNITY_EDITOR
        public override bool IsVisible(UnityEditor.SerializedProperty property)
        {
            return Visibility(property) == !Negate;
        }

        bool Visibility(UnityEditor.SerializedProperty property)
        {
            var serializedObject = property.serializedObject;
            System.Type eventOwnerType = serializedObject.targetObject.GetType();
            string eventName = MethodName;

            // Try finding a method with the name provided:
            if (eventMethodInfo == null)
                eventMethodInfo = eventOwnerType.GetMethod(eventName,
                    BindingFlags.Instance |
                    BindingFlags.Static |
                    BindingFlags.Public |
                    BindingFlags.NonPublic);

            // If we could not find a method with that name, look for a field:
            if (eventMethodInfo == null && fieldInfo == null)
                fieldInfo = eventOwnerType.GetField(eventName,
                    BindingFlags.Instance |
                    BindingFlags.Static |
                    BindingFlags.Public |
                    BindingFlags.NonPublic);

            if (eventMethodInfo != null)
                return (bool) eventMethodInfo.Invoke(serializedObject.targetObject, null);
            if (fieldInfo != null)
                return (bool) fieldInfo.GetValue(serializedObject.targetObject);
            Debug.LogWarning(string.Format(
                $"VisibleIf: Unable to find method or field {eventName} in {eventOwnerType}"));

            return true;
        }
#endif
    }
}