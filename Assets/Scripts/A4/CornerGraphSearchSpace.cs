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

        [SerializeField]int cellSizeX = 4;
        [SerializeField]int cellSizeZ = 4;
        
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

            int blockedtracker= 0;
            int openTracker = 0;

            int countX = (int)((levelInfo.sizeV - 1) / 8f - cellSizeX / 2f);
            int countZ = (int)((levelInfo.sizeW - 1) / 8f - cellSizeZ / 2f);

            for (int x = 0; x <= countX; x += cellSizeX)
            {
                for (int z = 0; z <= countZ; z += cellSizeZ)
                {
                    // Add node at (x, z) if clear. Mirror to all four quadrants. Don't duplicate (0,z) or (x, 0).
                    if(CheckIfClearAt(x, z) == true) {
                        openTracker += 1;
                    } else {
                        blockedtracker +=1;
                    }
                    if (z != 0) { 
                        // AddNodeIfClear(x, -z); 
                        if(CheckIfClearAt(x, z) == true) {
                            openTracker += 1;
                        } else {
                            blockedtracker +=1;
                        }
                    }
                    if (x != 0) { 
                        // AddNodeIfClear(-x, z); 
                        if(CheckIfClearAt(x, z) == true) {
                            openTracker += 1;
                        } else {
                            blockedtracker +=1;
                        }
                    }
                    if ( x != 0 && z != 0) { 
                        if(CheckIfClearAt(x, z) == true) {
                            openTracker += 1;
                        } else {
                            blockedtracker +=1;
                        }
                        // AddNodeIfClear(-x, -z); 
                    }

                    if(blockedtracker == 1) {
                        AddNodeAt(x, z);
                    }
                }
            }

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

        bool CheckIfClearAt(int x, int z)
        {
            // TODO: CS-check or Math-check this math. I only Engineer-checked it.
            int vMin = levelInfo.MapDataXtoV(x - cellSizeX / 2f);
            int vMax = levelInfo.MapDataXtoV(x + cellSizeX / 2f);
            
            int wMin = levelInfo.MapDataZtoW(z - cellSizeZ / 2f);
            int wMax = levelInfo.MapDataZtoW(z + cellSizeZ / 2f);

            for (int v = vMin; v <= vMax; v++)
            {
                for (int w = wMin; w <= wMax; w++)
                {
                    if (v < 0 || v >= levelInfo.sizeV || w < 0 || w >= levelInfo.sizeW)
                    {
                        // This should happen if we got the math right, but ...
                        Debug.LogError($"CheckIfClearAt(x={x}, z={z}): produced out of range (v={v}, w={w})");
                    }
                    
                    if (levelInfo.mapData[v, w] != 0) // not clear (not ground)
                    {
                        return false;
                    }
                }
            }

            return true; // clear (ground)
        }

        void AddNodeAt(int v, int w)
        {
            float x = levelInfo.MapDataVtoX(v);
            float z = levelInfo.MapDataWtoZ(w);
            graph.NodeCollection.AddNode(new VectorXZ(x, z));
        }

        #endregion Private/Protected Members
    }
}