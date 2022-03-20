using UnityEngine;

namespace GameBrains.Actuators.Motion.Movers.UsingVector3.SimpleMovers
{
    public sealed class RigidbodySimpleMover : SimpleMover
    {
        [SerializeField] bool useForce;

        public override void Start()
        {
            base.Start();
            SetupRigidbody();
        }

        protected override void CalculatePhysics(float deltaTime)
        {
            if (Speed < minimumSpeed) { return; }
            
            if (useForce)
            {
                throw new System.NotImplementedException(
                    "Homework: How can we use Agent.Rigidbody.AddForce to move properly?");
            }

            Agent.Rigidbody.velocity = Direction * Speed;
        }
    }
}