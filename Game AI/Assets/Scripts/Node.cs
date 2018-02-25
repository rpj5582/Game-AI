using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {
    private Vector3 pos;
    private float cost;

    private GameObject visualization, visContainer;

    //Data structure for information about a node. The visulization object is just for debuggin
    public Node(Vector3 _pos, float _cost, GameObject _visualizationPrefab, GameObject _visContainer) {
        pos = _pos;
        cost = _cost;

        if(NodeManager.visualizeNodes) {
            visContainer = _visContainer;

            visualization = GameObject.Instantiate(_visualizationPrefab, pos, Quaternion.identity);
            visualization.transform.parent = visContainer.transform;
        }
    }

    public float CalculateCost() {
        return Mathf.Infinity;
    }
}
