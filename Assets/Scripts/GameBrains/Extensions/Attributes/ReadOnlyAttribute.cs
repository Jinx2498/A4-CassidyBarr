using UnityEngine;

namespace GameBrains.Extensions.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public class ReadOnlyAttribute : MultiPropertyAttribute
    {
#if UNITY_EDITOR
        public override void OnPreGUI(Rect position, UnityEditor.SerializedProperty property)
        {
            UnityEngine.GUI.enabled = false;
        }

        public override void OnPostGUI(Rect position, UnityEditor.SerializedProperty property)
        {
            UnityEngine.GUI.enabled = true;
        }
#endif
    }
}