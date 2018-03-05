using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Astar_Seeking : MonoBehaviour {

	public Node currentNode;
	public Node destinationNode;
	public List<Node> nodeList;
	private Node[] allNodes;

	public Rigidbody rigidbody;
	public float speed;
	public bool planning = true;

	// Use this for initialization
	void Start () 
	{
		allNodes = new Node[NodeManager.totalRowNodes * NodeManager.totalRowNodes];

		int k = 0;
		for (int i = 0; i < NodeManager.totalRowNodes; i++) 
		{
			for (int j = 0; j < NodeManager.totalRowNodes; j++) 
			{
				allNodes [k] = NodeManager.nodes[i, j];
				k++;
			}
		}

		currentNode = FindClosestNode ();
		destinationNode = allNodes [Random.Range (0, allNodes.Length - 1)];
		nodeList = new List<Node> ();
	}

	// Update is called once per frame
	void Update ()
	{
		if (planning) 
		{
			nodeList.Add (currentNode);
			//print ("New node added! " + currentNode.Pos);

			List<Node> options = currentNode.Neighbors;
			float distance = Mathf.Infinity;

			foreach (Node node in options) 
			{
				float tempDistance = Vector3.Distance (node.Pos, destinationNode.Pos);

				if (tempDistance < distance) 
				{
					currentNode = node;
					distance = tempDistance;
				}
			}

			if (currentNode.Equals(destinationNode)) 
			{
				nodeList.Add (destinationNode);

				print ("Destination node! " + destinationNode.Pos);
				print ("Path planned!");
				planning = false;
				currentNode = nodeList [0];
				nodeList.Remove (currentNode);
			}
		}

		if (!planning) 
		{
			rigidbody.position = Vector3.MoveTowards (rigidbody.position, currentNode.Pos, Time.deltaTime * speed);

			if (FindClosestNode().Equals(destinationNode)) 
			{
				//reached Destination
				destinationNode = allNodes [Random.Range (0, allNodes.Length - 1)];
				planning = true;
				print ("Path completed!");
				return;
			}

			if (FindClosestNode ().Equals (currentNode)) 
			{
				currentNode = nodeList [0];
				nodeList.Remove (currentNode);
				//print ("Aimed at: " + currentNode.Pos);
			}
		}
	}

	public Node FindClosestNode()
	{
		Node closest = null;
		float distance = Mathf.Infinity;
		Vector3 position = transform.position;
		foreach (Node node in allNodes)
		{
			Vector3 diff = node.Pos - position;
			float curDistance = diff.sqrMagnitude;
			if (curDistance < distance)
			{
				closest = node;
				distance = curDistance;
			}
		}
		return closest;
	}
}
