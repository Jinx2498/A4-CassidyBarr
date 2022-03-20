using GameBrains.Entities;
using GameBrains.Entities.EntityData;
using GameBrains.EventSystem;
using GameBrains.Extensions.MonoBehaviours;
using UnityEngine;

namespace GameBrains.Actuators.Motion.Navigation.PathManagement
{
	public sealed class PathFollower : ExtendedMonoBehaviour
	{
		#region Members and Properties

		PathfindingAgent PathfindingAgent
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
		
		PathfindingData PathfindingData
		{
			get
			{
				if (pathfindingData == null && PathfindingAgent != null)
				{
					pathfindingData = PathfindingAgent.Data;
				}
				return pathfindingData;
			}
		}
		PathfindingData pathfindingData;
		
		EdgeTraverser EdgeTraverser
		{
			get
			{
				if (edgeTraverser == null)
				{
					edgeTraverser = GetComponent<EdgeTraverser>();
				}
				return edgeTraverser;
			}
		}
		EdgeTraverser edgeTraverser;

		public Path PathToFollow
		{
			get => pathToFollow;
			private set => pathToFollow = value;
		}
		Path pathToFollow;

		public PathEdge EdgeToFollow
		{
			get => edgeToFollow;
			private set => edgeToFollow = value;
		}
		PathEdge edgeToFollow;

		bool IsFollowing { get; set; }
		
		bool BrakeOnFinalApproach
		{
			get => brakeOnFinalApproach;
			set => brakeOnFinalApproach = value;
		}
		[SerializeField] bool brakeOnFinalApproach;
		
		bool StopOnFinalArrival
		{
			get => stopOnFinalArrival;
			set => stopOnFinalArrival = value;
		}
		[SerializeField] bool stopOnFinalArrival;
		
		bool BrakeOnEachApproach
		{
			get => brakeOnEachApproach;
			set => brakeOnEachApproach = value;
		}
		[SerializeField] bool brakeOnEachApproach;
		
		bool StopOnEachArrival
		{
			get => stopOnEachArrival;
			set => stopOnEachArrival = value;
		}
		[SerializeField] bool stopOnEachArrival;

		#endregion Members and Property

		#region Enable/Disable

		public override void OnEnable()
		{
			base.OnEnable();

			if (EventManager.Instance != null)
			{
				EventManager.Instance.Subscribe<PathToLocationReadyEventPayload>(
					Events.PathToLocationReady,
					OnPathToLocationReady);
				
				EventManager.Instance.Subscribe<PathToEntityWithTypesReadyEventPayload>(
					Events.PathToEntityWithTypesReady,
					OnPathToEntityWithTypeReady);
				
				EventManager.Instance.Subscribe<TraversalCompletedEventPayload>(
					Events.TraversalCompleted,
					OnTraversalCompleted);

				EventManager.Instance.Subscribe<TraversalFailedEventPayload>(
					Events.TraversalFailed,
					OnTraversalFailed);
				
				EventManager.Instance.Subscribe<FollowCompletedEventPayload>(
					Events.FollowCompleted,
					OnFollowCompleted);

				EventManager.Instance.Subscribe<FollowFailedEventPayload>(
					Events.FollowFailed,
					OnFollowFailed);
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
				EventManager.Instance.Unsubscribe<PathToLocationReadyEventPayload>(
					Events.PathToLocationReady,
					OnPathToLocationReady);
				
				EventManager.Instance.Unsubscribe<PathToEntityWithTypesReadyEventPayload>(
					Events.PathToEntityWithTypesReady,
					OnPathToEntityWithTypeReady);
				
				EventManager.Instance.Unsubscribe<TraversalCompletedEventPayload>(
					Events.TraversalCompleted,
					OnTraversalCompleted);

				EventManager.Instance.Unsubscribe<TraversalFailedEventPayload>(
					Events.TraversalFailed,
					OnTraversalFailed);
				
				EventManager.Instance.Unsubscribe<FollowCompletedEventPayload>(
					Events.FollowCompleted,
					OnFollowCompleted);

				EventManager.Instance.Unsubscribe<FollowFailedEventPayload>(
					Events.FollowFailed,
					OnFollowFailed);
			}
		}

		#endregion Enable/Disable
		
		#region Update

		public override void Update()
		{
			base.Update();

			if (PathfindingData == null) { return; }
			PathToFollow?.Show(PathfindingData.ShowPath);
			EdgeToFollow?.Show(PathfindingData.ShowPath);
		}

		#endregion Update

		#region Follow Path

		bool Follow(Path nextPathToFollow)
		{
			if (PathfindingAgent == null) { return false; }
			if (EdgeTraverser == null) { return false; }
			
			StopIfFollowingPath();
			PathToFollow = nextPathToFollow;

			if (PathToFollow == null) { return false; }
			
			PathToFollow.Show(PathfindingData.ShowPath);
			IsFollowing = true;
			TraverseNextEdge();
			return true;
		}

		void StopIfFollowingPath()
		{
			if (!IsFollowing) { return; }
			
			IsFollowing = false;
			Path.CancelPath(ref pathToFollow, ref edgeToFollow);
			// TODO: Perhaps this should be a FollowCancelled event
			EventManager.Instance.Enqueue(
				Events.FollowCompleted,
				new FollowCompletedEventPayload(PathfindingAgent, PathToFollow));
		}

		void TraverseNextEdge()
		{
			PathEdge.CancelEdge(ref edgeToFollow);
			EdgeToFollow = PathToFollow.Dequeue();

			if (PathToFollow.IsEmpty) // last edge
			{
				EdgeTraverser.Traverse(
					EdgeToFollow,
					BrakeOnFinalApproach,
					StopOnFinalArrival);
			}
			else
			{
				EdgeTraverser.Traverse(
					EdgeToFollow,
					BrakeOnEachApproach,
					StopOnEachArrival);
			}
		}

		#endregion Follow Path

		#region Pathfinding Events
		
		bool OnPathToLocationReady(Event<PathToLocationReadyEventPayload> eventArguments)
		{	
			PathToLocationReadyEventPayload payload = eventArguments.EventData;
			
			if (payload.pathfindingAgent != PathfindingAgent) // event not for us
			{
				return false;
			}

			if (IsFollowing)
			{
				IsFollowing = false;
				Path.CancelPath(ref pathToFollow, ref edgeToFollow);
			}

			if (PathfindingAgent.Data.SmoothPath)
			{
				payload.path = Path.SmoothPath(PathfindingAgent, payload.path);
			}
			
			Follow(payload.path);
			return true;
		}
		
		bool OnPathToEntityWithTypeReady(Event<PathToEntityWithTypesReadyEventPayload> eventArguments)
		{	
			PathToEntityWithTypesReadyEventPayload payload = eventArguments.EventData;
			
			if (payload.pathfindingAgent != PathfindingAgent) // event not for us
			{
				return false;
			}
			
			if (IsFollowing)
			{
				IsFollowing = false;
				Path.CancelPath(ref pathToFollow, ref edgeToFollow);
			}

			if (PathfindingAgent.Data.SmoothPath)
			{
				payload.path = Path.SmoothPath(PathfindingAgent, payload.path);
			}
			
			Follow(payload.path);
			return true;
		}

		bool OnTraversalCompleted(Event<TraversalCompletedEventPayload> eventArguments)
		{
			TraversalCompletedEventPayload payload = eventArguments.EventData;
		
			if (payload.pathfindingAgent != PathfindingAgent) // event not for us
			{
				return false;
			}
			
			PathEdge.CancelEdge(ref edgeToFollow);

			if (PathToFollow.IsEmpty)
			{
				IsFollowing = false;
				EventManager.Instance.Enqueue(
					Events.FollowCompleted,
					new FollowCompletedEventPayload(PathfindingAgent, PathToFollow));
				return true;
			}

			if (PathfindingAgent.Data.SmoothPath)
			{
				PathToFollow = Path.SmoothPath(PathfindingAgent, PathToFollow);
			}
			
			TraverseNextEdge();
			return true;
		}

		bool OnTraversalFailed(Event<TraversalFailedEventPayload> eventArguments)
		{
			TraversalFailedEventPayload payload = eventArguments.EventData;
		
			if (payload.pathfindingAgent != PathfindingAgent) // event not for us
			{
				return false;
			}

			PathEdge.CancelEdge(ref edgeToFollow);
			IsFollowing = false;
			EventManager.Instance.Enqueue(
				Events.FollowFailed,
				new FollowFailedEventPayload(PathfindingAgent, PathToFollow));

			return true;
		}

		bool OnFollowCompleted(Event<FollowCompletedEventPayload> eventArguments)
		{
			FollowCompletedEventPayload payload = eventArguments.EventData;

			if (payload.pathfindingAgent != PathfindingAgent) // event not for us
			{
				return false;
			}
			
			IsFollowing = false;
			Path.CancelPath(ref pathToFollow, ref edgeToFollow);

			return true;
		}
		
		bool OnFollowFailed(Event<FollowFailedEventPayload> eventArguments)
		{
			FollowFailedEventPayload payload = eventArguments.EventData;

			if (payload.pathfindingAgent != PathfindingAgent) // event not for us
			{
				return false;
			}

			IsFollowing = false;
			Path.CancelPath(ref pathToFollow, ref edgeToFollow);
			
			return true;
		}

		#endregion Pathfinding Events
	}
}