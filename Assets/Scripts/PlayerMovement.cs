using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMovement : NetworkBehaviour {

	/*Public Fields*/
	public float forceToAdd = 0.04f;
	public float tiltThreshold = 0.4f;
	public float maxSpeed = 3f;
	public float magnitude;

	/*Private fields*/
	private RopeController ropeController;
	private Rigidbody2D rb2d = null;
	private float distanceToGround = 1.4f;
	private Vector2 velocity;
	private bool facingRight = true;

	void Start () {
		rb2d = GetComponent<Rigidbody2D>();
		rb2d.velocity = Vector2.up * 5;		//gives it upward force
		ropeController = gameObject.GetComponent<RopeController>();
	}

	// Update is called once per frame
	void Update () {
		velocity = rb2d.velocity;
		magnitude = velocity.magnitude;
		if (ropeController.ropeActive && magnitude < maxSpeed) {
			tiltForce();
		}
		checkPlayerDirection();
	}

	// Update per physics frame
	void FixedUpdate() {
		
	}

	// Last Update
	void LateUpdate() {
		lockPlayerRotation();
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
		if (velocity.x > 0.05f && !facingRight) {
			flip();
		} else if (velocity.x < -0.05f && facingRight) {
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

    /*
	* Locks player's rotation
	*/
    void lockPlayerRotation() {
    	transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }


    /*
	* Checks if player is grounded
	* Vertical raycast downward to "Grund" layer
	*
	* @return true is grounded
	*/
    bool isGrounded() {
    	RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 
    						distanceToGround, 1 << LayerMask.NameToLayer("Ground"));
    	// Debug.DrawRay(transform.position, Vector2.down * distanceToGround, Color.green);
    	if (hit.collider != null) {
        	return true;
    	}
    	return false;
    }
}
