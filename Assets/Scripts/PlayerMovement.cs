using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	/*Public Fields*/
	public float forceToAdd = 0.04f;
	public float tiltThreshold = 0.4f;
	public float maxSpeed = 3f;
	public float magnitude;
	public Vector2 velocity;

	/*Private fields*/
	private RopeController ropeController;
	private Rigidbody2D rb2d = null;

	void Start () {
		//gives it upward force
		rb2d = GetComponent<Rigidbody2D>();
		rb2d.velocity = Vector2.up * 5;
		ropeController = gameObject.GetComponent<RopeController>();
	}


	void Update () {
		if (ropeController.ropeActive && magnitude < maxSpeed) {
			tiltForce();
		}
	}

	void FixedUpdate() {
		velocity = rb2d.velocity;
		magnitude = velocity.magnitude;
	}

	/*
	* Uses the mobile's gyroscope to detect tilting
	* Adds force to player if sufficient tilt AND player is on the rope
	*/
	void tiltForce() {
		if (Input.acceleration.x > tiltThreshold) {
			GetComponent<Rigidbody2D>().AddForce(Vector2.right*forceToAdd);
		}
		else if (Input.acceleration.x < -tiltThreshold) { 
			GetComponent<Rigidbody2D>().AddForce(Vector2.left*forceToAdd);
		} 

	}
}
