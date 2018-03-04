using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour {
    public static int totalRowNodes = 40;

    public static Node[,]nodes;

    //Debugging object
    [SerializeField]
    private GameObject visualizationPrefab;

    //Parent all the node visualization objects to this object for neatness
    [SerializeField]
    private GameObject nodeVisContainer;

    //If false, no node visualization objects will be created
    public static bool visualizeNodes = false;

    void Awake () {
        SpawnNodes();
        SetNodeNeighbors();
    }
    
    private void SetNodeNeighbors() {
        foreach(Node _node in nodes) {
            _node.FindNeighbors();
        }
    }

    private void SpawnNodes() {
        nodes = new Node[totalRowNodes,totalRowNodes];

        Vector3 spawnPos = Vector3.zero;
        float xPos = 0; 
        float zPos = 0;

        //Extents works like radius, so needs to be multiplied by 2
        float terrainWidth = Terrain.activeTerrain.terrainData.bounds.extents.x * 2;

        //Spawn rows then columns
        for(int i = 0; i < totalRowNodes; i++) {
            for(int j = 0; j < totalRowNodes; j++) {
                //Find the x and z coordinate of this node then add it to the terrain's world position to get it's world position
                //Space inbtween each node scales with terrain width
                xPos = Terrain.activeTerrain.terrainData.bounds.min.x + (i * (terrainWidth / totalRowNodes));
                zPos = Terrain.activeTerrain.terrainData.bounds.min.y + (j * (terrainWidth / totalRowNodes));

                spawnPos = new Vector3(xPos, Terrain.activeTerrain.SampleHeight(new Vector3(xPos, 0, zPos)), zPos);
                Node newNode = new Node(spawnPos, 0.5f, visualizationPrefab, nodeVisContainer);
                newNode.GridPos = new Vector2(i, j);
                nodes[i, j] = newNode;
            }
        }
    }
}
