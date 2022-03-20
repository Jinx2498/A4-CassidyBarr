using GameBrains.Actuators.Motion.Navigation.SearchGraph;
using GameBrains.Extensions.MonoBehaviours;
using GameBrains.Extensions.Vectors;
using GameBrains.GameManagement;
using UnityEngine;

namespace A4
{
    // TODO A4: Implement and Test automatic generation of a corner graph search space.
    // TODO for A4 (optional): Check all the math formulas. I'm not 100% sure I got them right.
    // TODO for A4: Add appropriate testing.
    
    public class CornerGraphSearchSpace : ExtendedMonoBehaviour
    {
        #region Members and Properties
        
        // Reference the levelInfo. Should be set in the inspector or could set it up in Awake.
        [SerializeField] LevelInfo levelInfo;

        Graph graph;
        const float MaxCast = 10f; // Probably to small to connect all corner waypoints

        #endregion Members and Properties

        #region Start

        public override void Start()
        {
            base.Start();

            graph = FindObjectOfType<Graph>();
            graph.IsLocked = false; // make sure the graph is not locked or you can't add nodes and edges.

            GenerateCornerGraphSearchSpace(); 
        }

        #endregion Start

        #region Members

        public void GenerateCornerGraphSearchSpace()
        {
            if (graph == null || graph.NodeCollection == null)
            {
                Debug.LogWarning("GenerateCornerGraphSearchSpace: Graph missing.");
                return;
            } 
            
            // TODO for A4: Add nodes at corners. Probably call a private helper method to determine the node placement.

            // I'm going to add edges using RaycastNodes which checks for LOS between nodes.
            // Change the default maximum range to check from 20 to something reasonable.
            Parameters.Instance.NodeCastMaximumDistance = MaxCast;
            graph.NodeCollection.RaycastNodes(); // Creates bidirectional edges between nodes with clear line of sight.
            
            // TODO for A4 (optional): Or you could manually add edges instead of raycasting. See example in GridSearchSpace.
        }
        
        #endregion Members
        
        #region Private/Protected Members
        
        // TODO for A4: You'll probably need some private helper methods here.

        void AddNodeAt(int v, int w)
        {
            float x = levelInfo.MapDataVtoX(v);
            float z = levelInfo.MapDataWtoZ(w);
            graph.NodeCollection.AddNode(new VectorXZ(x, z));
        }

        #endregion Private/Protected Members
    }
}