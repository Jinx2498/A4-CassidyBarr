using GameBrains.Actuators.Motion.Navigation.SearchGraph;
using GameBrains.Entities;
using GameBrains.Entities.Types;
using GameBrains.Extensions.MonoBehaviours;
using GameBrains.Extensions.Vectors;

namespace Testing
{
    public class W24TestPathfindingAgent : ExtendedMonoBehaviour
    {
        #region Members and Properties
        
        public bool respawn;
        public VectorXYZ spawnPoint;
        public bool clearSteeringBehaviours;
        
        public bool testPathToLocation;
        public PathfindingAgent pathfindingAgent;
        public VectorXZ destination;

        public bool testPathToEntityWithType;
        public EntityTypes entityTypes;

        public bool testSeekToLocation;
        public bool testArriveAtLocation;

        public bool testClosestNodeToLocation;
        public bool testCostToClosestEntityWithType;
        public bool testNodeIsCloseToEntityOfType;
        public bool testNextNodeIsCloseToEntityOfType;

        Graph graph;
        int nodeIndex;
        #endregion Members and Properties

        #region Awake
        
        public override void Awake()
        {
            base.Awake();
            
            graph = FindObjectOfType<Graph>();
        }
        
        #endregion Awake

        #region Update

        public override void Update()
        {
            base.Update();
            
            if (respawn)
            {
                respawn = false;
                pathfindingAgent.Spawn(spawnPoint);
            }
            
            if (clearSteeringBehaviours)
            {
                clearSteeringBehaviours = false;
                pathfindingAgent.Data.SteeringBehaviours.Clear();
            }
            
            if (testPathToLocation)
            {
                testPathToLocation = false;

                pathfindingAgent.Data.FindPathTo(destination);
            }

            if (testSeekToLocation)
            {
                testSeekToLocation = false;

                pathfindingAgent.Data.SeekToLocation(destination);
            }

            if (testArriveAtLocation)
            {
                testArriveAtLocation = false;

                pathfindingAgent.Data.ArriveAtLocation(destination);
            }

            if (testPathToEntityWithType)
            {
                testPathToEntityWithType = false;
                
                pathfindingAgent.Data.FindPathTo(entityTypes);
            }

            if (testClosestNodeToLocation)
            {
                testClosestNodeToLocation = false;

                Node node =
                    pathfindingAgent.Data.ClosestNodeToLocation(pathfindingAgent.Data.Location);
                Log.Debug($"Closest visible node to {pathfindingAgent.ShortName} is {node.name}.");
            }

            if (testCostToClosestEntityWithType)
            {
                testCostToClosestEntityWithType = false;

                float cost = pathfindingAgent.Data.CostToClosestEntityWithTypes(entityTypes);
                Log.Debug($"The cost to the closest entity of type {entityTypes} is {cost}.");
            }

            if (testNodeIsCloseToEntityOfType)
            {
                testNodeIsCloseToEntityOfType = false;

                bool found = false;
                
                if (graph != null && graph.NodeCollection != null)
                {
                    var nodes = graph.NodeCollection.Nodes;

                    if (nodes != null)
                    {
                        foreach (var node in nodes)
                        {
                            if (pathfindingAgent.Data.NodeIsCloseToEntityOfTypes(node, entityTypes, out var foundEntity))
                            {
                                found = true;
                                Log.Debug($"Node {node.name} is close to {foundEntity} which is of type {entityTypes}.");
                            }
                        }
                    }
                }

                if (!found)
                {
                    Log.Debug($"No nodes are close to an entity of type {entityTypes}.");
                }
            }

            if (testNextNodeIsCloseToEntityOfType)
            {
                testNextNodeIsCloseToEntityOfType = false;

                if (graph != null && graph.NodeCollection != null)
                {
                    var nodes = graph.NodeCollection.Nodes;

                    if (nodes != null && nodes.Length > 0)
                    {
                        var node = nodes[nodeIndex];

                        if (pathfindingAgent.Data.NodeIsCloseToEntityOfTypes(node, entityTypes, out var foundEntity))
                        {
                            Log.Debug($"Node {node.name} is close to {foundEntity} which is of type {entityTypes}.");
                        }
                        else
                        {
                            Log.Debug($"Node {node.name} is NOT close to {foundEntity} which is of type {entityTypes}.");
                        }

                        nodeIndex = (nodeIndex + 1) % nodes.Length;
                        return;
                    }
                }

                Log.Debug($"There are no nodes.");
            }
        }
        
        #endregion Update
    }
}