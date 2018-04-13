using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapVisibility : MonoBehaviour {
	public GameObject nodes;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.I))
		{
			nodes.active = !nodes.active;
		}
	}
}
