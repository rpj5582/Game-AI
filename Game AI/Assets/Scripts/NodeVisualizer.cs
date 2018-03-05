using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Contains no pathfinding functionality.
//Used to display nodes to make it easier to visualize node state and position
public class NodeVisualizer : MonoBehaviour {
    public Node myNode;
    private MeshRenderer myRenderer;

    private Vector3 origScale;
    public float hoveredSizeScale;
    
    void Awake() {
        myRenderer = GetComponent<MeshRenderer>();
        origScale = transform.localScale;
    }
    
    //Highlights this node and its nieghbors
    void OnMouseEnter() {
        if (!myNode.Traversable) return;
        myRenderer.material.color = Color.blue;
        transform.localScale = origScale * hoveredSizeScale;

        foreach(Node _neighbor in myNode.Neighbors) {
            _neighbor.NodeViz.SetVisualizeAsNeighbor(true);
        }
    }

    //Remove any visuals from this node and its neighbors
    void OnMouseExit() {
        if (!myNode.Traversable) return;
        myRenderer.material.color = Color.white;
        transform.localScale = origScale;

        foreach (Node _neighbor in myNode.Neighbors) {
            _neighbor.NodeViz.SetVisualizeAsNeighbor(false);
        }
    }

    //Changes the color and scale of this node to show that it is the nieghbor of
    //whatever node is currently moused over
    public void SetVisualizeAsNeighbor(bool _b) {
        if (!myNode.Traversable) return;
        transform.localScale = _b ? origScale * hoveredSizeScale : origScale;
        myRenderer.material.color = _b ? Color.green : Color.white;
    }

    public void SetTraversable(bool _b) {
        myRenderer.material.color = myNode.Traversable ? Color.white : Color.red;
    }
}
