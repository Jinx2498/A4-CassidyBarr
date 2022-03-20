using GameBrains.Entities;
using GameBrains.EventSystem;
using UnityEngine;

namespace GameBrains.FiniteStateMachine.PathfindingStates
{
    [CreateAssetMenu(fileName = "Start", menuName = "StateMachine/PathfindingStates/Start")]
    public class Start : State
    {
        public override void Enter(StateMachine stateMachine)
        {
            base.Enter(stateMachine);

            var pathfindingAgent = stateMachine.Owner as PathfindingAgent;
            if (pathfindingAgent == null)
            {
                Debug.LogError("State requires owner to be a PathfindingAgent.");
                return;
            }

            stateMachine.StateData = new PathfindingStateData();
            PathfindingStateData pathfindingStateData = (PathfindingStateData)stateMachine.StateData;
            pathfindingStateData.pathfindingAgent = pathfindingAgent;
            pathfindingStateData.pathfindingData = pathfindingAgent.Data;
            pathfindingStateData.pathRequested = false;
            stateMachine.ChangeState(StateManager.Lookup(typeof(ChooseTarget)));
        }

        public override void Execute(StateMachine stateMachine)
        {
            base.Execute(stateMachine);

            // var pathfindingAgent = stateMachine.Owner as PathfindingAgent;
            // if (pathfindingAgent == null)
            // {
            //     Debug.LogError("State requires owner to be a PathfindingAgent.");
            //     stateMachine.ChangeState(stopState);
            //     return;
            // }
            //
            // if (!pathfindingAgent.Data.pathRequested && pathfindingAgent.Data.CurrentPath == null)
            // {
            //     int targetNumber = Random.Range(1, 6); // [1..5]
            //     var targetObject = GameObject.Find("Target" + targetNumber);
            //     var targetLocation = (VectorXZ) targetObject.transform.position;
            //     pathfindingAgent.Data.FindPathTo(targetLocation);
            //     pathfindingAgent.Data.pathRequested = true;
            // }
        }

        public override void Exit(StateMachine stateMachine)
        {
            base.Exit(stateMachine);
        }

        public override bool HandleEvent<TEvent>(
            StateMachine stateMachine,
            Event<TEvent> eventArguments)
        {
            // var pathfindingAgent = stateMachine.Owner as PathfindingAgent;
            // if (pathfindingAgent == null)
            // {
            //     Debug.LogError("State requires owner to be a PathfindingAgent.");
            //     stateMachine.ChangeState(stopState);
            //     return base.HandleEvent(stateMachine, eventArguments);
            // }
            //
            // if (eventArguments.EventType == Events.PathToLocationReady)
            // {
            //     if (eventArguments.EventData is PathToLocationReadyEventPayload payload)
            //     {
            //         // check if event for us
            //         if (payload.pathfindingAgent != pathfindingAgent)
            //         {
            //             return base.HandleEvent(stateMachine, eventArguments);
            //         }
            //
            //         pathfindingAgent.Data.CurrentPath = payload.path;
            //         stateMachine.ChangeState(followPathState);
            //         return true;
            //     }
            // }
            // else if (eventArguments.EventType == Events.NoPathToLocationAvailable)
            // {
            //     if (eventArguments.EventData is NoPathToLocationAvailableEventPayload payload)
            //     {
            //         // check if event for us
            //         if (payload.pathfindingAgent != pathfindingAgent)
            //         {
            //             return base.HandleEvent(stateMachine, eventArguments);
            //         }
            //
            //         pathfindingAgent.Data.CurrentPath = null;
            //         pathfindingAgent.Data.pathRequested = false; // try again
            //         return true;
            //     }
            // }

            return base.HandleEvent(stateMachine, eventArguments);
        }
    }
}