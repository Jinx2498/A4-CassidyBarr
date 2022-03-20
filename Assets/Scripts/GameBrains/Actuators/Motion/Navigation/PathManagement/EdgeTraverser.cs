using GameBrains.Actuators.Motion.Steering;
using GameBrains.Entities;
using GameBrains.Entities.EntityData;
using GameBrains.EventSystem;
using GameBrains.Extensions.MonoBehaviours;
using GameBrains.Motion.Steering.VelocityBased;
using UnityEngine;

namespace GameBrains.Actuators.Motion.Navigation.PathManagement
{
	public sealed class EdgeTraverser : ExtendedMonoBehaviour
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

		PathFollower PathFollower
		{
			get
			{
				if (pathFollower == null)
				{
					pathFollower = GetComponent<PathFollower>();
				}
				return pathFollower;
			}
		}
		PathFollower pathFollower;

		SteeringBehaviour steering;

		#endregion Members and Properties

		#region Enable/Disable

		public override void OnEnable()
		{
			base.OnEnable();

			if (EventManager.Instance != null)
			{
				EventManager.Instance.Subscribe<LinearStopCompletedEventPayload>(
					Events.LinearStopCompleted,
					OnLinearStopCompleted);

				EventManager.Instance.Subscribe<SeekCompletedEventPayload>(
					Events.SeekCompleted,
					OnSeekCompleted);
			}
			else
			{
				Debug.LogWarning("Event manager missing. Unable to subscribe to steering events.");
			}
		}

		public override void OnDisable()
		{
			base.OnDisable();

			if (EventManager.Instance != null)
			{
				EventManager.Instance.Unsubscribe<LinearStopCompletedEventPayload>(
					Events.LinearStopCompleted,
					OnLinearStopCompleted);

				EventManager.Instance.Unsubscribe<SeekCompletedEventPayload>(
					Events.SeekCompleted,
					OnSeekCompleted);
			}
		}

		#endregion Enable/Disable

		#region Traverse Edge
	
		public bool Traverse(PathEdge edgeToFollow, bool brakeOnApproach, bool stopOnArrival)
		{	
			if (PathfindingAgent == null) { return false; }

			if (steering != null)
			{
				PathfindingData.RemoveSteeringBehaviour(steering.ID);
				Destroy(steering);
			}

			steering = brakeOnApproach 
				? Arrive.CreateInstance(PathfindingData)
				: Seek.CreateInstance(PathfindingData);
			steering.NoStop = !stopOnArrival;
			steering.NeverCompletes = false;
			
			steering.TargetLocation = edgeToFollow.toLocation;
			PathfindingData.AddSteeringBehaviour(steering);

			return true;
		}

		#endregion Traverse Edge

		#region Steering Events

		bool OnLinearStopCompleted(Event<LinearStopCompletedEventPayload> eventArguments)
		{
			LinearStopCompletedEventPayload payload = eventArguments.EventData;

			if (steering == null ||
			    payload.steerableAgent != PathfindingAgent ||
			    payload.linearStop.ID != steering.ID) // event not for us
			{
				return false;
			}
			
			if (steering != null)
			{
				PathfindingData.RemoveSteeringBehaviour(steering.ID);
				Destroy(steering);
			}

			return true;
		}

		bool OnSeekCompleted(Event<SeekCompletedEventPayload> eventArguments)
		{
			SeekCompletedEventPayload payload = eventArguments.EventData;
	
			if (steering == null ||
			    payload.steerableAgent != PathfindingAgent ||
			    payload.seek.ID != steering.ID) // event not for us
			{
				return false;
			}

			if (payload.seek.TargetLocation == PathFollower.EdgeToFollow.toLocation)
			{
				if (steering != null && steering.NoStop)
				{
					PathfindingData.RemoveSteeringBehaviour(steering.ID);
					Destroy(steering);
				}
				
				EventManager.Instance.Enqueue(
					Events.TraversalCompleted,
					new TraversalCompletedEventPayload(PathfindingAgent, PathFollower.EdgeToFollow));
			}

			return true;
		}

		#endregion Steering Events
	}
}