using GameBrains.Entities.EntityData;
using GameBrains.EventSystem;
using GameBrains.Extensions.Vectors;
using UnityEngine;

namespace GameBrains.Entities
{
    public class SteerableAgent : MovingAgent
    {
        #region Motion

        public new SteeringData Data
        {
            get
            { 
                if (data == null) { data = (SteeringData) transform; }
                return data as SteeringData;
            }
        }

        #endregion Motion
        
        #region Awake

        public override void Awake()
        {
            base.Awake();

            data = (SteeringData) transform;
        }
        
        #endregion Awake
        
        #region OnEnable, OnDisable, OnDestroy

        // Don't Enable in Editor mode

        public override void OnEnable()
        {
            if (!Application.IsPlaying(this)) { return; }
            
            base.OnEnable();

            SubscribeToSteeringEvents();
        }

        public override void OnDisable()
        {
            if (!Application.IsPlaying(this)) { return; }
            
            base.OnDisable();

            UnsubscribeFromSteeringEvents();
        }
        
        #endregion OnEnable, OnDisable, OnDestroy

        #region Subscribe and Unsubscribe

        void SubscribeToSteeringEvents()
        {
            if (EventManager.Instance != null)
            {
                EventManager.Instance.Subscribe<LinearStopCompletedEventPayload>(
                    Events.LinearStopCompleted,
                    HandleEvent);
                
                EventManager.Instance.Subscribe<LinearSlowCompletedEventPayload>(
                    Events.LinearSlowCompleted,
                    HandleEvent);
                
                EventManager.Instance.Subscribe<SeekCompletedEventPayload>(
                    Events.SeekCompleted,
                    HandleEvent);
                
                EventManager.Instance.Subscribe<ArriveCompletedEventPayload>(
                    Events.ArriveCompleted,
                    HandleEvent);
                
                EventManager.Instance.Subscribe<ArriveBrakingStartedEventPayload>(
                    Events.ArriveBrakingStarted,
                    HandleEvent);
                
                EventManager.Instance.Subscribe<FleeCompletedEventPayload>(
                    Events.FleeCompleted,
                    HandleEvent);
                
                EventManager.Instance.Subscribe<DepartCompletedEventPayload>(
                    Events.DepartCompleted,
                    HandleEvent);
                
                EventManager.Instance.Subscribe<PursueCompletedEventPayload>(
                    Events.PursueCompleted,
                    HandleEvent);
                
                EventManager.Instance.Subscribe<EvadeCompletedEventPayload>(
                    Events.EvadeCompleted,
                    HandleEvent);
                
                EventManager.Instance.Subscribe<AngularStopCompletedEventPayload>(
                    Events.AngularStopCompleted,
                    HandleEvent);
                
                EventManager.Instance.Subscribe<AngularSlowCompletedEventPayload>(
                    Events.AngularSlowCompleted,
                    HandleEvent);
                
                EventManager.Instance.Subscribe<AlignCompletedEventPayload>(
                    Events.AlignCompleted,
                    HandleEvent);
                
                EventManager.Instance.Subscribe<AngularArriveCompletedEventPayload>(
                    Events.AngularArriveCompleted,
                    HandleEvent);
                
                EventManager.Instance.Subscribe<AngularArriveBrakingStartedEventPayload>(
                    Events.AngularArriveBrakingStarted,
                    HandleEvent);
                
                EventManager.Instance.Subscribe<WanderCompletedEventPayload>(
                    Events.WanderCompleted,
                    HandleEvent);
            }
            else
            {
                Debug.LogWarning("Event manager missing. Unable to subscribe to steering events.");
            }
        }

        void UnsubscribeFromSteeringEvents()
        {
            // If the EventManger got destroyed first, no need to unsubscribe
            if (EventManager.Instance != null)
            {
                EventManager.Instance.Unsubscribe<LinearStopCompletedEventPayload>(
                    Events.LinearStopCompleted,
                    HandleEvent);
                
                EventManager.Instance.Unsubscribe<LinearSlowCompletedEventPayload>(
                    Events.LinearSlowCompleted,
                    HandleEvent);
                
                EventManager.Instance.Unsubscribe<SeekCompletedEventPayload>(
                    Events.SeekCompleted,
                    HandleEvent);
                
                EventManager.Instance.Unsubscribe<ArriveCompletedEventPayload>(
                    Events.ArriveCompleted,
                    HandleEvent);
                
                EventManager.Instance.Unsubscribe<ArriveBrakingStartedEventPayload>(
                    Events.ArriveBrakingStarted,
                    HandleEvent);
                
                EventManager.Instance.Unsubscribe<FleeCompletedEventPayload>(
                    Events.FleeCompleted,
                    HandleEvent);
                
                EventManager.Instance.Unsubscribe<DepartCompletedEventPayload>(
                    Events.DepartCompleted,
                    HandleEvent);
                
                EventManager.Instance.Unsubscribe<PursueCompletedEventPayload>(
                    Events.PursueCompleted,
                    HandleEvent);
                
                EventManager.Instance.Unsubscribe<EvadeCompletedEventPayload>(
                    Events.EvadeCompleted,
                    HandleEvent);
                
                EventManager.Instance.Unsubscribe<AngularStopCompletedEventPayload>(
                    Events.AngularStopCompleted,
                    HandleEvent);
                
                EventManager.Instance.Unsubscribe<AngularSlowCompletedEventPayload>(
                    Events.AngularSlowCompleted,
                    HandleEvent);
                
                EventManager.Instance.Unsubscribe<AlignCompletedEventPayload>(
                    Events.AlignCompleted,
                    HandleEvent);
                
                EventManager.Instance.Unsubscribe<AngularArriveCompletedEventPayload>(
                    Events.AngularArriveCompleted,
                    HandleEvent);
                
                EventManager.Instance.Unsubscribe<AngularArriveBrakingStartedEventPayload>(
                    Events.AngularArriveBrakingStarted,
                    HandleEvent);
                
                EventManager.Instance.Unsubscribe<WanderCompletedEventPayload>(
                    Events.WanderCompleted,
                    HandleEvent);
            }
        }

        #endregion Subscribe and Unsubscribe
        
        #region Sense, think, and act
        
        protected override void Act(float deltaTime)
        {
            if (!IsPlayerControlled)
            {
                Data.CalculateSteering();
            }

            base.Act(deltaTime);
        }
        
        #endregion Sense, think, and act

        #region Spawn

        // Relocate and reactive moving entity. Reset Kinematic Data.
        public override void Spawn(VectorXYZ spawnPoint)
        {
            base.Spawn(spawnPoint);

            Data.Reset();
        }

        #endregion Spawn
    }
}