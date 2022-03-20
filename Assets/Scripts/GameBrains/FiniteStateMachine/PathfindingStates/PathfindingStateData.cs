using GameBrains.Entities;
using GameBrains.Entities.EntityData;

namespace GameBrains.FiniteStateMachine.PathfindingStates
{
    public class PathfindingStateData : StateData
    {
        // If request in progress, probably should wait before requesting another.
        public bool pathRequested;
        public PathfindingAgent pathfindingAgent;
        public PathfindingData pathfindingData;
    }
}