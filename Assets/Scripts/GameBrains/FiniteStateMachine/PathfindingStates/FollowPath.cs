using GameBrains.Entities;
using GameBrains.Entities.EntityData;
using GameBrains.EventSystem;
using UnityEngine;

namespace GameBrains.FiniteStateMachine.PathfindingStates
{
    [CreateAssetMenu(fileName = "FollowPath", menuName = "StateMachine/PathfindingStates/FollowPath")]
    public class FollowPath : State
    {
        public override void Enter(StateMachine stateMachine)
         {
             base.Enter(stateMachine);
         }

         public override void Execute(StateMachine stateMachine)
         {
             base.Execute(stateMachine);
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
             
             if (eventArguments.EventType == Events.FollowCompleted)
             {
                 if (eventArguments.EventData is FollowCompletedEventPayload payload)
                 {
                     // check if event for us
                     if (payload.pathfindingAgent != pathfindingAgent)
                     {
                         return base.HandleEvent(stateMachine, eventArguments);
                     }
                     
                     if (VerbosityDebug)
                     {
                         Log.Debug("FollowPath succeeded.");
                     }
                     
                     pathfindingData.CurrentPath = null;
                     
                     var chooseTargetState = StateManager.Lookup(typeof(ChooseTarget));
                     stateMachine.ChangeState(chooseTargetState);
                     
                     return true;
                 }
             }
             else if (eventArguments.EventType == Events.FollowFailed)
             {
                 if (eventArguments.EventData is FollowFailedEventPayload payload)
                 {
                     // check if event for us
                     if (payload.pathfindingAgent != pathfindingAgent)
                     {
                         return base.HandleEvent(stateMachine, eventArguments);
                     }

                     if (VerbosityDebug)
                     {
                         Log.Debug("FollowPath failed.");
                     }
                     
                     pathfindingData.CurrentPath = null;
                     
                     var chooseTargetState = StateManager.Lookup(typeof(ChooseTarget));
                     stateMachine.ChangeState(chooseTargetState);
                     
                     return true;
                 }
             }

             return base.HandleEvent(stateMachine, eventArguments);
         }
     }
}