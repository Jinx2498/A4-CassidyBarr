using GameBrains.Visualization;
using UnityEngine;

namespace Testing
{
    public class W06TestRayCastChecker : W06TestCastChecker
    {
        public override string CheckerName => "RayCast";

        protected override CastChecker CreateInstance()
        {
            return ScriptableObject.CreateInstance<RayCastChecker>();
        }
    }
}