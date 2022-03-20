using GameBrains.Visualization;
using UnityEngine;

namespace Testing
{
    public class W06TestRayCastVisualizer : W06TestCastVisualizer
    {
        protected override CastVisualizer CreateInstance()
        {
            return ScriptableObject.CreateInstance<RayCastVisualizer>();
        }
    }
}