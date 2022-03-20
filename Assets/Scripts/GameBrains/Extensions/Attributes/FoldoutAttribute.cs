/*===============================================================
Product:    ActorBasedBehaviors
Developer:  Dimitry Pixeye - info@pixeye,games
Company:    Homebrew
Date:       5/17/2018 6:31 AM
================================================================*/

using UnityEngine;

namespace GameBrains.Extensions.Attributes
{
    public class FoldoutAttribute : PropertyAttribute
    {
        public string name;
        public bool foldEverything;

        // Adds the property to the specified foldout group.
        public FoldoutAttribute(string name, bool foldEverything = false)
        {
            this.foldEverything = foldEverything;
            this.name = name;
        }
    }
}