using GameBrains.Entities;
using GameBrains.Entities.Types;
using GameBrains.EventSystem;
using GameBrains.Extensions.MonoBehaviours;
using GameBrains.Extensions.Vectors;
using UnityEngine;

namespace Testing
{
    public sealed class W23BTestPathToLocation : ExtendedMonoBehaviour
    {
        #region Members and Properties
        
        [SerializeField] bool testPathToLocation;
        [SerializeField] PathfindingAgent pathfindingAgent;
        [SerializeField] VectorXZ destination;

        [SerializeField] bool testPathToEntityWithType;
        [SerializeField] EntityTypes entityTypes;
        
        #endregion Members and Properties

        #region Enable/Disable

        public override void OnEnable()
        {
            base.OnEnable();

            if (EventManager.Instance != null)
            {
                EventManager.Instance.Subscribe<PathToLocationReadyEventPayload>(
                    Events.PathToLocationReady,
                    OnPathToLocationReady);

                EventManager.Instance.Subscribe<NoPathToLocationAvailableEventPayload>(
                    Events.NoPathToLocationAvailable,
                    OnNoPathToLocationAvailable);

                EventManager.Instance.Subscribe<PathToEntityWithTypesReadyEventPayload>(
                    Events.PathToEntityWithTypesReady,
                    OnPathToEntityWithTypeReady);

                EventManager.Instance.Subscribe<NoPathToEntityWithTypesAvailableEventPayload>(
                    Events.NoPathToEntityWithTypesAvailable,
                    OnNoPathToEntityWithTypeAvailable);
            }
            else
            {
                Debug.LogWarning("Event manager missing. Unable to subscribe to pathfinding events.");
            }
        }

        public override void OnDisable()
        {
            base.OnDisable();

            if (EventManager.Instance != null)
            {
                EventManager.Instance.Unsubscribe<PathToLocationReadyEventPayload>(
                    Events.PathToLocationReady,
                    OnPathToLocationReady);

                EventManager.Instance.Unsubscribe<NoPathToLocationAvailableEventPayload>(
                    Events.NoPathToLocationAvailable,
                    OnNoPathToLocationAvailable);

                EventManager.Instance.Unsubscribe<PathToEntityWithTypesReadyEventPayload>(
                    Events.PathToEntityWithTypesReady,
                    OnPathToEntityWithTypeReady);

                EventManager.Instance.Unsubscribe<NoPathToEntityWithTypesAvailableEventPayload>(
                    Events.NoPathToEntityWithTypesAvailable,
                    OnNoPathToEntityWithTypeAvailable);
            }
        }
        
        #endregion Enable/Disable

        #region Update

        public override void Update()
        {
            if (testPathToLocation)
            {
                testPathToLocation = false;

                EventManager.Instance.Enqueue(
                    Events.PathToLocationRequest,
                    new PathToLocationRequestEventPayload(pathfindingAgent, destination));
            }

            if (testPathToEntityWithType)
            {
                testPathToEntityWithType = false;

                EventManager.Instance.Enqueue(
                    Events.PathToEntityWithTypesRequest,
                    new PathToEntityWithTypesRequestEventPayload(pathfindingAgent, entityTypes));
            }
        }

        #endregion Update

        #region Events

        bool OnPathToLocationReady(Event<PathToLocationReadyEventPayload> eventArguments)
        {
            PathToLocationReadyEventPayload payload = eventArguments.EventData;

            if (payload.pathfindingAgent != pathfindingAgent) // event not for us
            {
                Log.Debug($"Path ready for someone else: {payload.pathfindingAgent}");
                return false;
            }

            Log.Debug($"Path ready for us: {payload.path}");

            return true;
        }

        bool OnNoPathToLocationAvailable(Event<NoPathToLocationAvailableEventPayload> eventArguments)
        {
            NoPathToLocationAvailableEventPayload payload = eventArguments.EventData;

            if (payload.pathfindingAgent != pathfindingAgent) // event not for us
            {
                Log.Debug($"Path not available for someone else: {payload.pathfindingAgent}");
                return false;
            }

            Log.Debug("Path not available for us");
            return true;
        }

        bool OnPathToEntityWithTypeReady(Event<PathToEntityWithTypesReadyEventPayload> eventArguments)
        {
            PathToEntityWithTypesReadyEventPayload payload = eventArguments.EventData;

            if (payload.pathfindingAgent != pathfindingAgent) // event not for us
            {
                Log.Debug($"Path ready for someone else: {payload.pathfindingAgent}");
                return false;
            }

            Log.Debug($"Path ready for us: {payload.path}");
            return true;
        }

        bool OnNoPathToEntityWithTypeAvailable(Event<NoPathToEntityWithTypesAvailableEventPayload> eventArguments)
        {
            NoPathToEntityWithTypesAvailableEventPayload payload = eventArguments.EventData;

            if (payload.pathfindingAgent != pathfindingAgent) // event not for us
            {
                Log.Debug($"Path not available for someone else: {payload.pathfindingAgent}");
                return false;
            }

            Log.Debug("Path not available for us");
            return true;
        }

        #endregion Events
    }
}