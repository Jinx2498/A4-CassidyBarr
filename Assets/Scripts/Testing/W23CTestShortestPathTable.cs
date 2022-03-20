using System.Collections.Generic;
using GameBrains.Actuators.Motion.Navigation.SearchAlgorithms;
using GameBrains.Actuators.Motion.Navigation.SearchGraph;
using GameBrains.Extensions.MonoBehaviours;
using UnityEngine;

namespace Testing
{
    public sealed class W23CTestShortestPathTable : ExtendedMonoBehaviour
    {
        #region Members and Properties
        
        [SerializeField] bool testPathCosts;
        [SerializeField] Node sourceBestPathTable;
        [SerializeField] Node destinationBestPathTable;

        Graph graph;
        
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
            if (graph == null) { return; }

            if (testPathCosts)
            {
                testPathCosts = false;

                BestPathTable.Create(graph);

                Log.Debug(
                    "Best path table Path Exists: " +
                    BestPathTable.PathExists(sourceBestPathTable, destinationBestPathTable));

                Log.Debug(
                    "Best path table Cost: " +
                    BestPathTable.Cost(sourceBestPathTable, destinationBestPathTable));

                Log.Debug(
                    "Best path table Next Node: " +
                    BestPathTable.NextNode(sourceBestPathTable, destinationBestPathTable));

                List<Edge> bestPathTablePath
                    = BestPathTable.Path(sourceBestPathTable, destinationBestPathTable);
                for (var i = 0; i < bestPathTablePath.Count; i++)
                {
                    Edge edge = bestPathTablePath[i];
                    Log.Debug($"Best path table path edge {i}: {edge}");
                }
            }
        }
        
        #endregion Update
    }
}