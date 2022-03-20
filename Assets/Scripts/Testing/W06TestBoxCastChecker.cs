using GameBrains.Visualization;
using UnityEngine;

namespace Testing
{
    public class W06TestBoxCastChecker : W06TestCastChecker
    {
        public override string CheckerName => "BoxCast";

        protected override CastChecker CreateInstance()
        {
            return ScriptableObject.CreateInstance<BoxCastChecker>();
        }
    }
}