using GameBrains.Extensions.Vectors;
using GameBrains.Visualization;
using UnityEngine;

namespace GameBrains.Actuators.Motion.Navigation.PathManagement
{
    public class PathEdge
    {
        #region Members and Properties

        public bool trueEdge;
        public VectorXZ fromLocation;
        public EdgeBeacon edgeBeacon;
        public VectorXZ toLocation;
        public PointMarker toPointMarker;
        public PointBeacon toPointBeacon;

        #endregion Members and Properties

        #region Contructor

        public PathEdge(
            bool trueEdge,
            VectorXZ fromLocation,
            VectorXZ toLocation)
        {
            this.trueEdge = trueEdge;
            this.fromLocation = fromLocation;
            this.toLocation = toLocation;
        }

        #endregion Contructor

        #region Methods
        
        #region Show

        public void Show(bool show)
        {
            ShowEdgeBeacon(show);
            ShowPointMarker(show);
            ShowPointBeacon(show);
        }

        void ShowPointBeacon(bool show)
        {
            if (toPointBeacon == null)
            {
                toPointBeacon = CreatePointBeacon(toLocation);
            }

            toPointBeacon.Hide(!show);
        }

        void ShowPointMarker(bool show)
        {
            if (toPointMarker == null)
            {
                toPointMarker = CreatePointMarker(toLocation);
            }

            toPointMarker.Hide(!show);
        }

        void ShowEdgeBeacon(bool show)
        {
            if (edgeBeacon == null)
            {
                edgeBeacon = CreateEdgeBeacon(fromLocation, toLocation);
            }

            edgeBeacon.Hide(!show);
        }
        
        #endregion Show

        #region Clean up
        
        public void CleanUp()
        {
            Object.Destroy(edgeBeacon);
            edgeBeacon = null;
				
            Object.Destroy(toPointMarker);
            toPointMarker = null;
				
            Object.Destroy(toPointBeacon);
            toPointBeacon = null;
        }
        
        #endregion Clean Up
        
        #region Cancel Edge

        public static void CancelEdge(ref PathEdge currentPathEdge)
        {
            if (currentPathEdge != null)
            {
                currentPathEdge.Show(false);
                currentPathEdge.CleanUp();
                currentPathEdge = null;
            }
        }

        // Use when you can't pass by ref
        public static PathEdge CancelEdge(PathEdge currentPathEdge)
        {
            CancelEdge(ref currentPathEdge);
            return currentPathEdge;
        }

        #endregion Cancel Edge
        
        #region Create Visualizers

        static PointMarker CreatePointMarker(VectorXZ location)
        {
            var pointMarker = ScriptableObject.CreateInstance<PointMarker>();
            pointMarker.Draw(location);
            pointMarker.Hide(true);
            return pointMarker;
        }

        static PointBeacon CreatePointBeacon(VectorXZ location)
        {
            var pointBeacon = ScriptableObject.CreateInstance<PointBeacon>();
            pointBeacon.Draw(location);
            pointBeacon.Hide(true);
            return pointBeacon;
        }
		
        static EdgeBeacon CreateEdgeBeacon(VectorXZ fromLocation, VectorXZ toLocation)
        {
            var edgeBeacon = ScriptableObject.CreateInstance<EdgeBeacon>();
            edgeBeacon.Draw(fromLocation, toLocation);
            edgeBeacon.Hide(true);
            return edgeBeacon;
        }
        
        #endregion Create Visualizers
        
        #region To String

        public override string ToString()
        {
            return $"{fromLocation}-->{toLocation}";
        }
        
        #endregion To String

        #endregion Methods
    }
}