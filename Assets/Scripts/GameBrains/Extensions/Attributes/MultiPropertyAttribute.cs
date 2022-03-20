using System.Linq;
using UnityEngine;

namespace GameBrains.Extensions.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public abstract class MultiPropertyAttribute : PropertyAttribute
    {
#if UNITY_EDITOR
        public IOrderedEnumerable<object> stored = null;

        public virtual void OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label)
        {
            UnityEditor.EditorGUI.PropertyField(position,property,label);
        }

        public virtual void OnPreGUI(Rect position, UnityEditor.SerializedProperty property){}
        public virtual void OnPostGUI(Rect position, UnityEditor.SerializedProperty property){}

        public virtual bool IsVisible(UnityEditor.SerializedProperty property){return true;}

        public virtual float? GetPropertyHeight(UnityEditor.SerializedProperty property, GUIContent label)
        {
            return null;
        }
#endif
    }
}