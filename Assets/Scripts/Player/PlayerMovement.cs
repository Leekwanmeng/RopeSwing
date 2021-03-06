﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;



public class PlayerMovement : NetworkBehaviour {

	/*Public Fields*/
	public float swingForce = 10f;
	public float walkForce = 7f;
	public float maxSwingSpeed = 5f;
	public float maxWalkSpeed = 4f;
	public float climbStep = 3f;

	public float tiltThreshold = 0.5f;
	public float magnitude;
	public bool facingRight = true;

	public float verticalInput;
	public float horizontalInput;

	public Vector2 ropeHook;

	/*Private fields*/
	private float distanceToGround = 1.6f;
	private float distanceToCeiling = 1.6f;
	private Vector2 ropeToHandOffset = new Vector2(0f, 0.5f);

	private RopeController ropeController;
	private Rigidbody2D rb2d = null;
	private Vector2 velocity;
 	private PlayerSyncSprite syncPos;
 	private Animator animator;
 	private float maxRopeDistance;

    // TESTING

    public void Construct() {
    }


    public void ConstructCheckDirection(Vector2 vel, bool right) {
        facingRight = right;
        velocity = vel;

    }

    public void SetDirection(bool right) {
        facingRight = right;
    }

    public void SetPosition(Vector3 myVec) {
        transform.position = myVec;
    }


    //Enumerator for testing

    public IEnumerator GetEnumerator() {
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
		animator = GetComponent<Animator>();
		facingRight = true;
		syncPos = GetComponent<PlayerSyncSprite>();
		maxRopeDistance = ropeController.maxRopeDistance;
	}

	// Update is called once per frame
	void Update () {
		if (!isLocalPlayer) {
			return;
		}
		velocity = rb2d.velocity;
		magnitude = velocity.magnitude;
		checkMovement();
		checkClimb();
		checkPlayerDirection();
		animateMovement();
		animateGround();
	}

	// Update per physics frame
	void FixedUpdate() {
		if (ropeController.ropeActive) {
			checkSwing();
		} else if (!ropeController.ropeActive){ //&& isGrounded()) {
			checkWalk();
		}
	}

	// Last Update
	void LateUpdate() {
		lockPlayerRotation();
		
	}

	/*
	* Called to determine if player can move
	* Assigns type of movement
	*/
	void checkMovement() {
		if ((Input.acceleration.y > tiltThreshold + 0.1f) || (Input.acceleration.y < -(tiltThreshold + 0.25f))) {
			verticalInput = Input.acceleration.y;
		} else {
			verticalInput = 0;
		}
		if ((Input.acceleration.x > tiltThreshold) || (Input.acceleration.x < -tiltThreshold)) {
			horizontalInput = Input.acceleration.x;
		} else {
			horizontalInput = 0;
		}
	}

	/*
	* Called to determine if player can climb
	* Lengthens / reduces rope.distance to simulate climbing based on vertical tilt
	* Pre-condition: Player on rope
	*/
	void checkClimb() {
		if (verticalInput > 0) {
			RaycastHit2D hitCeiling = Physics2D.Raycast(transform.position, Vector2.up, 
    						distanceToCeiling, 1 << LayerMask.NameToLayer("Wall"));

			if (hitCeiling.collider != null) return;
			ropeController.rope.distance -= Time.deltaTime * climbStep;

			// Force to add slight player movement for NetworkTransform sync
			rb2d.AddForce(Vector2.up);
		} else if (verticalInput < 0) {
			RaycastHit2D hitGround = Physics2D.Raycast(transform.position, Vector2.down, 
    						distanceToGround, 1 << LayerMask.NameToLayer("Ground"));
			if (hitGround.collider != null || ropeController.rope.distance > maxRopeDistance) return;
			ropeController.rope.distance += Time.deltaTime * climbStep;

			// Force to add slight player movement for NetworkTransform sync
			rb2d.AddForce(Vector2.down);
		}
		
	}

	/*
	* Called to determine if player can swing
	* Adds perpendicular force left and right based on horizontal tilt
	* Pre-condition: Player on rope
	*/
	void checkSwing() {
		if (ropeHook != Vector2.zero) {
			Vector2 playerToHookDirection = (ropeHook - (Vector2) transform.position).normalized;
			Vector2 perpendicularDirection;

			if (horizontalInput > 0) {
				perpendicularDirection = new Vector2(playerToHookDirection.y, -playerToHookDirection.x);
				rb2d.AddForce(perpendicularDirection * swingForce);
			} else if (horizontalInput < 0) {
				perpendicularDirection = new Vector2(-playerToHookDirection.y, playerToHookDirection.x);
				rb2d.AddForce(perpendicularDirection * swingForce);
			}
		}
	}

	/*
	* Called to determine if player can walk
	* Adds force left and right based on horizontal tilt
	* Pre-condition: Player is grounded
	*/
	void checkWalk() {
		if (horizontalInput != 0 && rb2d.velocity.x < maxWalkSpeed) {
	        rb2d.AddForce(new Vector2((horizontalInput * walkForce - rb2d.velocity.x) * 10f, 0f));
	        rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y);
	    }
	}

	/*
	* Checks player's horizontal movement and determines if player should flip
	* Calls public method in PlayerSyncSprite to send flip sprite on server side
	*/
	public void checkPlayerDirection() {
		if ((velocity.x > 0.1f && !facingRight) || (velocity.x < 0.1f && facingRight)) {
			facingRight = !facingRight;
			syncPos.CmdFlipSprite(facingRight);
		}
	}

	// For testing
	[Client]
    public void flipSprite() {
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
    public bool isGrounded() {
    	RaycastHit2D hitGround = Physics2D.Raycast(transform.position, Vector2.down, 
    						distanceToGround, 1 << LayerMask.NameToLayer("Ground"));
    	Debug.DrawRay(transform.position, Vector2.down * distanceToGround, Color.green);
    	if (hitGround.collider != null) {
        	return true;
    	}
    	return false;
    }

    /*
	* Sets values for player animation
	*/
    void animateMovement() {
    	animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxWalkSpeed);
    }

    /*
	* Sets values for player animation
	*/
    void animateGround() {
    	if (isGrounded()) {
    		animator.SetBool("grounded", true);
    	} else {
    		animator.SetBool("grounded", false);
    	}
    }

    

    //make player follow moving platform
    void OnCollisionStay2D(Collision2D coll)
    {
        
        if (coll.gameObject.tag == "MovingPlatform")
        {

            transform.SetParent(coll.gameObject.transform, true);
        }
        

    }
    

    void OnCollisionExit2D(Collision2D coll)
    {

        if (coll.gameObject.tag == "MovingPlatform")
        {
            transform.parent = null;
        }

    }
    

}