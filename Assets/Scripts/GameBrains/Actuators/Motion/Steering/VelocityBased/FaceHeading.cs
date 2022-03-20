using GameBrains.Entities.EntityData;

namespace GameBrains.Actuators.Motion.Steering.VelocityBased
{
    [System.Serializable]
    public class FaceHeading : Face
    {
        #region Creators

        public new static FaceHeading CreateInstance(SteeringData steeringData)
        {
            var steeringBehaviour = CreateInstance<FaceHeading>(steeringData);
            Initialize(steeringBehaviour);
            return steeringBehaviour;
        }

        protected static void Initialize(FaceHeading steeringBehaviour)
        {
            Face.Initialize(steeringBehaviour);
            steeringBehaviour.NoStop = false;
            steeringBehaviour.NoSlow = true;
            steeringBehaviour.NeverCompletes = true;
        }

        #endregion Creators

        #region Members and Properties
        
        public virtual bool FaceHeadingActive { get; protected set; } = true;
        
        #endregion Members and Properties

        #region Steering

        public override SteeringOutput Steer()
        {
            FaceHeadingActive = false; // let Face handle things
            
            if (SteeringData.Speed > 0)
            {
                OtherTargetLocation = SteeringData.Location + SteeringData.Velocity / SteeringData.Speed;
            }
            else
            {
                OtherTargetLocation = SteeringData.Location + SteeringData.HeadingVectorXZ;
            }
            
            return base.Steer();
        }

        #endregion Steering
    }
}