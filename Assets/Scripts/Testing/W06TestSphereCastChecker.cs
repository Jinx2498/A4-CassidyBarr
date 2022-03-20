using GameBrains.Visualization;
using UnityEngine;

namespace Testing
{
    public class W06TestSphereCastChecker : W06TestCastChecker
    {
        public override string CheckerName => "SphereCast";

        protected override CastChecker CreateInstance()
        {
            return ScriptableObject.CreateInstance<SphereCastChecker>();
        }
    }
}