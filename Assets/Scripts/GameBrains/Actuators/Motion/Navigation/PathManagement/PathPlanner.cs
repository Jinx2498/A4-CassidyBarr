using System;
using System.Collections.Generic;
using GameBrains.Actuators.Motion.Navigation.SearchAlgorithms.CycleLimitedSearch;
using GameBrains.Actuators.Motion.Navigation.SearchGraph;
using GameBrains.Entities;
using GameBrains.Entities.EntityData;
using GameBrains.Entities.Types;
using GameBrains.EventSystem;
using GameBrains.Extensions.MonoBehaviours;
using GameBrains.Extensions.Vectors;
using GameBrains.Visualization;
using UnityEngine;

namespace GameBrains.Actuators.Motion.Navigation.PathManagement
{
	public sealed class PathPlanner : ExtendedMonoBehaviour
	{
		#region Members and Properties

		public enum SearchTypes { Position, EntityType }

		public PathManager PathManager => PathManager.Instance;

		public PathfindingAgent PathfindingAgent
		{
			get
			{
				if (pathfindingAgent == null)
				{
					pathfindingAgent = GetComponent<PathfindingAgent>();
				}
				return pathfindingAgent;
			}
		}
		PathfindingAgent pathfindingAgent;

		PathfindingData PathfindingData => PathfindingAgent.Data;

		public VectorXZ Source { get; set; }

		public VectorXZ Destination { get; set; }

		public EntityTypes EntityTypes { get; set; }
		
		public Entity EntityWithType { get => entityWithType; set => entityWithType = value; }
		Entity entityWithType;

		public Search CurrentSearch { get; set; }

		public SearchTypes CurrentSearchType { get; set; }

		public bool ShowDirectPathVisualizer
		{
			get => showDirectPathVisualizer;
			set => showDirectPathVisualizer = value;
		}
		[SerializeField] bool showDirectPathVisualizer = true;
		
		public bool ShowDirectPathVisualizerOnlyWhenBlocked
		{
			get => showDirectPathVisualizerOnlyWhenBlocked;
			set => showDirectPathVisualizerOnlyWhenBlocked = value;
		}
		[SerializeField] bool showDirectPathVisualizerOnlyWhenBlocked;
		
		public float ShowDirectPathVisualizerCastRadius
		{
			get => showDirectPathVisualizerCastRadius;
			set => showDirectPathVisualizerCastRadius = value;
		}
		[SerializeField] float showDirectPathVisualizerCastRadius = 1f;
		
		public Color ShowDirectPathVisualizerClearColor
		{
			get => showDirectPathVisualizerClearColor;
			set => showDirectPathVisualizerClearColor = value;
		}
		[SerializeField] Color showDirectPathVisualizerClearColor = Color.cyan;
		
		public Color ShowDirectPathVisualizerBlockedColor
		{
			get => showDirectPathVisualizerBlockedColor;
			set => showDirectPathVisualizerBlockedColor = value;
		}
		[SerializeField] Color showDirectPathVisualizerBlockedColor = Color.magenta;
		
		CapsuleCastVisualizer directPathVisualizer;

		#endregion Members and Properties

		#region Enable/Disable
		
		public override void OnEnable()
		{
			base.OnEnable();

			if (EventManager.Instance != null)
			{
				EventManager.Instance.Subscribe<PathToLocationRequestEventPayload>(
					Events.PathToLocationRequest,
					OnPathToLocationRequest);

				EventManager.Instance.Subscribe<PathToEntityWithTypesRequestEventPayload>(
					Events.PathToEntityWithTypesRequest,
					OnPathToEntityWithTypeRequest);
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
				EventManager.Instance.Unsubscribe<PathToLocationRequestEventPayload>(
					Events.PathToLocationRequest,
					OnPathToLocationRequest);

				EventManager.Instance.Unsubscribe<PathToEntityWithTypesRequestEventPayload>(
					Events.PathToEntityWithTypesRequest,
					OnPathToEntityWithTypeRequest);
			}
		}
		
		#endregion Enable/Disable

		#region Search

		public SearchResults CycleOnce()
		{
			if (CurrentSearch == null)
			{
				throw new System.Exception("PathPlanner.CycleOnce: No search object instantiated.");
			}

			SearchResults searchResult = CurrentSearch.DoOneCycleOfSearch();

			if (searchResult == SearchResults.Failure)
			{
				if (CurrentSearchType is SearchTypes.Position)
				{
					EventManager.Instance.Enqueue(
						Events.NoPathToLocationAvailable,
						new NoPathToLocationAvailableEventPayload(PathfindingAgent));
				}
				else if (CurrentSearchType is SearchTypes.EntityType)
				{
					EventManager.Instance.Enqueue(
						Events.NoPathToEntityWithTypesAvailable,
						new NoPathToEntityWithTypesAvailableEventPayload(PathfindingAgent));
				}
			}
			else if (searchResult == SearchResults.Success)
			{
				if (CurrentSearchType is SearchTypes.Position)
				{
					var path = new Path(
						Source, 
						CurrentSearch.Solution, 
						Destination);
					EventManager.Instance.Enqueue(
						Events.PathToLocationReady,
						new PathToLocationReadyEventPayload(
							PathfindingAgent,
							path));
				}
				else if (CurrentSearchType is SearchTypes.EntityType)
				{
					var path = new Path(
						Source,
						CurrentSearch.Solution,
						EntityWithType.Data.Location);
					EventManager.Instance.Enqueue(
						Events.PathToEntityWithTypesReady,
						new PathToEntityWithTypesReadyEventPayload(
							PathfindingAgent,
							path,
							EntityWithType));
				}
			}

			return searchResult;
		}
		
		void GetReadyForNewSearch()
		{
			PathManager.RemovePathPlanner(this);
			CurrentSearch = null;
			EntityWithType = null;
		}

		#endregion Search
		
		#region Path to Position

		bool OnPathToLocationRequest(Event<PathToLocationRequestEventPayload> eventArguments)
		{
			PathToLocationRequestEventPayload payload = eventArguments.EventData;

			if (payload.pathfindingAgent != PathfindingAgent) // event not for us
			{
				return false;
			}

			if (!RequestPathToLocation(payload.destination))
			{
				EventManager.Instance.Enqueue(
					Events.NoPathToLocationAvailable,
					new NoPathToLocationAvailableEventPayload(PathfindingAgent));
			}

			return true;
		}

		bool RequestPathToLocation(VectorXZ destination)
		{
			GetReadyForNewSearch();

			if (PathfindingAgent == null || PathManager == null || PathManager.Graph == null)
			{
				return false;
			}

			CurrentSearchType = SearchTypes.Position;

			Source = PathfindingData.Location;

			Destination = destination;

			// if the destination is walkable from the agent's position a path does
			// not need to be calculated, the agent can go straight to the position
			// by ARRIVING at the current waypoint (or using Quick Path)
			if (ClearPathToDestination())
			{
				// there will be no search
				
				var path = new Path(
					Source, 
					new List<Edge>(), 
					Destination);
				EventManager.Instance.Enqueue(
					Events.PathToLocationReady,
					new PathToLocationReadyEventPayload(
						PathfindingAgent,
						path));
				return true;
			}

			Node closestNodeToAgent = ClosestNodeTo(Source);

			if (closestNodeToAgent == null) { return false; }

			Node closestNodeToDestination = ClosestNodeTo(Destination);

			if (closestNodeToDestination == null) { return false; }
			
			CurrentSearch = new AStarSearch(closestNodeToAgent, closestNodeToDestination);

			PathManager.AddPathPlanner(this);

			return true;
		}

		bool ClearPathToDestination()
		{
			directPathVisualizer = ShowDirectPathVisualizer
				? ScriptableObject.CreateInstance<CapsuleCastVisualizer>()
				: null;
			if (directPathVisualizer)
			{
				directPathVisualizer.destroyInsteadOfHide = true;
			}

			return PathfindingAgent.Data.CanMoveBetween(
				(VectorXYZ)Source,
				(VectorXYZ)Destination,
				directPathVisualizer,
				ShowDirectPathVisualizer,
				ShowDirectPathVisualizerOnlyWhenBlocked,
				ShowDirectPathVisualizerCastRadius,
				ShowDirectPathVisualizerClearColor,
				ShowDirectPathVisualizerBlockedColor);
		}
		
		Node ClosestNodeTo(VectorXZ location)
		{
			return PathfindingData.ClosestNodeToLocation(location);
		}
		
		#endregion Path to Position
		
		#region Path to Entity With Types

		bool OnPathToEntityWithTypeRequest(Event<PathToEntityWithTypesRequestEventPayload> eventArguments)
		{
			PathToEntityWithTypesRequestEventPayload payload = eventArguments.EventData;

			if (payload.pathfindingAgent != PathfindingAgent) // event not for us
			{
				return false;
			}

			if (!RequestPathToEntityWithType(payload.entityTypes))
			{
				EventManager.Instance.Enqueue(
					Events.NoPathToEntityWithTypesAvailable,
					new NoPathToEntityWithTypesAvailableEventPayload(PathfindingAgent));
			}

			return true;
		}

		bool RequestPathToEntityWithType(EntityTypes entityTypes)
		{
			GetReadyForNewSearch();

			if (PathfindingAgent == null || PathManager == null || PathManager.Graph == null) { return false; }

			CurrentSearchType = SearchTypes.EntityType;

			Source = PathfindingData.Location;

			EntityTypes = entityTypes;

			Node closestNodeToAgent = PathfindingData.ClosestNodeToLocation(Source);

			if (closestNodeToAgent == null) { return false; }

			CurrentSearch =
				new DijkstrasSearch(
					closestNodeToAgent,
					node => PathfindingData.NodeIsCloseToEntityOfTypes(node, EntityTypes, out entityWithType));

			PathManager.AddPathPlanner(this);

			return true;
		}
		
		#endregion Path to Entity With Types
	}
}