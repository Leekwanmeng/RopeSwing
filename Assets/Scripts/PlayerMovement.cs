using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;



public class PlayerMovement : NetworkBehaviour {

	/*Public Fields*/
	public float swingForce = 10f;
	public float maxSwingSpeed = 4f;

	public float walkingSpeed = 10f;
	public float maxWalkSpeed = 5f;
	public float climbingStep = 2f;
	public float distanceToGround = 1.6f;

	public float xTiltThreshold = 0.5f;
	public float yTiltThreshold = 0.6f;

	public float magnitude;
	public bool facingRight = true;
	public Vector2 ropeHook;

	/*Private fields*/
	private TryRopeController tryRopeController;
	private Rigidbody2D rb2d;
	private Vector2 velocity;
 	private PlayerSyncSprite syncPos;
 	private Animator animator;
 	private bool isColliding;
 	
 	public float verticalInput;
 	public float horizontalInput;

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
		checkMovement();
		checkPlayerDirection();
		animateMovement();
	}

	// Update per physics frame
	void FixedUpdate() {
		if (tryRopeController.ropeActive) {
			climb();
			swing();
		} else if (!tryRopeController.ropeActive && isGrounded()) {
			walk();
		}
	}

	// Last Update
	void LateUpdate() {
		lockPlayerRotation();
	}

	/*
	* Called to determine if player can move, based on Mobile Input
	* Assigns type of movement
	*/
	void checkMovement() {
		if (Input.acceleration.y > yTiltThreshold || 
			Input.acceleration.y < -(yTiltThreshold + 0.2f)) {
			verticalInput = Input.acceleration.y;
		} else {
			verticalInput = 0;
		}

		if (Input.acceleration.x > xTiltThreshold || 
			Input.acceleration.x < -xTiltThreshold) {
			horizontalInput = Input.acceleration.x;
		} else {
			horizontalInput = 0;
		}
	}



	/*
	* Player Climbing movement
	*/
	void climb() {
		if (verticalInput > 0 && !isColliding) {
			tryRopeController.rope.distance -= Time.deltaTime * climbingStep;
		} else if (verticalInput < 0) {
			tryRopeController.rope.distance += Time.deltaTime * climbingStep;
		}
	}


	/*
	* Player Swinging movement
	*/
	void swing() {
		if (magnitude < maxSwingSpeed) {
			Vector2 playerToHookDirection = (ropeHook - (Vector2) transform.position).normalized;
			Vector2 perpendicularDirection = new Vector2(0f,1f);

			if (horizontalInput > 0) {
				perpendicularDirection = new Vector2(playerToHookDirection.y, -playerToHookDirection.x);
			} else if (horizontalInput < 0) {
				perpendicularDirection = new Vector2(-playerToHookDirection.y, playerToHookDirection.x);
			}

			Vector2 force = perpendicularDirection * swingForce;
			rb2d.AddForce(force);
		}
	}

	/*
	* Player Walking movement
	*/
	void walk() {
		if (magnitude < maxWalkSpeed) {
			if (horizontalInput != 0) {
            	rb2d.AddForce(new Vector2((horizontalInput * walkingSpeed - rb2d.velocity.x) * walkingSpeed, 0));
            	rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y);
			}
		}
	}



	/*
	* Checks player's horizontal movement and determines if player should flip
	*/
	public void checkPlayerDirection() {
		if ((velocity.x > 0.1f && !facingRight) || (velocity.x < 0.1f && facingRight)) {
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
	* Vertical raycast downward to "Ground" layer
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

    void OnTriggerStay2D(Collider2D colliderStay) {
        isColliding = true;
    }

    void OnTriggerExit2D(Collider2D colliderOnExit) {
        isColliding = false;
    }

    /*
	* Player Movement Animation
	*/
    void animateMovement() {
    	animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxWalkSpeed);
    }
}
