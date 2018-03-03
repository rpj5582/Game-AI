using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeVisualizer : MonoBehaviour {
    public Node myNode;
    private MeshRenderer myRenderer;

    private Vector3 origScale;
    public float hoveredSizeScale;
    
    void Awake() {
        myRenderer = GetComponent<MeshRenderer>();
        origScale = transform.localScale;
    }
    
    void OnMouseEnter() {
        myRenderer.material.color = Color.blue;
        transform.localScale = origScale * hoveredSizeScale;

        foreach(Node _neighbor in myNode.Neighbors) {
            _neighbor.NodeViz.SetVisualizeAsNeighbor(true);
        }
    }
    void OnMouseExit() {
        myRenderer.material.color = Color.white;
        transform.localScale = origScale;

        foreach (Node _neighbor in myNode.Neighbors) {
            _neighbor.NodeViz.SetVisualizeAsNeighbor(false);
        }
    }

    public void SetVisualizeAsNeighbor(bool _b) {
        transform.localScale = _b ? origScale * hoveredSizeScale : origScale;
        myRenderer.material.color = _b ? Color.green : Color.white;
    }
}
