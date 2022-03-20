using System.ComponentModel;
using GameBrains.Actuators.Motion.Navigation.PathManagement;
using GameBrains.Entities;
using GameBrains.Entities.Types;
using GameBrains.Extensions.Vectors;

// ReSharper disable once CheckNamespace
namespace GameBrains.EventSystem // NOTE: Don't change this namespace
{
	#region Events
	
	public static partial class Events
    {
        [Description("PathToLocationRequest")]
        public static readonly EventType PathToLocationRequest = (EventType)Count++;

		[Description("PathToLocationReady")]
        public static readonly EventType PathToLocationReady = (EventType)Count++;

		[Description("NoPathToLocationAvailable")]
        public static readonly EventType NoPathToLocationAvailable = (EventType)Count++;

		[Description("PathToEntityWithTypeRequest")]
        public static readonly EventType PathToEntityWithTypesRequest = (EventType)Count++;

		[Description("PathToEntityWithTypeReady")]
        public static readonly EventType PathToEntityWithTypesReady = (EventType)Count++;

		[Description("NoPathToEntityWithTypeAvailable")]
        public static readonly EventType NoPathToEntityWithTypesAvailable = (EventType)Count++;

		[Description("FollowCompleted")]
        public static readonly EventType FollowCompleted = (EventType)Count++;

		[Description("FollowFailed")]
        public static readonly EventType FollowFailed = (EventType)Count++;

		[Description("TraversalCompleted")]
        public static readonly EventType TraversalCompleted = (EventType)Count++;

		[Description("TraversalFailed")]
        public static readonly EventType TraversalFailed = (EventType)Count++;
    }
	
	#endregion Events

	#region Event Payloads
	
	public struct PathToLocationRequestEventPayload
	{
		public PathfindingAgent pathfindingAgent;
		public VectorXZ destination;
		
		public PathToLocationRequestEventPayload(
			PathfindingAgent pathfindingAgent,
	        VectorXZ destination)
	    {
	        this.pathfindingAgent = pathfindingAgent;
	        this.destination = destination;
	    }
	}
	
	public struct PathToLocationReadyEventPayload
	{
		public PathfindingAgent pathfindingAgent;
		public Path path;
		
		public PathToLocationReadyEventPayload(
			PathfindingAgent pathfindingAgent,
	        Path path)
	    {
	        this.pathfindingAgent = pathfindingAgent;
	        this.path = path;
	    }
	}
	
	public struct NoPathToLocationAvailableEventPayload
	{
		public PathfindingAgent pathfindingAgent;
		
		public NoPathToLocationAvailableEventPayload(PathfindingAgent pathfindingAgent)
	    {
	        this.pathfindingAgent = pathfindingAgent;
	    }
	}
	
	public struct PathToEntityWithTypesRequestEventPayload
	{
		public PathfindingAgent pathfindingAgent;
		public EntityTypes entityTypes;

		public PathToEntityWithTypesRequestEventPayload(
			PathfindingAgent pathfindingAgent,
			EntityTypes entityTypes)
	    {
	        this.pathfindingAgent = pathfindingAgent;
			this.entityTypes = entityTypes;
	    }
	}
	
	public struct PathToEntityWithTypesReadyEventPayload
	{
		public PathfindingAgent pathfindingAgent;
		public Path path;
		public Entity entityWithTypes;
		
		public PathToEntityWithTypesReadyEventPayload(
			PathfindingAgent pathfindingAgent,
	        Path path,
		    Entity entityWithTypes)
	    {
	        this.pathfindingAgent = pathfindingAgent;
	        this.path = path;
			this.entityWithTypes = entityWithTypes;
	    }
	}
	
	public struct NoPathToEntityWithTypesAvailableEventPayload
	{
		public PathfindingAgent pathfindingAgent;
		
		public NoPathToEntityWithTypesAvailableEventPayload(PathfindingAgent pathfindingAgent)
	    {
	        this.pathfindingAgent = pathfindingAgent;
	    }
	}
	
	public struct FollowCompletedEventPayload
	{
		public PathfindingAgent pathfindingAgent;
		public Path path;

		public FollowCompletedEventPayload(
			PathfindingAgent pathfindingAgent,
			Path path)
		{
			this.pathfindingAgent = pathfindingAgent;
			this.path = path;
		}
	}

	public struct FollowFailedEventPayload
	{
		public PathfindingAgent pathfindingAgent;
		public Path path;

		public FollowFailedEventPayload(
			PathfindingAgent pathfindingAgent,
			Path path)
		{
			this.pathfindingAgent = pathfindingAgent;
			this.path = path;
		}
	}
	
	public struct TraversalCompletedEventPayload
	{
		public PathfindingAgent pathfindingAgent;
		public PathEdge pathEdge;

		public TraversalCompletedEventPayload(
			PathfindingAgent pathfindingAgent,
			PathEdge pathEdge)
		{
			this.pathfindingAgent = pathfindingAgent;
			this.pathEdge = pathEdge;
		}
	}

	public struct TraversalFailedEventPayload
	{
		public PathfindingAgent pathfindingAgent;
		public PathEdge pathEdge;

		public TraversalFailedEventPayload(
			PathfindingAgent pathfindingAgent,
			PathEdge pathEdge)
		{
			this.pathfindingAgent = pathfindingAgent;
			this.pathEdge = pathEdge;
		}
	}

	#endregion Event Payloads
}