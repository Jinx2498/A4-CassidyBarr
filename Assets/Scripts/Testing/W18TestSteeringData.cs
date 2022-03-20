using GameBrains.Entities.EntityData;
using GameBrains.Extensions.MathExtensions;
using GameBrains.Extensions.MonoBehaviours;
using GameBrains.Extensions.Vectors;
using UnityEngine;

namespace Testing
{
    // Attach to a game object which will be moved during the test.
    // TODO: Need better testing
    public sealed class W18TestSteeringData : ExtendedMonoBehaviour
    {
        public bool testCalculatePosition;
        public bool testOrientation;
        public bool testAccumulateVelocities;
        public Transform steerableTransform;
        public SteeringData steeringData;
        public VectorXZ velocity = new VectorXZ(0, 1);
        public float angularVelocity = 1;
        public VectorXZ acceleration = new VectorXZ(0, 0);
        public float orientation = 360 + 180; // Should get wrapped to 180
        VectorXZ startingLocation;

        public override void Awake()
        {
            base.Awake();

            if (steerableTransform != null)
            {
                steeringData = steerableTransform;
                startingLocation = steeringData.Location;
            }
        }

        public override void Update()
        {
            base.Update();

            if (steeringData == null) { return; }

            if (testCalculatePosition)
            {
                testCalculatePosition = false;
                TestCalculatePosition();
            }

            if (testOrientation)
            {
                testOrientation = false;
                TestOrientation();
            }

            if (testAccumulateVelocities)
            {
                testAccumulateVelocities = false;
                TestAccumulateVelocities();
            }
        }

        public void TestCalculatePosition()
        {
            steeringData.Reset();
            startingLocation = steeringData.Location;
            // velocity and acceleration are capped.
            steeringData.Velocity = velocity;
            steeringData.Acceleration = acceleration;

            float deltaTime = 2;

            steeringData.CalculatePosition(deltaTime);

            if (VerbosityDebug)
            {
                var limitedVelocity = Math.LimitMagnitude(velocity, steeringData.MaximumSpeed);
                var limitedAcceleration = Math.LimitMagnitude(acceleration, steeringData.MaximumAcceleration);
                Log.Debug($"V: should be {limitedVelocity}. It is {steeringData.Velocity}");
                Log.Debug($"A: should be {limitedAcceleration}. It is {steeringData.Acceleration}");
                Log.Debug($"dt: is {deltaTime}");
                var calculatedPosition = startingLocation + limitedVelocity * deltaTime + limitedAcceleration * (deltaTime * deltaTime) / 2;
                Log.Debug($"P: should be {calculatedPosition}. It is {steeringData.Position}");
            }
        }

        public void TestOrientation()
        {
            steeringData.Reset();
            startingLocation = steeringData.Location;
            steeringData.Orientation = orientation;

            if (VerbosityDebug)
            {
                Log.Debug($"O: should be {Math.WrapAngle(orientation)}. Is {steeringData.Orientation}");
                // TODO: Expected values should be calculated
                Log.Debug($"H: should be (0, 0, -1). Is {steeringData.HeadingVectorXYZ}");
                Log.Debug($"H2: should be (0, -1). Is {steeringData.HeadingVectorXZ}");
            }
        }

        public void TestAccumulateVelocities()
        {
            steeringData.Reset();
            startingLocation = steeringData.Location;
            steeringData.Velocity = velocity;
            steeringData.AngularVelocity = angularVelocity;
            steeringData.SetAccumulatedVelocities(velocity, 1);
            steeringData.AccumulateVelocities(new VectorXZ(0, 0), 0);

            if (VerbosityDebug)
            {
                var limitedVelocity = Math.LimitMagnitude(velocity, steeringData.MaximumSpeed);
                var limitedAcceleration = Math.LimitMagnitude(acceleration, steeringData.MaximumAcceleration);
                Log.Debug($"V: should be {limitedVelocity}. Is {steeringData.Velocity}");
                Log.Debug($"A: should be {limitedAcceleration}. Is {steeringData.Acceleration}");
                Log.Debug($"P: should be {startingLocation}. Is {steeringData.Position}");
                Log.Debug($"O: should be {Math.WrapAngle(orientation)}. Is {steeringData.Orientation}");
                // TODO: Expected values should be calculated
                Log.Debug($"H: should be (0, 0, -1). Is {steeringData.HeadingVectorXYZ}");
                Log.Debug($"H2: should be (0,-1). Is {steeringData.HeadingVectorXZ}");
            }
            
            steeringData = transform;
            steeringData.Velocity = velocity;
            steeringData.AngularVelocity = angularVelocity;
            steeringData.SetAccumulatedVelocities(new VectorXZ(0, 1), 1);
            steeringData.AccumulateVelocities(new VectorXZ(1, 1), 1);

            if (VerbosityDebug)
            {
                var limitedVelocity = Math.LimitMagnitude(velocity, steeringData.MaximumSpeed);
                var limitedAcceleration = Math.LimitMagnitude(acceleration, steeringData.MaximumAcceleration);
                Log.Debug($"V: should be {limitedVelocity}. Is {steeringData.Velocity}");
                Log.Debug($"A: should be {limitedAcceleration}. Is {steeringData.Acceleration}");
                Log.Debug($"P: should be {startingLocation}. Is {steeringData.Position}");
                // TODO: Expected values should be calculated
                Log.Debug($"O: should be {Math.WrapAngle(orientation)}. Is {steeringData.Orientation}");
                Log.Debug($"H: should be (0, 0, -1). Is {steeringData.HeadingVectorXYZ}");
                Log.Debug($"H2: should be (0, -1). Is {steeringData.HeadingVectorXZ}");
            }
        }
    }
}