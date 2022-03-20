using System.Collections.Generic;
using GameBrains.Actuators.Motion.Navigation.SearchGraph;
using GameBrains.Entities;
using GameBrains.Extensions.Lists;
using GameBrains.Extensions.Vectors;
using UnityEngine;

namespace GameBrains.Actuators.Motion.Navigation.PathManagement
{
	// A path is a list of edges from the source to destination or an empty list if no path exists.
	public class Path
    {
	    #region Members and properties
		
	    public VectorXZ Source { get; set; }
	    public VectorXZ Destination { get; set; }
	    
	    public List<PathEdge> PathEdges { get; }

	    public bool IsEmpty => PathEdges == null || PathEdges.IsEmpty();

	    #endregion Members and properties
	    
	    #region Constructor
	    
		public Path(VectorXZ source, List<Edge> edges, VectorXZ destination)
		{
			Source = source;
			Destination = destination;
			PathEdges = CreatePathEdges(edges);
		}

		#region Create Path Edges

		List<PathEdge> CreatePathEdges(List<Edge> edges)
		{
			var pathEdges = new List<PathEdge>();
			bool edgesIsEmpty = edges == null || edges.IsEmpty();
			bool edgesStartWithSource = !edgesIsEmpty && Source == edges[0].FromNode.Location;
			
			if (!edgesStartWithSource)
			{
				var edgeSource = Source;
				var edgeDestination = edgesIsEmpty ? Destination : edges[0].FromNode.Location;
				pathEdges.Add(new PathEdge(false, edgeSource, edgeDestination));
			}

			if (!edgesIsEmpty)
			{
				foreach (var edge in edges)
				{
					var edgeSource = edge.FromNode.Location;
					var edgeDestination = edge.ToNode.Location;
					pathEdges.Add(new PathEdge(true, edgeSource, edgeDestination));
				}
			}
			
			bool edgesEndWithDestination = !edgesIsEmpty && Destination == edges[edges.Count-1].ToNode.Location;

			if (!edgesEndWithDestination && !edgesIsEmpty)
			{
				var edgeSource = edges[edges.Count - 1].ToNode.Location;
				var edgeDestination = Destination;
				pathEdges.Add(new PathEdge(false, edgeSource, edgeDestination));
			}

			return pathEdges;
		}
		
		#endregion Create Path Edges
		
		#endregion Constructor

		#region Path Methods

		public void ReplacePathEdge(int index, bool trueEdge, VectorXZ fromLocation, VectorXZ toLocation)
		{
			if (index < PathEdges.Count)
			{
				if (PathEdges[index] != null)
				{
					PathEdges[index].Show(false);
					PathEdges[index].CleanUp();
				}

				PathEdges[index] = new PathEdge(trueEdge, fromLocation, toLocation);
			}
			else
			{
				Debug.LogWarning("ReplacePathEdge index out of range.");
			}
		}

		#region Dequeue/Peek

		public PathEdge Dequeue()
		{
			if (IsEmpty) { return null; }

			PathEdge firstEdge = PathEdges[0];
			PathEdges.RemoveAt(0);
			return firstEdge;
		}
		
		public PathEdge Peek()
		{
			if (IsEmpty) { return null; }
		
			PathEdge firstEdge = PathEdges[0];
			return firstEdge;
		}

		#endregion Dequeue/Peek

		#region To String

		public override string ToString()
		{
			return PathEdges.ToNumberedItemsString();
		}

		#endregion To String

		#region Show

		public void Show(bool show)
		{
			foreach (var pathEdge in PathEdges)
			{
				pathEdge.Show(show);
			}
		}

		#endregion Show

		#region Clean Up

		public void CleanUp()
		{
			for (var index = PathEdges.Count - 1; index >= 0 ; index--)
			{
				var pathEdge = PathEdges[index];
				pathEdge.Show(false);
				pathEdge.CleanUp();
				PathEdges.RemoveAt(index);
			}
		}

		#endregion Clean Up

		#region Smooth Path

		public static void SmoothPath(
			PathfindingAgent pathfindingAgent,
			ref Path currentPath,
			ref PathEdge currentEdge)
		{
			SmoothPath(pathfindingAgent, ref currentPath);

			// TODO: Set current edge. Maybe cancel first?
		}
		
		public static void SmoothPath(
			PathfindingAgent pathfindingAgent,
			ref Path currentPath)
		{
			var pathfindingData = pathfindingAgent.Data;
			var pathSource = pathfindingData.Location;
			var pathEdges = currentPath.PathEdges;
			var lastEdgeIndex = pathEdges.Count - 1;

			for (int index = lastEdgeIndex; index >= 0; index--)
			{
				var toLocation = pathEdges[index].toLocation;
				if (pathfindingData.CanMoveTo((VectorXYZ)toLocation))
				{
					for (int i = 0; i <= index; i++)
					{
						pathEdges[i] = PathEdge.CancelEdge(pathEdges[i]);
					}

					pathEdges.RemoveRange(1, index);
					currentPath.ReplacePathEdge(0, false, pathSource, toLocation);
					pathEdges[0].Show(pathfindingData.ShowPath);
					break;
				}
			}
		}
		
		// Use when you can't pass by ref
		public static (Path currentPath, PathEdge currentEdge) SmoothPath(
			PathfindingAgent pathfindingAgent,
			Path currentPath,
			PathEdge currentEdge)
		{
			SmoothPath(pathfindingAgent, ref currentPath, ref currentEdge);
			return (currentPath, currentEdge);
		}
		
		// Use when you can't pass by ref
		public static Path SmoothPath(
			PathfindingAgent pathfindingAgent,
			Path currentPath)
		{
			SmoothPath(pathfindingAgent, ref currentPath);
			return currentPath;
		}

		#endregion Smooth Path

		#region Cancel Path

		public static void CancelPath(ref Path currentPath, ref PathEdge currentPathEdge)
		{
			CancelPath(ref currentPath);

			PathEdge.CancelEdge(ref currentPathEdge);
		}
		
		public static void CancelPath(ref Path currentPath)
		{
			if (currentPath != null)
			{
				currentPath.Show(false);
				currentPath.CleanUp();
				currentPath = null;
			}
		}

		// Use when you can't pass by ref
		public static (Path path, PathEdge pathEdge) CancelPath(Path currentPath, PathEdge currentPathEdge)
		{
			CancelPath(ref currentPath, ref currentPathEdge);
			return (currentPath, currentPathEdge);
		}
		
		// Use when you can't pass by ref
		public static Path CancelPath(Path currentPath)
		{
			CancelPath(ref currentPath);
			return currentPath;
		}

		#endregion Cancel Path

		#endregion Path Methods
    }
}