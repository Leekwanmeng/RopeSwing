using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;



public class PlayerMovement : NetworkBehaviour {

	/*Public Fields*/
	public float swingForce = 10f;
	public float walkForce = 20f;
	public float maxSwingSpeed = 5f;
	public float maxWalkSpeed = 5f;
	public float climbStep = 3f;

	public float tiltThreshold = 0.5f;
	public float magnitude;
	public bool facingRight = true;

	public float verticalInput;
	public float horizontalInput;

	/*Private fields*/
	private TryRopeController tryRopeController;
	private Rigidbody2D rb2d = null;
	private float distanceToGround = 1.6f;
	private Vector2 velocity;
 	private PlayerSyncSprite syncPos;
 	private Animator animator;
 	private bool isColliding;

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
		print(isColliding);
		if (!isLocalPlayer) {
			return;
		}
		velocity = rb2d.velocity;
		magnitude = velocity.magnitude;
		checkMovement();
		checkClimb();
		checkPlayerDirection();
		animateMovement();
		
	}

	// Update per physics frame
	void FixedUpdate() {
		if (tryRopeController.ropeActive) {
			checkSwing();
		} else if (!tryRopeController.ropeActive && isGrounded()) {
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
		if ((Input.acceleration.y > tiltThreshold) || (Input.acceleration.y < -tiltThreshold)) {
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


	void checkClimb() {
		
		if (verticalInput > 0) {
			tryRopeController.rope.distance -= Time.deltaTime * climbStep;
		} else if (verticalInput < 0) {
			tryRopeController.rope.distance += Time.deltaTime * climbStep;
		}
		
	}

	void checkSwing() {
		if (horizontalInput > 0) {
			rb2d.AddForce(Vector2.right * swingForce);
		} else if (horizontalInput < 0) {
			rb2d.AddForce(Vector2.left * swingForce);
		}
	}

	void checkWalk() {
		if (horizontalInput > 0) {
			rb2d.AddForce(Vector2.right * walkForce);
		} else if (horizontalInput < 0) {
			rb2d.AddForce(Vector2.left * walkForce);
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
    	Debug.DrawRay(transform.position, Vector2.down * distanceToGround, Color.green);
    	if (hitGround.collider != null) {
        	return true;
    	}
    	return false;
    }

    void animateMovement() {
    	animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxWalkSpeed);
    }

}