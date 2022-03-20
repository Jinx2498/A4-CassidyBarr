using GameBrains.Visualization;
using UnityEngine;

namespace Testing
{
    public class W06TestSphereCastVisualizer : W06TestCastVisualizer
    {
        protected override CastVisualizer CreateInstance()
        {
            return ScriptableObject.CreateInstance<SphereCastVisualizer>();
        }
    }
}