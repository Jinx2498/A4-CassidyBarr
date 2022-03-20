using GameBrains.Entities;
using GameBrains.Entities.EntityData;
using GameBrains.EventSystem;
using GameBrains.Extensions.Vectors;
using UnityEngine;

namespace GameBrains.FiniteStateMachine.PathfindingStates
{
    [CreateAssetMenu(fileName = "ChooseTarget", menuName = "StateMachine/PathfindingStates/ChooseTarget")]
    public class ChooseTarget : State
    {
        public override void Enter(StateMachine stateMachine) { base.Enter(stateMachine); }

        public override void Execute(StateMachine stateMachine)
        {
            base.Execute(stateMachine);
            
            // This must set up and initialized in the Start state.
            PathfindingStateData pathfindingStateData = (PathfindingStateData)stateMachine.StateData;
            PathfindingData pathfindingData = pathfindingStateData.pathfindingData;
            
            if (!pathfindingStateData.pathRequested && pathfindingData.CurrentPath == null)
            {
                int targetNumber = Random.Range(0, 5); // [0..4]
                var targetObject = GameObject.Find($"TargetEntity ({targetNumber})");
                if (targetObject != null)
                {
                    if (VerbosityDebug)
                    {
                        Log.Debug($"Chose target {targetObject}.");
                    }
                    
                    var targetLocation = (VectorXZ)targetObject.transform.position;
                    pathfindingData.FindPathTo(targetLocation);
                    pathfindingStateData.pathRequested = true;
                }
                else if (VerbosityDebug)
                {
                    Log.Debug("No target found.");
                }
            }
        }

        public override void Exit(StateMachine stateMachine)
        {
            base.Exit(stateMachine);
        }

        public override bool HandleEvent<TEvent>(
            StateMachine stateMachine,
            Event<TEvent> eventArguments)
        {
            PathfindingStateData pathfindingStateData = (PathfindingStateData)stateMachine.StateData;
            PathfindingAgent pathfindingAgent = pathfindingStateData.pathfindingAgent;
            PathfindingData pathfindingData = pathfindingStateData.pathfindingData;
            
            if (eventArguments.EventType == Events.PathToLocationReady)
            {
                if (eventArguments.EventData is PathToLocationReadyEventPayload payload)
                {
                    // check if event for us
                    if (payload.pathfindingAgent != pathfindingAgent)
                    {
                        return base.HandleEvent(stateMachine, eventArguments);
                    }
            
                    pathfindingData.CurrentPath = payload.path;
                    pathfindingStateData.pathRequested = false;
                    var followPathState = StateManager.Lookup(typeof(FollowPath));
                    stateMachine.ChangeState(followPathState);
                    return true;
                }
            }
            else if (eventArguments.EventType == Events.NoPathToLocationAvailable)
            {
                if (eventArguments.EventData is NoPathToLocationAvailableEventPayload payload)
                {
                    // check if event for us
                    if (payload.pathfindingAgent != pathfindingAgent)
                    {
                        return base.HandleEvent(stateMachine, eventArguments);
                    }
            
                    pathfindingData.CurrentPath = null;
                    pathfindingStateData.pathRequested = false; // try again
                    return true;
                }
            }
            
            return base.HandleEvent(stateMachine, eventArguments);
        }
    }
}