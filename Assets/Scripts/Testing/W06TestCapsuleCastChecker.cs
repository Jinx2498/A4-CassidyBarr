using GameBrains.Visualization;
using UnityEngine;

namespace Testing
{
    public class W06TestCapsuleCastChecker : W06TestCastChecker
    {
        public override string CheckerName => "CapsuleCast";

        protected override CastChecker CreateInstance()
        {
            return ScriptableObject.CreateInstance<CapsuleCastChecker>();
        }
    }
}