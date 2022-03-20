using SingleUpdateSearch = GameBrains.Actuators.Motion.Navigation.SearchAlgorithms.SingleUpdateSearch;
using CycleLimitedSearch = GameBrains.Actuators.Motion.Navigation.SearchAlgorithms.CycleLimitedSearch;
using OneCyclePerUpdateSearch = GameBrains.Actuators.Motion.Navigation.SearchAlgorithms.OneCyclePerUpdateSearch;
using System.Collections.Generic;
using GameBrains.Actuators.Motion.Navigation.SearchAlgorithms;
using GameBrains.Actuators.Motion.Navigation.SearchGraph;
using GameBrains.Extensions.MonoBehaviours;

namespace Testing
{
    public sealed class W22TestSearch : ExtendedMonoBehaviour
    {
        #region Members and Properties

        #region Least Path Costs

        public bool testPathCosts;
        public Node sourceBestPathTable;
        public Node destinationBestPathTable;

        #endregion Least Path Costs

        #region Single Update Dijkstras Search

        public bool testSingleUpdateDijkstrasSearch;
        public Node sourceSingleUpdateDijkstrasSearch;
        public Node destinationSingleUpdateDijkstrasSearch;
        SingleUpdateSearch.DijkstrasSearch currentSingleUpdateDijkstrasSearch;
        int cyclesUsedSingleUpdateDijkstrasSearch;
        int cyclesPerUpdateSingleUpdateDijkstrasSearch;

        #endregion Single Update Dijkstras Search

        #region Single Update AStar Search

        public bool testSingleUpdateAStarSearch;
        public Node sourceSingleUpdateAStarSearch;
        public Node destinationSingleUpdateAStarSearch;
        SingleUpdateSearch.AStarSearch currentSingleUpdateAStarSearch;
        int cyclesUsedSingleUpdateAStarSearch;
        int cyclesPerUpdateSingleUpdateAStarSearch;

        #endregion Single Update AStar Search

        #region One Cycle Per Update Dijkstras Search

        public bool testOneCyclePerUpdateDijkstrasSearch;
        public Node sourceOneCyclePerUpdateDijkstrasSearch;
        public Node destinationOneCyclePerUpdateDijkstrasSearch;
        OneCyclePerUpdateSearch.DijkstrasSearch currentOneCyclePerUpdateDijkstrasSearch;
        int cyclesUsedOneCyclePerUpdateDijkstrasSearch;
        int cyclesPerUpdateOneCyclePerUpdateDijkstrasSearch;

        #endregion One Cycle Per Update Dijkstras Search

        #region One Cycle Per Update AStar Search

        public bool testOneCyclePerUpdateAStarSearch;
        public Node sourceOneCyclePerUpdateAStarSearch;
        public Node destinationOneCyclePerUpdateAStarSearch;
        OneCyclePerUpdateSearch.AStarSearch currentOneCyclePerUpdateAStarSearch;
        int cyclesUsedOneCyclePerUpdateAStarSearch;
        int cyclesPerUpdateOneCyclePerUpdateAStarSearch;

        #endregion One Cycle Per Update AStar Search

        #region Cycle Limited Dijkstras Search

        public bool testCycleLimitedDijkstrasSearch;
        public Node sourceCycleLimitedDijkstrasSearch;
        public Node destinationCycleLimitedDijkstrasSearch;
        CycleLimitedSearch.DijkstrasSearch currentCycleLimitedDijkstrasSearch;
        int updatesCycleLimitedDijkstrasSearch;
        int cyclesUsedCycleLimitedDijkstrasSearch;
        public int cyclesPerUpdateCycleLimitedDijkstrasSearch = int.MaxValue;

        #endregion Cycle Limited Dijkstras Search

        #region Cycle Limited AStar Search

        public bool testCycleLimitedAStarSearch;
        public Node sourceCycleLimitedAStarSearch;
        public Node destinationCycleLimitedAStarSearch;
        CycleLimitedSearch.AStarSearch currentCycleLimitedAStarSearch;
        int updatesCycleLimitedAStarSearch;
        int cyclesUsedCycleLimitedAStarSearch;
        public int cyclesPerUpdateCycleLimitedAStarSearch = int.MaxValue;

        #endregion Cycle Limited AStar Search

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
            base.Update();
            
            if (graph == null) { return; }

            #region Least Path Costs

            if (testPathCosts)
            {
                testPathCosts = false;
                
                #region Best Path Table (replaces Least Cost Path)
                
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
                
                #endregion Best Path Table (replaces Least Cost Path)

                #region Least Cost Path Table (obsolete: replaced by Best Cost Table)

                // SingleUpdateSearch.LeastCostPathTable.Create(graph);
                //
                // Log.Debug(
                //     "Least cost path table Cost: " +
                //     SingleUpdateSearch.LeastCostPathTable.Cost(
                //         sourceBestPathTable,
                //         destinationBestPathTable));
                //
                // Log.Debug(
                //     "Least cost path table Path Exists: " +
                //     SingleUpdateSearch.LeastCostPathTable.PathExists(
                //         sourceBestPathTable,
                //         destinationBestPathTable));
                //
                // Log.Debug(
                //     "Least cost path table Next Node: " +
                //     SingleUpdateSearch.LeastCostPathTable.NextNode(
                //         sourceBestPathTable,
                //         destinationBestPathTable));
                //
                // List<Edge> leastCostTablePath
                //     = SingleUpdateSearch.LeastCostPathTable.Path(
                //         sourceBestPathTable,
                //         destinationBestPathTable);
                // for (var i = 0; i < leastCostTablePath.Count; i++)
                // {
                //     Edge edge = leastCostTablePath[i];
                //     Log.Debug("Least cost path table path edge " + i + ": " + edge);
                // }

                #endregion Least Cost Path Table (obsolete: replaced by Best Cost Table)
            }

            #endregion Least Path Costs

            #region Single Update Dijkstras Search

            if (testSingleUpdateDijkstrasSearch)
            {
                testSingleUpdateDijkstrasSearch = false;

                if (currentSingleUpdateDijkstrasSearch != null)
                {
                    Log.Debug(
                        "Dijkstras SingleUpdate Search in Progress. Try again later.");
                    return;
                }

                Log.Debug(
                    "Dijkstras SingleUpdate destination: "
                    + destinationSingleUpdateDijkstrasSearch.name);
                currentSingleUpdateDijkstrasSearch
                    = new SingleUpdateSearch.DijkstrasSearch(
                        sourceSingleUpdateDijkstrasSearch,
                        node => node == destinationSingleUpdateDijkstrasSearch);
                cyclesPerUpdateSingleUpdateDijkstrasSearch = 0;
            }

            if (currentSingleUpdateDijkstrasSearch != null)
            {
                SingleUpdateSearch.SearchResults searchResult
                    = currentSingleUpdateDijkstrasSearch.DoSearch(out int cyclesUsed);
                cyclesPerUpdateSingleUpdateDijkstrasSearch++;
                cyclesUsedSingleUpdateDijkstrasSearch += cyclesUsed;

                if (searchResult == SingleUpdateSearch.SearchResults.Failure)
                {
                    Log.Debug(
                        "Dijkstras SingleUpdate Failed: "
                        + " Updates: "
                        + cyclesPerUpdateSingleUpdateDijkstrasSearch
                        + " Cycles: "
                        + cyclesUsedSingleUpdateDijkstrasSearch);
                    currentSingleUpdateDijkstrasSearch = null;
                }
                else if (searchResult == SingleUpdateSearch.SearchResults.Success)
                {
                    Log.Debug(
                        "Dijkstras SingleUpdate Succeeded: "
                        + " Updates: "
                        + cyclesPerUpdateSingleUpdateDijkstrasSearch
                        + " Cycles: "
                        + cyclesUsedSingleUpdateDijkstrasSearch);
                    for (var i = 0; i < currentSingleUpdateDijkstrasSearch.Solution.Count; i++)
                    {
                        Edge edge = currentSingleUpdateDijkstrasSearch.Solution[i];
                        Log.Debug("Dijkstras SingleUpdate path edge " + i + ": " + edge);
                    }

                    currentSingleUpdateDijkstrasSearch = null;
                }
            }

            #endregion Single Update Dijkstras Search

            #region Single Update AStar Search

            if (testSingleUpdateAStarSearch)
            {
                testSingleUpdateAStarSearch = false;

                if (currentSingleUpdateAStarSearch != null)
                {
                    Log.Debug("AStar SingleUpdate Search in Progress. Try again later.");
                    return;
                }

                Log.Debug(
                    "AStar SingleUpdate destination: "
                    + destinationSingleUpdateAStarSearch.name);
                currentSingleUpdateAStarSearch =
                    new SingleUpdateSearch.AStarSearch(
                        sourceSingleUpdateAStarSearch,
                        destinationSingleUpdateAStarSearch);
                cyclesPerUpdateSingleUpdateAStarSearch = 0;
            }

            if (currentSingleUpdateAStarSearch != null)
            {
                SingleUpdateSearch.SearchResults searchResult
                    = currentSingleUpdateAStarSearch.DoSearch(out int cyclesUsed);
                cyclesPerUpdateSingleUpdateAStarSearch++;
                cyclesUsedSingleUpdateAStarSearch += cyclesUsed;

                if (searchResult == SingleUpdateSearch.SearchResults.Failure)
                {
                    Log.Debug(
                        "AStar SingleUpdate Failed: "
                        + " Updates: "
                        + cyclesPerUpdateSingleUpdateAStarSearch
                        + " Cycles: "
                        + cyclesUsedSingleUpdateAStarSearch);
                    currentSingleUpdateAStarSearch = null;
                }
                else if (searchResult == SingleUpdateSearch.SearchResults.Success)
                {
                    Log.Debug(
                        "AStar SingleUpdate Succeeded: "
                        + " Updates: "
                        + cyclesPerUpdateSingleUpdateAStarSearch
                        + " Cycles: "
                        + cyclesUsedSingleUpdateAStarSearch);
                    for (var i = 0; i < currentSingleUpdateAStarSearch.Solution.Count; i++)
                    {
                        Edge edge = currentSingleUpdateAStarSearch.Solution[i];
                        Log.Debug("AStar SingleUpdate path edge " + i + ": " + edge);
                    }

                    currentSingleUpdateAStarSearch = null;
                }
            }

            #endregion Single Update AStar Search

            #region One Cycle Per Update Dijkstras Search

            if (testOneCyclePerUpdateDijkstrasSearch)
            {
                testOneCyclePerUpdateDijkstrasSearch = false;

                if (currentOneCyclePerUpdateDijkstrasSearch != null)
                {
                    Log.Debug(
                        "Dijkstras OneCyclePerUpdate Search in Progress. Try again later.");
                    return;
                }

                Log.Debug(
                    "Dijkstras OneCyclePerUpdate destination: "
                    + destinationOneCyclePerUpdateDijkstrasSearch.name);
                currentOneCyclePerUpdateDijkstrasSearch
                    = new OneCyclePerUpdateSearch.DijkstrasSearch(
                        sourceOneCyclePerUpdateDijkstrasSearch,
                        node => node == destinationOneCyclePerUpdateDijkstrasSearch);
                cyclesPerUpdateOneCyclePerUpdateDijkstrasSearch = 0;
                cyclesUsedOneCyclePerUpdateDijkstrasSearch = 0;
            }

            if (currentOneCyclePerUpdateDijkstrasSearch != null)
            {
                OneCyclePerUpdateSearch.SearchResults searchResult
                    = currentOneCyclePerUpdateDijkstrasSearch.DoSearch();
                cyclesPerUpdateOneCyclePerUpdateDijkstrasSearch++;
                cyclesUsedOneCyclePerUpdateDijkstrasSearch++;

                if (searchResult == OneCyclePerUpdateSearch.SearchResults.Failure)
                {
                    Log.Debug(
                        "Dijkstras OneCyclePerUpdate Failed: "
                        + " Updates: "
                        + cyclesPerUpdateOneCyclePerUpdateDijkstrasSearch
                        + " Cycles: "
                        + cyclesUsedOneCyclePerUpdateDijkstrasSearch);
                    currentOneCyclePerUpdateDijkstrasSearch = null;
                }
                else if (searchResult == OneCyclePerUpdateSearch.SearchResults.Success)
                {
                    Log.Debug(
                        "Dijkstras OneCyclePerUpdate Succeeded: "
                        + " Updates: "
                        + cyclesPerUpdateOneCyclePerUpdateDijkstrasSearch
                        + " Cycles: "
                        + cyclesUsedOneCyclePerUpdateDijkstrasSearch);
                    for (var i = 0; i < currentOneCyclePerUpdateDijkstrasSearch.Solution.Count; i++)
                    {
                        Edge edge = currentOneCyclePerUpdateDijkstrasSearch.Solution[i];
                        Log.Debug(
                            "Dijkstras OneCyclePerUpdate path edge " + i + ": " + edge);
                    }

                    currentOneCyclePerUpdateDijkstrasSearch = null;
                }
                else
                {
                    Log.Debug(
                        "Dijkstras OneCyclePerUpdate Running: "
                        + " Updates: "
                        + cyclesPerUpdateOneCyclePerUpdateDijkstrasSearch
                        + " Cycles: "
                        + cyclesUsedOneCyclePerUpdateDijkstrasSearch);
                }
            }

            #endregion One Cycle Per Update Dijkstras Search

            #region One Cycle Per Update AStar Search

            if (testOneCyclePerUpdateAStarSearch)
            {
                testOneCyclePerUpdateAStarSearch = false;

                if (currentOneCyclePerUpdateAStarSearch != null)
                {
                    Log.Debug("AStar OneCyclePerUpdate Search in Progress. Try again later.");
                    return;
                }

                Log.Debug(
                    "AStar OneCyclePerUpdate destination: "
                    + destinationOneCyclePerUpdateAStarSearch.name);
                currentOneCyclePerUpdateAStarSearch
                    = new OneCyclePerUpdateSearch.AStarSearch(
                        sourceOneCyclePerUpdateAStarSearch,
                        destinationOneCyclePerUpdateAStarSearch);
                cyclesPerUpdateOneCyclePerUpdateAStarSearch = 0;
                cyclesUsedOneCyclePerUpdateAStarSearch = 0;
            }

            if (currentOneCyclePerUpdateAStarSearch != null)
            {
                OneCyclePerUpdateSearch.SearchResults searchResult
                    = currentOneCyclePerUpdateAStarSearch.DoSearch();
                cyclesPerUpdateOneCyclePerUpdateAStarSearch++;
                cyclesUsedOneCyclePerUpdateAStarSearch++;

                if (searchResult == OneCyclePerUpdateSearch.SearchResults.Failure)
                {
                    Log.Debug(
                        "AStar OneCyclePerUpdate Failed: "
                        + " Updates: "
                        + cyclesPerUpdateOneCyclePerUpdateAStarSearch
                        + " Cycles: "
                        + cyclesUsedOneCyclePerUpdateAStarSearch);
                    currentOneCyclePerUpdateAStarSearch = null;
                }
                else if (searchResult == OneCyclePerUpdateSearch.SearchResults.Success)
                {
                    Log.Debug(
                        "AStar OneCyclePerUpdate Succeeded: "
                        + " Updates: "
                        + cyclesPerUpdateOneCyclePerUpdateAStarSearch
                        + " Cycles: "
                        + cyclesUsedOneCyclePerUpdateAStarSearch);
                    for (var i = 0; i < currentOneCyclePerUpdateAStarSearch.Solution.Count; i++)
                    {
                        Edge edge = currentOneCyclePerUpdateAStarSearch.Solution[i];
                        Log.Debug("AStar OneCyclePerUpdate path edge " + i + ": " + edge);
                    }

                    currentOneCyclePerUpdateAStarSearch = null;
                }
                else
                {
                    Log.Debug(
                        "AStar OneCyclePerUpdate Running: "
                        + " Updates: "
                        + cyclesPerUpdateOneCyclePerUpdateAStarSearch
                        + " Cycles: "
                        + cyclesUsedOneCyclePerUpdateAStarSearch);
                }
            }

            #endregion One Cycle Per Update AStar Search

            #region Cycle Limited Dijkstras Search

            if (testCycleLimitedDijkstrasSearch)
            {
                testCycleLimitedDijkstrasSearch = false;

                if (currentCycleLimitedDijkstrasSearch != null)
                {
                    Log.Debug("CycleLimitedDijkstrasSearch in Progress. Try again later.");
                    return;
                }

                Log.Debug(
                    "CycleLimitedDijkstrasSearch destination: "
                    + destinationCycleLimitedDijkstrasSearch.name);
                currentCycleLimitedDijkstrasSearch =
                    new CycleLimitedSearch.DijkstrasSearch(
                        sourceCycleLimitedDijkstrasSearch,
                        destinationCycleLimitedDijkstrasSearch);
                updatesCycleLimitedDijkstrasSearch = 0;
                cyclesUsedCycleLimitedDijkstrasSearch = 0;
            }

            if (currentCycleLimitedDijkstrasSearch != null)
            {
                CycleLimitedSearch.SearchResults searchResult
                    = currentCycleLimitedDijkstrasSearch.DoSearch(
                        out int cyclesUsed,
                        cyclesPerUpdateCycleLimitedDijkstrasSearch);
                updatesCycleLimitedDijkstrasSearch++;
                cyclesUsedCycleLimitedDijkstrasSearch += cyclesUsed;

                if (searchResult == CycleLimitedSearch.SearchResults.Failure)
                {
                    Log.Debug(
                        "CycleLimitedDijkstrasSearch Failed: "
                        + " Updates: "
                        + updatesCycleLimitedDijkstrasSearch
                        + " Cycles: "
                        + cyclesUsedCycleLimitedDijkstrasSearch);
                    currentCycleLimitedDijkstrasSearch = null;
                }
                else if (searchResult == CycleLimitedSearch.SearchResults.Success)
                {
                    Log.Debug(
                        "CycleLimitedDijkstrasSearch Succeeded: "
                        + " Updates: "
                        + updatesCycleLimitedDijkstrasSearch
                        + " Cycles: "
                        + cyclesUsedCycleLimitedDijkstrasSearch);
                    for (var i = 0; i < currentCycleLimitedDijkstrasSearch.Solution.Count; i++)
                    {
                        Edge edge = currentCycleLimitedDijkstrasSearch.Solution[i];
                        Log.Debug(
                            "CycleLimitedDijkstrasSearch path edge "
                            + i
                            + ": "
                            + edge);
                    }

                    currentCycleLimitedDijkstrasSearch = null;
                }
                else if (searchResult == CycleLimitedSearch.SearchResults.Running)
                {
                    Log.Debug(
                        "CycleLimitedDijkstrasSearch Running: "
                        + " Updates: "
                        + updatesCycleLimitedDijkstrasSearch
                        + " Cycles: "
                        + cyclesUsedCycleLimitedDijkstrasSearch);
                }
            }

            #endregion Cycle Limited Dijkstras Search

            #region Cycle Limited AStar Search

            if (testCycleLimitedAStarSearch)
            {
                testCycleLimitedAStarSearch = false;

                if (currentCycleLimitedAStarSearch != null)
                {
                    Log.Debug("CycleLimitedAStarSearch in Progress. Try again later.");
                    return;
                }

                Log.Debug(
                    "CycleLimitedAStarSearch destination: "
                    + destinationCycleLimitedAStarSearch.name);
                currentCycleLimitedAStarSearch =
                    new CycleLimitedSearch.AStarSearch(
                        sourceCycleLimitedAStarSearch,
                        destinationCycleLimitedAStarSearch);
                updatesCycleLimitedAStarSearch = 0;
                cyclesUsedCycleLimitedAStarSearch = 0;
            }

            if (currentCycleLimitedAStarSearch != null)
            {
                CycleLimitedSearch.SearchResults searchResult
                    = currentCycleLimitedAStarSearch.DoSearch(
                        out int cyclesUsed,
                        cyclesPerUpdateCycleLimitedAStarSearch);
                updatesCycleLimitedAStarSearch++;
                cyclesUsedCycleLimitedAStarSearch += cyclesUsed;

                if (searchResult == CycleLimitedSearch.SearchResults.Failure)
                {
                    Log.Debug(
                        "CycleLimitedAStarSearch Failed: "
                        + " Updates: "
                        + updatesCycleLimitedAStarSearch
                        + " Cycles: "
                        + cyclesUsedCycleLimitedAStarSearch);
                    currentCycleLimitedAStarSearch = null;
                }
                else if (searchResult == CycleLimitedSearch.SearchResults.Success)
                {
                    Log.Debug(
                        "CycleLimitedAStarSearch Succeeded: "
                        + " Updates: "
                        + updatesCycleLimitedAStarSearch
                        + " Cycles: "
                        + cyclesUsedCycleLimitedAStarSearch);
                    for (var i = 0; i < currentCycleLimitedAStarSearch.Solution.Count; i++)
                    {
                        Edge edge = currentCycleLimitedAStarSearch.Solution[i];
                        Log.Debug("CycleLimitedAStarSearch path edge " + i + ": " + edge);
                    }

                    currentCycleLimitedAStarSearch = null;
                }
                else if (searchResult == CycleLimitedSearch.SearchResults.Running)
                {
                    Log.Debug(
                        "CycleLimitedAStarSearch Running: "
                        + " Updates: "
                        + updatesCycleLimitedAStarSearch
                        + " Cycles: "
                        + cyclesUsedCycleLimitedAStarSearch);
                }
            }

            #endregion Cycle Limited AStar Search
        }

        #endregion Update
    }
}