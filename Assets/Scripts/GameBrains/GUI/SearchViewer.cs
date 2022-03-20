using System.Collections.Generic;
using System.Text;
using GameBrains.Actuators.Motion.Navigation.PathManagement;
using GameBrains.Actuators.Motion.Navigation.SearchAlgorithms.CycleLimitedSearch;
using GameBrains.Entities;
using GameBrains.Entities.Types;
using GameBrains.Extensions.Vectors;
using UnityEngine;

namespace GameBrains.GUI
{
    [AddComponentMenu("Scripts/GameBrains/GUI/Search Viewer")]
    public class SearchViewer : FpsViewer
    {
        struct SearchRecord
        {
            public PathfindingAgent agent;
            public Search search;
            public SearchAlgorithms algorithms;
            public PathPlanner.SearchTypes searchType;
            public VectorXZ destination;
            public EntityTypes entityTypes;

            public SearchRecord(
                PathfindingAgent agent,
                Search search, 
                SearchAlgorithms algorithms,
                PathPlanner.SearchTypes searchType, 
                VectorXZ destination,
                EntityTypes entityTypes)
            {
                this.agent = agent;
                this.search = search;
                this.algorithms = algorithms;
                this.searchType = searchType;
                this.destination = destination;
                this.entityTypes = entityTypes;
            }
        }
        
        PathManager pathManager;
        List<SearchRecord> searchRecords = new List<SearchRecord>();

        public override void Start()
        {
            base.Start(); // Initializes the window id.
            windowTitle = "Search Viewer";
            pathManager = FindObjectOfType<PathManager>();
        }

        // This creates the GUI inside the window.
        // It requires the id of the window it's currently making GUI for.
        protected override void WindowFunction(int windowID)
        {
            // Purposely not calling base.WindowFunction here.
        
            // Draw any Controls inside the window here.
            
            Color savedColor = UnityEngine.GUI.color;
            UnityEngine.GUI.color = defaultContentColor;

            GUILayout.BeginHorizontal();
        
             #region Column One
            
             GUILayout.BeginVertical(GUILayout.MinWidth(minimumColumnWidth));
            
             if (showFps) { GUILayout.Label($"FPS: {fps:f1}"); }
            
             var currentSearchRequests = pathManager.SearchRequests;

             var activeSearch = currentSearchRequests != null && currentSearchRequests.Count != 0;
             if (activeSearch)
             {
                 searchRecords = new List<SearchRecord>();
                 foreach (var searchRequest in currentSearchRequests)
                 {
                     var agent = searchRequest.PathfindingAgent;
                     var search = searchRequest.CurrentSearch;
                     var algorithm = search.SearchAlgorithms;
                     var searchType = searchRequest.CurrentSearchType;
                     var destination = searchRequest.Destination;
                     var entityTypes = searchRequest.EntityTypes;
                     searchRecords.Add(
                         new SearchRecord(agent, search, algorithm, searchType, destination, entityTypes));
                 }
             }

             var searchMessage = activeSearch ? "Active" : "Previous";
             GUILayout.Label($"{searchMessage} Searches: {searchRecords.Count}");

             for (var i = 0; i < searchRecords.Count; i++)
             {
                 var searchRecord = searchRecords[i];
                 
                 var sb = new StringBuilder();
                 sb.Append($"{i}: {searchRecord.agent.ShortName} using {searchRecord.algorithms.ToString()}");
                 sb.Append($"({searchRecord.searchType.ToString()})");
                 // TODO: Add cost and result?
                 sb.Append($"  {searchRecord.search.Source.Location}-->");
                 switch (searchRecord.search.SearchAlgorithms)
                 {
                     case SearchAlgorithms.Dijkstra:
                         sb.AppendLine($"{searchRecord.entityTypes}");
                         break;
                     case SearchAlgorithms.AStar:
                         sb.AppendLine($"{searchRecord.destination}");
                         break;
                 }

                 GUILayout.Label(sb.ToString());
             }
            
             GUILayout.EndVertical();
            
             #endregion Column One
            
             #region Column Two
            
             // GUILayout.BeginVertical(GUILayout.MinWidth(minimumColumnWidth));
             // // Add second column here
             //
             // GUILayout.EndVertical();
            
             #endregion Column Two
        
            GUILayout.EndHorizontal();
            
            UnityEngine.GUI.color = savedColor;
        
            // Make the windows be draggable.
            UnityEngine.GUI.DragWindow();
        }
    }
}