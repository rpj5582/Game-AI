﻿using System.Collections;
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
    
	//set color depending on corresponding node's current value (red, blue, grey, default to grey)

	void Update(){
		if (myNode.Value == 0) {
			myRenderer.material.color = Color.grey;
		} else if (myNode.Value > 0) {
			myRenderer.material.color = Color.red;
		} else {
			myRenderer.material.color = Color.blue;
		}
	}
}
