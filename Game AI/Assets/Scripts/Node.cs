using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {
    private Vector3 worldPos;
    private Vector2 gridPos;

    private float cost;
    private List<Node> neighbors;

    private GameObject visualization, visContainer;
    private NodeVisualizer nodeViz;

    private bool traversable;

    #region Properties
    public List<Node> Neighbors {
        get {
            return neighbors;
        }

        set {
            neighbors = value;
        }
    }

    public Vector3 Pos {
        get {
            return worldPos;
        }

        set {
            worldPos = value;
        }
    }

    public Vector2 GridPos {
        get {
            return gridPos;
        }

        set {
            gridPos = value;
        }
    }

    public NodeVisualizer NodeViz {
        get {
            return nodeViz;
        }

        set {
            nodeViz = value;
        }
    }

    public bool Traversable {
        get {
            return traversable;
        }

        set {
            traversable = value;
        }
    }
    #endregion

    //Data structure for information about a node. The visulization object is just for debuggin
    public Node(Vector3 _pos, float _cost, GameObject _visualizationPrefab, GameObject _visContainer) {
        Pos = _pos;
        cost = _cost;
        Neighbors = new List<Node>();

        ReadjustHeightForBuildings();

        //Set up scripts and objects to help visualize node position and relation to other nodes
        if (NodeManager.visualizeNodes) {
            visContainer = _visContainer;

            visualization = GameObject.Instantiate(_visualizationPrefab, Pos, Quaternion.identity);
            visualization.transform.parent = visContainer.transform;
            NodeViz = visualization.GetComponent<NodeVisualizer>();
            NodeViz.myNode = this;
            NodeViz.SetTraversable(Traversable);
        }
    }

    private void ReadjustHeightForBuildings() {
        Traversable = true;

        //Excludes all layers that are not buildings
        int buildingLayermask = 1 << LayerMask.NameToLayer("Building");
        int waterLayermask = 1 << LayerMask.NameToLayer("Water");
        int layerMask = buildingLayermask | waterLayermask;

        RaycastHit hit;
        Vector3 rayStartPos = Pos + Vector3.up * 30;

        //Shoot a ray downwards towards this node. If it hits anything on the building layer, move the node to that hit point
        if (Physics.Raycast(new Ray(rayStartPos, -Vector3.up), out hit, Vector3.Distance(Pos, rayStartPos), layerMask)) {
            Pos = hit.point;
            Traversable = false;
        }

        #region Sphere Collider version
        //Finds any colliders within overlap range of this node
        /*Collider[] overlappedColliders = Physics.OverlapSphere(Pos, 0.1f, layerMask);

        //Augments the node position to compensate
        if(overlappedColliders.Length > 0) {
            Pos += new Vector3(0, overlappedColliders[0].gameObject.GetComponent<Collider>().bounds.extents.y * 2, 0);
        }*/
        #endregion

    }

    public float CalculateCost() {
        return Mathf.Infinity;
    }

    //Adds an X and Y offset to the grid position of this node to find all it's neighbors
    //John -- This is pretty inefficient. We could combine the two inner 'if' statements into one if we start running into
    //any kind of performance bottleneck here. Leaving it for readability now.
    public void FindNeighbors() {
        for(int x = -1; x < 2; x++) {
            for(int y = -1; y < 2; y++) {

                //Make sure we don't add this node to its own neighbor list
                if (x == 0 && y == 0) continue;
                
                //Checks to make sure that the neighbor we're looking for is actually on the grid
                if (((int)GridPos.x + x) < NodeManager.totalRowNodes && ((int)GridPos.x + x) >= 0) {
                    if(((int)GridPos.y + y) < NodeManager.totalRowNodes && ((int)GridPos.y + y) >= 0) {
                        Neighbors.Add(NodeManager.nodes[(int)GridPos.x + x, (int)GridPos.y + y]);
                    }  
                }
            }
        }
    }
}
