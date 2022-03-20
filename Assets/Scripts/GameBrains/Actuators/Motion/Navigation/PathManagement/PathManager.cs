using System.Collections.Generic;
using GameBrains.Actuators.Motion.Navigation.SearchAlgorithms;
using GameBrains.Actuators.Motion.Navigation.SearchAlgorithms.CycleLimitedSearch;
using GameBrains.Actuators.Motion.Navigation.SearchGraph;
using GameBrains.Extensions.MonoBehaviours;

namespace GameBrains.Actuators.Motion.Navigation.PathManagement
{
	public sealed class PathManager : ExtendedMonoBehaviour
	{
		#region Members and Properties
		
		public static GameManagement.Parameters Parameters => GameManagement.Parameters.Instance;

		public Graph Graph { get; private set; }

		public List<PathPlanner> SearchRequests
		{
			get => searchRequests;
			private set => searchRequests = value;
		}
		public List<PathPlanner> searchRequests;

		public int SearchesPerCycle => Parameters.MaximumSearchCyclesPerUpdateStep;
		
		public static PathManager Instance
		{
			get
			{
				if (instance == null) { instance = FindObjectOfType<PathManager>(); }
			
				return instance;
			}
		}
		static PathManager instance;

		#endregion Members and Properties

		#region Start

		public override void Start()
		{
			base.Start();
			Graph = FindObjectOfType<Graph>();
			SearchRequests = new List<PathPlanner>();
			Graph.IsLocked = true;
			BestPathTable.Create(Graph);
		}

		#endregion Start

		#region Update

		public override void Update()
		{
			base.Update();
			
			int cyclesRemaining = SearchesPerCycle;
			int currentSearchRequestIndex = 0;

			while (cyclesRemaining > 0 && SearchRequests.Count > 0)
			{
				SearchResults searchResult = SearchRequests[currentSearchRequestIndex].CycleOnce();

				if (searchResult != SearchResults.Running)
				{
					SearchRequests.RemoveAt(currentSearchRequestIndex);
				}
				else
				{
					currentSearchRequestIndex++;
				}

				if (currentSearchRequestIndex >= SearchRequests.Count)
				{
					currentSearchRequestIndex = 0;
				}

				cyclesRemaining--;
			}
		}

		#endregion Update

		#region Path Planner

		public void AddPathPlanner(PathPlanner pathPlanner)
		{
			// make sure the agent does not already have a current search
			if (pathPlanner != null && !SearchRequests.Contains(pathPlanner))
			{
				SearchRequests.Add(pathPlanner);
			}
		}

		public void RemovePathPlanner(PathPlanner pathPlanner)
		{
			if (pathPlanner != null && SearchRequests != null)
			{
				SearchRequests.Remove(pathPlanner);
			}
		}

		#endregion Path Planner
	}
}