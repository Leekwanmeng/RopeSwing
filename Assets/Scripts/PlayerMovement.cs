using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	/*Public Fields*/
	public float forceToAdd = 2f;
	public float tiltThreshold = 0.4f;

	/*Private fields*/
	private RopeController ropeController;

	void Start () {
		//gives it upward force
		GetComponent<Rigidbody2D>().velocity = Vector2.up * 5;
		ropeController = gameObject.GetComponent<RopeController>();
	}


	void Update () {
		if (ropeController.ropeActive) {
			tiltForce();
		}
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
