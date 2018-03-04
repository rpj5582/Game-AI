using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MCMovement : MonoBehaviour {
	public Vector3 forward = new Vector3(0.0f, 0.0f, 1.0f); //direction the object is naturally facing
	public float speed = 0.0f;
	public float maxSpeed = 30.0f;
	private float slowDown = 0.97f;
	private float speedIncrement = 10.0f;

	private float angularSpeed = 180.0f;
	private float angle = 0.0f;
	private bool useSlowdown = true;

	// Use this for initialization
	void Start ()
	{
		//position = gameObject.transform.position;
	}

	// Update is called once per frame
	void Update ()
	{
		//Is it moving or slowing down?
		if(Input.GetKey(KeyCode.W))
		{
			speed += speedIncrement * Time.deltaTime;
		}
		else if(useSlowdown)
		{
			speed *= slowDown;
		}

		//Check max and min speed
		if(speed > maxSpeed)
		{
			speed = maxSpeed;
		}
		else if(speed < 0.01f)
		{
			speed = 0.0f;
		}

		//Increment angle or orientation
		if(Input.GetKey(KeyCode.A))
		{
			angle -= angularSpeed * Time.deltaTime;
		}
		else if(Input.GetKey(KeyCode.D))
		{
			angle += angularSpeed * Time.deltaTime;
		}

		//Keep the angle between 0 and 360
		angle %= 360.0f;

		//transform.position = position; // set the position of the game object
		transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f); //Set the orientation of the Game Object

		//Current position increment by the forward vector times the speed increment
		float speedBasedOnTime = speed * Time.deltaTime; //this is just to make it more obvious
		Vector3 forwardInWorldSpace = transform.rotation * forward;
		transform.position += forwardInWorldSpace * speedBasedOnTime;
		/*
		//Check the Forward in world space
		if(gameObject.tag == "Player")
			Debug.Log(forwardInWorldSpace);
		*/
	}


}
