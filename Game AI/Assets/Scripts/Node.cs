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

    private int influenceValue;

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

    public int Value {
        get {
			return influenceValue;
        }

        set {
			influenceValue = value;
        }
    }
    #endregion

    //Data structure for information about a node. The visulization object is just for debuggin
    public Node(Vector3 _pos, float _cost, GameObject _visualizationPrefab, GameObject _visContainer) {
		Pos = new Vector3(_pos.x, 14, _pos.z);
        cost = _cost;
        Neighbors = new List<Node>();
		influenceValue = 0;

        //Set up scripts and objects to help visualize node position and relation to other nodes
        if (NodeManager.visualizeNodes) {
            visContainer = _visContainer;

            visualization = GameObject.Instantiate(_visualizationPrefab, Pos, Quaternion.identity);
            visualization.transform.parent = visContainer.transform;
            NodeViz = visualization.GetComponent<NodeVisualizer>();
            NodeViz.myNode = this;
        }
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
