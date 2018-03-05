using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Astar_Seeking : MonoBehaviour {

	public Text seekingSharer;

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
		seekingSharer.text = "";

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

		while (!destinationNode.Traversable) 
		{
			destinationNode = allNodes [Random.Range (0, allNodes.Length - 1)];
		}

		nodeList = new List<Node> ();

		UpdateText ();
	}

	// Update is called once per frame
	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			destinationNode = allNodes [Random.Range (0, allNodes.Length - 1)];

			while (!destinationNode.Traversable) 
			{
				destinationNode = allNodes [Random.Range (0, allNodes.Length - 1)];
			}

			nodeList.Clear ();

			planning = true;

			UpdateText();
		}

		if (Input.GetKeyDown(KeyCode.Plus))
		{
			speed += 0.10f;
			UpdateText();
		}

		if (Input.GetKeyDown(KeyCode.Minus))
		{
			speed -= 0.10f;
			UpdateText();
		}

		if (Input.GetKeyDown(KeyCode.Equals))
		{
			speed += 0.10f;
			UpdateText();
		}

		if (Input.GetKeyDown(KeyCode.Underscore))
		{
			speed -= 0.10f;
			UpdateText();
		}

		if (planning) 
		{
			nodeList.Add (currentNode);
			//print ("New node added! " + currentNode.Pos);

			List<Node> options = currentNode.Neighbors;
			float distance = Mathf.Infinity;

			foreach (Node node in options) 
			{
				if (node.Traversable) 
				{
					float tempDistance = Vector3.Distance (node.Pos, destinationNode.Pos);

					if (tempDistance < distance) 
					{
						currentNode = node;
						distance = tempDistance;
					}
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

				while (!destinationNode.Traversable) 
				{
					destinationNode = allNodes [Random.Range (0, allNodes.Length - 1)];
				}

				planning = true;
				print ("Path completed!");
				UpdateText ();
				return;
			}

			if (FindClosestNode ().Equals (currentNode)) 
			{
				currentNode = nodeList [0];
				nodeList.Remove (currentNode);
				//print ("Aimed at: " + currentNode.Pos);
			}
		}

		UpdateText ();
	}

	public Node FindClosestNode()
	{
		Node closest = null;
		float distance = Mathf.Infinity;
		Vector3 position = transform.position;
		foreach (Node node in allNodes)
		{
			if (node.Traversable) 
			{
				Vector3 diff = node.Pos - position;
				float curDistance = diff.sqrMagnitude;
				if (curDistance < distance)
				{
					closest = node;
					distance = curDistance;
				}
			}
		}
		return closest;
	}

	private void UpdateText()
	{
		seekingSharer.text = "Currently seeking: " + destinationNode.Pos + '\n' + "From: " + currentNode.Pos + '\n' + "At: " + speed + " units/second" + '\n' + "Press space to reset destination node" + '\n' + "Press +/- to change speed"; 
	}
}
