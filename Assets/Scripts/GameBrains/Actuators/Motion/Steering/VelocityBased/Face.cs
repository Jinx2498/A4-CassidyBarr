using GameBrains.Entities.EntityData;
using GameBrains.Extensions.MathExtensions;
using GameBrains.Extensions.Vectors;
using GameBrains.Motion.Steering.VelocityBased;
using UnityEngine;

namespace GameBrains.Actuators.Motion.Steering.VelocityBased
{
    [System.Serializable]
    public class Face : Align
    {
        #region Creators

        public new static Face CreateInstance(SteeringData steeringData)
        {
            var steeringBehaviour = CreateInstance<Face>(steeringData);
            Initialize(steeringBehaviour);
            return steeringBehaviour;
        }

        public static Face CreateInstance(
            SteeringData steeringData,
            VectorXZ targetLocation)
        {
            var steeringBehaviour = CreateInstance<Face>(steeringData, targetLocation);
            Initialize(steeringBehaviour);
            InitializeActualLocation(steeringBehaviour, targetLocation);
            return steeringBehaviour;
        }
        
        public new static Face CreateInstance(
            SteeringData steeringData,
            Transform targetTransform)
        {
            var steeringBehaviour = CreateInstance<Face>(steeringData, (VectorXZ)targetTransform.position);
            Initialize(steeringBehaviour);
            InitializeActualTransform(steeringBehaviour, targetTransform);
            return steeringBehaviour;
        }

        public new static Face CreateInstance(
            SteeringData steeringData,
            KinematicData targetKinematicData)
        {
            var steeringBehaviour = CreateInstance<Face>(steeringData, targetKinematicData.Location);
            Initialize(steeringBehaviour);
            InitializeActualKinematicData(steeringBehaviour, targetKinematicData);
            return steeringBehaviour;
        }

        protected static void Initialize(Face steeringBehaviour)
        {
            Align.Initialize(steeringBehaviour);
            steeringBehaviour.NoStop = false;
            steeringBehaviour.NoSlow = true;
            steeringBehaviour.NeverCompletes = false;
        }

        protected static void InitializeActualKinematicData(
            Face steeringBehaviour,
            KinematicData faceTargetKinematicData)
        {
            steeringBehaviour.OtherTargetKinematicData = faceTargetKinematicData;
        }
        
        protected static void InitializeActualTransform(
            Face steeringBehaviour,
            Transform faceTargetTransform)
        {
            steeringBehaviour.OtherTargetTransform = faceTargetTransform;
        }
        
        protected static void InitializeActualLocation(
            Face steeringBehaviour,
            VectorXZ faceTargetLocation)
        {
            steeringBehaviour.OtherTargetLocation = faceTargetLocation;
        }

        #endregion Creators

        #region Members and Properties

        public virtual bool FaceActive { get; protected set; } = true;
        
        #endregion Members and Properties

        #region Steering

        public override SteeringOutput Steer()
        {
            FaceActive = false; // let Align handle things
            
            VectorXZ direction = OtherTargetLocation - SteeringData.Location;

            var atan2Degrees = Mathf.Atan2(-direction.z, direction.x) * Mathf.Rad2Deg;
            // Add 90 degrees to make relative to z-axis instead of x-axis
            TargetOrientation = Math.WrapAngle(90 + atan2Degrees);
            
            return base.Steer();
        }

        #endregion Steering
    }
}