using GameBrains.Entities.EntityData;
using GameBrains.EventSystem;
using UnityEngine;

namespace GameBrains.Entities
{
    public class PathfindingAgent : SteerableAgent
    {
        #region Motion

        public new PathfindingData Data
        {
            get
            { 
                if (data == null) { data = (PathfindingData) transform; }
                return data as PathfindingData;
            }
        }

        #endregion Motion

        #region Awake

        public override void Awake()
        {
            base.Awake();

            data = (PathfindingData) transform;
        }
        
        #endregion Awake
        
        #region Enable/Disable

        public override void OnEnable()
        {
            base.OnEnable();

            SubscribeToPathfindingEvents();
        }

        public override void OnDisable()
        {
            base.OnDisable();

            UnsubscribeFromPathfindingEvents();
        }

        #endregion Enable/Disable
        
        #region Subscribe and Unsubscribe

        void SubscribeToPathfindingEvents()
        {
            if (EventManager.Instance != null)
            {
                EventManager.Instance.Subscribe<PathToLocationRequestEventPayload>(
                    Events.PathToLocationRequest,
                    HandleEvent);

                EventManager.Instance.Subscribe<PathToLocationReadyEventPayload>(
                    Events.PathToLocationReady,
                    HandleEvent);

                EventManager.Instance.Subscribe<NoPathToLocationAvailableEventPayload>(
                    Events.NoPathToLocationAvailable,
                    HandleEvent);

                EventManager.Instance.Subscribe<PathToEntityWithTypesRequestEventPayload>(
                    Events.PathToEntityWithTypesRequest,
                    HandleEvent);

                EventManager.Instance.Subscribe<PathToEntityWithTypesReadyEventPayload>(
                    Events.PathToEntityWithTypesReady,
                    HandleEvent);

                EventManager.Instance.Subscribe<NoPathToEntityWithTypesAvailableEventPayload>(
                    Events.NoPathToEntityWithTypesAvailable,
                    HandleEvent);

                EventManager.Instance.Subscribe<FollowCompletedEventPayload>(
                    Events.FollowCompleted,
                    HandleEvent);

                EventManager.Instance.Subscribe<FollowFailedEventPayload>(
                    Events.FollowFailed,
                    HandleEvent);

                EventManager.Instance.Subscribe<TraversalCompletedEventPayload>(
                    Events.TraversalCompleted,
                    HandleEvent);

                EventManager.Instance.Subscribe<TraversalFailedEventPayload>(
                    Events.TraversalFailed,
                    HandleEvent);
            }
            else if (Application.isPlaying)
            {
                Debug.LogWarning("Event manager missing. Unable to subscribe to pathfinding events.");
            }
        }

        void UnsubscribeFromPathfindingEvents()
        {
            // If the EventManger got destroyed first, no need to unsubscribe
            if (EventManager.Instance != null)
            {
                EventManager.Instance.Unsubscribe<PathToLocationRequestEventPayload>(
                    Events.PathToLocationRequest,
                    HandleEvent);
                
                EventManager.Instance.Unsubscribe<PathToLocationReadyEventPayload>(
                    Events.PathToLocationReady,
                    HandleEvent);

                EventManager.Instance.Unsubscribe<NoPathToLocationAvailableEventPayload>(
                    Events.NoPathToLocationAvailable,
                    HandleEvent);
                
                EventManager.Instance.Unsubscribe<PathToEntityWithTypesRequestEventPayload>(
                    Events.PathToEntityWithTypesRequest,
                    HandleEvent);

                EventManager.Instance.Unsubscribe<PathToEntityWithTypesReadyEventPayload>(
                    Events.PathToEntityWithTypesReady,
                    HandleEvent);

                EventManager.Instance.Unsubscribe<NoPathToEntityWithTypesAvailableEventPayload>(
                    Events.NoPathToEntityWithTypesAvailable,
                    HandleEvent);
                
                EventManager.Instance.Unsubscribe<FollowCompletedEventPayload>(
                    Events.FollowCompleted,
                    HandleEvent);
                
                EventManager.Instance.Unsubscribe<FollowFailedEventPayload>(
                    Events.FollowFailed,
                    HandleEvent);
                
                EventManager.Instance.Unsubscribe<TraversalCompletedEventPayload>(
                    Events.TraversalCompleted,
                    HandleEvent);
                
                EventManager.Instance.Unsubscribe<TraversalFailedEventPayload>(
                    Events.TraversalFailed,
                    HandleEvent);
            }
        }

        #endregion Subscribe and Unsubscribe
        
        #region Act

        protected override void Act(float deltaTime)
        {
            base.Act(deltaTime);
            
            // TODO: Choose a random destination, find and follow path, rinse and repeat.
        }
        
        #endregion Act
    }
}