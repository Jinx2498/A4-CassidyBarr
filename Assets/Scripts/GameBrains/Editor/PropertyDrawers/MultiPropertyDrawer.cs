#if UNITY_EDITOR
using System.Linq;
using GameBrains.Extensions.Attributes;
using UnityEditor;
using UnityEngine;

namespace GameBrains.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(MultiPropertyAttribute),true)]
    public class MultiPropertyDrawer : PropertyDrawer
    {
        MultiPropertyAttribute RetrieveAttributes()
        {
            MultiPropertyAttribute mAttribute = attribute as MultiPropertyAttribute;

            if (mAttribute == null) {  return null; }

            // Get the attribute list, sorted by "order".
            if (mAttribute.stored == null)
            {
                mAttribute.stored
                    = fieldInfo.GetCustomAttributes(
                            typeof(MultiPropertyAttribute),
                            false)
                        .OrderBy(s => ((PropertyAttribute)s).order);
            }

            return mAttribute;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            MultiPropertyAttribute mAttribute = RetrieveAttributes();

            // If the attribute is invisible, regain the standard vertical spacing.
            foreach (MultiPropertyAttribute attr in mAttribute.stored)
                if (!attr.IsVisible(property))
                    return -EditorGUIUtility.standardVerticalSpacing;

            // In case no attribute returns a modified height, return the property's default one:
            float height = base.GetPropertyHeight(property, label);

            // Check if any of the attributes wants to modify height:
            foreach (object atr in mAttribute.stored)
            {
                if (atr as MultiPropertyAttribute != null)
                {
                    var tempHeight = ((MultiPropertyAttribute)atr).GetPropertyHeight(property, label);
                    if (tempHeight.HasValue)
                    {
                        height = tempHeight.Value;
                        break;
                    }
                }
            }
            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            MultiPropertyAttribute mAttribute = RetrieveAttributes();

            // Calls to IsVisible. If it returns false for any attribute, the property will not be rendered.
            foreach (MultiPropertyAttribute attr in mAttribute.stored)
                if (!attr.IsVisible(property)) return;

            // Calls to OnPreRender before the last attribute draws the UI.
            foreach (MultiPropertyAttribute attr in mAttribute.stored)
                attr.OnPreGUI(position,property);

            // The last attribute is in charge of actually drawing something:
            ((MultiPropertyAttribute)mAttribute.stored.Last()).OnGUI(position,property,label);

            // Calls to OnPostRender after the last attribute draws the UI. These are called in reverse order.
            foreach (MultiPropertyAttribute attr in mAttribute.stored.Reverse())
                attr.OnPostGUI(position,property);
        }
    }
}
#endif