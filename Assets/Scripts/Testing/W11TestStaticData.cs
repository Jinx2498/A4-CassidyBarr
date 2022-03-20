using GameBrains.Entities.EntityData;
using GameBrains.Extensions.MonoBehaviours;
using GameBrains.Extensions.Vectors;
using GameBrains.Visualization;
using UnityEngine;

namespace Testing
{
    public class W11TestStaticData : ExtendedMonoBehaviour
    {
        public StaticData staticData;
        public Transform agentTransform;
        public VectorXYZ lookTargetPosition;
        public VectorXYZ moveTargetPosition;
        public bool checkHasLineOfSight;
        public bool checkIsAtPosition;

        public RayCastVisualizer rayCastVisualizer;
        public float closeEnoughDistance = 1.0f;

        public override void Awake()
        {
            base.Awake();
            
            staticData = agentTransform;
            rayCastVisualizer = ScriptableObject.CreateInstance<RayCastVisualizer>();
        }

        public override void Update()
        {
            base.Update();
            
            if (checkHasLineOfSight)
            {
                checkHasLineOfSight = false;

                Log.Debug(
                    staticData.HasLineOfSight(
                        lookTargetPosition,
                        rayCastVisualizer,
                        true));
            }

            if (checkIsAtPosition)
            {
                checkIsAtPosition = false;

                Log.Debug(
                    staticData.IsAtPosition(
                        staticData.Position + VectorXYZ.forward, 
                        closeEnoughDistance));
            }
        }
    }
}