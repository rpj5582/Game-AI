using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour {
    struct NodeQueueMember
    {
        public Node node;
        public int influenceValue;

        public NodeQueueMember(Node _node, int _influenceValue)
        {
            node = _node;
            influenceValue = _influenceValue;
        }
    }

    public static int totalRowNodes = 40;

    public static Node[,]nodes;

    //Debugging object
    [SerializeField]
    private GameObject visualizationPrefab;

    //Parent all the node visualization objects to this object for neatness
    [SerializeField]
    private GameObject nodeVisContainer;

    //If false, no node visualization objects will be created
    public static bool visualizeNodes = true;

    void Awake () {
        SpawnNodes();
        SetNodeNeighbors();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            nodeVisContainer.SetActive(!nodeVisContainer.activeSelf);
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            RefreshInfluenceMap();
        }
    }

    private void RefreshInfluenceMap()
    {
        // Reset the map
        for (int i = 0; i < totalRowNodes; i++)
        {
            for (int j = 0; j < totalRowNodes; j++)
            {
                nodes[i, j].Value = 0;
            }
        }

        // Loop through all units and update the map with their positions and influence values
        GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");
        for (int i = 0; i < units.Length; i++)
        {
            // Snap the unit's position to the grid so we can find the corresponding node
            Unit unit = units[i].GetComponent<Unit>();
            Vector3 snappedUnitPosition = SnapPositionToGrid(unit.transform.position);

            // Calculate the row and column of the node in the array (algebraic inverse of the xPos and zPos used when spawning the nodes)
            float terrainWidth = Terrain.activeTerrain.terrainData.bounds.extents.x * 2;
            int row = (int)((snappedUnitPosition.x - Terrain.activeTerrain.terrainData.bounds.min.x) / (terrainWidth / totalRowNodes));
            int col = (int)((snappedUnitPosition.z - Terrain.activeTerrain.terrainData.bounds.min.y) / (terrainWidth / totalRowNodes));

            Node node = nodes[row, col];

            if(unit.Team == Unit.TEAM.RED)
                UpdateNodes(node, (int)unit.Level);
            else
                UpdateNodes(node, -(int)unit.Level);
        }
    }

    private void UpdateNodes(Node startNode, int startValue)
    {
        // Used to tell if we should add 1 or subtract 1 to get the influence value closer to zero
        // (since negative influence is the other team)
        int dir;

        if (startValue == 0) return; // Early out if function called with 0 influence
        else if (startValue > 0) dir = -1;
        else dir = 1;
        
        // A NodeQueueMember is just a struct that contains a Node and an influence value.
        // A queue is used for a breadth first search, so both the node and the corresponding influence value need to be stored in the queue.
        Queue<NodeQueueMember> nodeQueue = new Queue<NodeQueueMember>();

        // Enqueue the starting node
        nodeQueue.Enqueue(new NodeQueueMember(startNode, startValue));

        for (int i = 0; i < nodeQueue.Count; i++)
        {
            NodeQueueMember queueMember = nodeQueue.Dequeue();
            if (queueMember.influenceValue == 0) continue;

            // Add to the current value to blend between the two teams.
            queueMember.node.Value += queueMember.influenceValue;

            // Add each neighbor to the queue and save the influence value minus (or plus, for negative values) 1.
            List<Node> neighbors = queueMember.node.Neighbors;
            for (int j = 0; j < neighbors.Count; j++)
            {
                nodeQueue.Enqueue(new NodeQueueMember(neighbors[j], queueMember.influenceValue + 1 * dir));
            }
        }
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

    private Vector3 SnapPositionToGrid(Vector3 position)
    {
        float terrainWidth = Terrain.activeTerrain.terrainData.bounds.extents.x * 2;
        float snapIncrement = (terrainWidth / totalRowNodes);

        float xPos = Mathf.Round(position.x / snapIncrement) * snapIncrement;
        float yPos = Mathf.Round(position.y / snapIncrement) * snapIncrement;
        float zPos = Mathf.Round(position.z / snapIncrement) * snapIncrement;

        return new Vector3(xPos, yPos, zPos);
    }
}
