using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;



public class PlayerMovement : NetworkBehaviour {

	/*Public Fields*/
	public float swingForce = 200f;
	public float walkForce = 30f;
	public float tiltThreshold = 0.4f;
	public float maxSpeed = 20f;
	public float magnitude;

	/*Private fields*/
	private RopeController ropeController;
	private Rigidbody2D rb2d = null;
	private float distanceToGround = 2f;
	private Vector2 velocity;
    //made public for testing
    public bool facingRight = true;



    // TESTING

    public void Construct()
    {
    }


    public void ConstructCheckDirection(Vector2 vel, bool right)
    {
        facingRight = right;
        velocity = vel;

    }

    public void SetDirection(bool right)
    {
        facingRight = right;
    }

    public void SetPosition(Vector3 myVec)
    {
        transform.position = myVec;
    }


    //Enumerator for testing

    public IEnumerator GetEnumerator()
    {
        return null;
        //fix this later
    }



    // MAIN CODE

    public override void OnStartLocalPlayer() {
         Camera.main.GetComponent<SmoothCamera>().setPlayer(gameObject);
    }


	void Start () {
		rb2d = GetComponent<Rigidbody2D>();
		rb2d.velocity = Vector2.up * 5;		//gives it upward force
		ropeController = gameObject.GetComponent<RopeController>();
	}

	// Update is called once per frame
	void Update () {
		if (!isLocalPlayer) {
			return;
		}
		velocity = rb2d.velocity;
		magnitude = velocity.magnitude;
		movement();
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
	* Called to determine if player can move
	* Assigns type of movement
	*/
	void movement() {
		if (ropeController.ropeActive && magnitude < maxSpeed) {
			tiltForce(swingForce);
		} else if (!ropeController.ropeActive 
				&& isGrounded() && magnitude < maxSpeed) {
			tiltForce(walkForce);
		}
	}


	/*
	* Uses the mobile's gyroscope to detect tilting
	* Applies force to RigidBody2D accordingly
	*
	* @param Type and value of force
	*/
	void tiltForce(float force) {
		if (Input.acceleration.x > tiltThreshold) {
			GetComponent<Rigidbody2D>().AddForce(Vector2.right*force);
		}
		else if (Input.acceleration.x < -tiltThreshold) { 
			GetComponent<Rigidbody2D>().AddForce(Vector2.left*force);
		} 

	}

	/*
	* Checks player's horizontal movement and determines if player should flip
	*/
	public void checkPlayerDirection() {
		if (velocity.x > 0.05f && !facingRight) {
			flip();
		} else if (velocity.x < -0.05f && facingRight) {
			flip();
		}
	}

	/*
	* Flips player's transform
	*/
	public void flip() {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }


    /*
    public void flipMock(Transform Target)
    {
        facingRight = !facingRight;
        Vector3 theScale = Target.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    */


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
    public bool isGrounded() {
    	RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 
    						distanceToGround, 1 << LayerMask.NameToLayer("Ground"));
    	// Debug.DrawRay(transform.position, Vector2.down * distanceToGround, Color.green);
    	if (hit.collider != null) {
        	return true;
    	}
    	return false;
    }
}
