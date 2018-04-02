using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	/*Public Fields*/
	public float forceToAdd = 0.04f;
	public float tiltThreshold = 0.4f;
	public float maxSpeed = 3f;
	public float magnitude;

	/*Private fields*/
	private RopeController ropeController;
	private Rigidbody2D rb2d = null;
	private Vector2 velocity;
	private bool facingRight = true;

	void Start () {
		//gives it upward force
		rb2d = GetComponent<Rigidbody2D>();
		rb2d.velocity = Vector2.up * 5;
		ropeController = gameObject.GetComponent<RopeController>();
	}


	void Update () {
		velocity = rb2d.velocity;
		magnitude = velocity.magnitude;
		if (ropeController.ropeActive && magnitude < maxSpeed) {
			tiltForce();
		}
		checkPlayerDirection();
	}

	void FixedUpdate() {
		
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

	/*
	* Checks player's horizontal movement and determines if player should flip
	*/
	void checkPlayerDirection() {
		if (velocity.x > 0 && !facingRight) {
			flip();
		} else if (velocity.x < 0 && facingRight) {
			flip();
		}
	}

	/*
	* Flips player's transform
	*/
	void flip() {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
