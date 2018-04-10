using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;



public class PlayerMovement : NetworkBehaviour {

	/*Public Fields*/
	public float swingForce = 4f;
	public float walkForce = 0.03f;
	public float maxSwingSpeed = 3.2f;
	public float maxWalkSpeed = 3f;

	public float tiltThreshold = 0.5f;
	public float magnitude;
	public bool facingRight = true;

	/*Private fields*/
	private TryRopeController tryRopeController;
	private Rigidbody2D rb2d = null;
	private float distanceToGround = 1.6f;
	private Vector2 velocity;
 	private PlayerSyncSprite syncPos;
 	private Animator animator;

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
		tryRopeController = gameObject.GetComponent<TryRopeController>();
		animator = GetComponent<Animator>();
		facingRight = true;
		syncPos = GetComponent<PlayerSyncSprite>();
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
		animateMovement();
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
		if (tryRopeController.ropeActive) {
			if ((Input.acceleration.y > tiltThreshold) || (Input.acceleration.y < -tiltThreshold)) {
				climb();
			} else if ((Input.acceleration.x > tiltThreshold) || (Input.acceleration.x < -tiltThreshold)) {
				swing();
			}
		} else if (!tryRopeController.ropeActive && isGrounded()) {
			walk();
		}
	}

	void climb() {
		if (Input.acceleration.y > tiltThreshold) {
			tryRopeController.rope.distance += 0.5f;
		} else if (Input.acceleration.y < -tiltThreshold) {
			tryRopeController.rope.distance -= 0.5f;
		}
	}

	void swing() {
		if (magnitude < maxSwingSpeed) {
			if (Input.acceleration.x > tiltThreshold) {
				GetComponent<Rigidbody2D>().AddForce(Vector2.right * swingForce);
			} else if (Input.acceleration.x < -tiltThreshold) {
				GetComponent<Rigidbody2D>().AddForce(Vector2.left * swingForce);
			}
		}
	}

	void walk() {
		if (magnitude < maxWalkSpeed) {
			if (Input.acceleration.x > tiltThreshold) {
				GetComponent<Rigidbody2D>().AddForce(Vector2.right * walkForce);
			} else if (Input.acceleration.x < -tiltThreshold) {
				GetComponent<Rigidbody2D>().AddForce(Vector2.left * walkForce);
			}
		}
	}


	/*
	* Uses the mobile's gyroscope to detect tilting
	* Applies force to RigidBody2D accordingly
	*
	* @param Type and value of force
	*/
	void tiltHorizontalForce(float force) {
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
		if ((velocity.x > 0.05f && !facingRight) || (velocity.x < 0.05f && facingRight)) {
			facingRight = !facingRight;
			syncPos.CmdFlipSprite(facingRight);
		}
	}


	//////////////////////////////////
	//////////// OBSOLETE ////////////
	//////////////////////////////////
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
    	RaycastHit2D hitPlayer = Physics2D.Raycast(transform.position, Vector2.down, 
    						distanceToGround, 1 << LayerMask.NameToLayer("Player"));
    	// Debug.DrawRay(transform.position, Vector2.down * distanceToGround, Color.green);
    	if (hitGround.collider != null || hitPlayer.collider != null) {
        	return true;
    	}
    	return false;
    }

    void animateMovement() {
    	animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxWalkSpeed);
    }
}
