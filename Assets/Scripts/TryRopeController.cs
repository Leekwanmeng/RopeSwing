using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class TryRopeController : NetworkBehaviour {

	/*Public Fields*/
	
	[SyncVar]
	public bool ropeActive;
	// [SyncVar (hook = "OnRopeActve")]
	// public Vector2 startPosition;
	// [SyncVar]
	// public Vector2 endPosition;
	public DistanceJoint2D rope;
	public LineRenderer lineRenderer;

	/*Private Fields*/
	private Animator animator;
	private Vector2 touchPosition;
	private Vector2 playerPosition;
	private PlayerMovement playerMovement;

	public override void OnStartLocalPlayer() {
         Camera.main.GetComponent<SmoothCamera>().setPlayer(gameObject);
    }

	void Awake() {
		rope = GetComponent<DistanceJoint2D>();
		lineRenderer = GetComponent<LineRenderer>();
		animator = GetComponent<Animator>();
		lineRenderer.enabled = false;
		rope.enabled = false;
		playerPosition = transform.position;
		playerMovement = GetComponent<PlayerMovement>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!isLocalPlayer) {
			return;
		}
		playerPosition = transform.position;
		TouchDetection();
		ifRopeActive();
		LineRender();
		// CmdRenderRope();
		animateSwing();
		
	}

	// Update per physics frame
	void FixedUpdate() {
	}

	// Last update
	void LateUpdate() {
		
	}

	// [Client]
	// void AssignPositions(Vector2 startPos, Vector2 endPos) {
	// 	CmdPositions(startPos, endPos);
	// }


	// [Command]
	// void CmdPositions(Vector2 startPos, Vector2 endPos) {
	// 	startPosition = startPos;
	// 	endPosition = endPos;
	// }



	void LineRender() {
		if (!rope.enabled) {
			return;
		}
		Vector2 playerHandPosition = playerPosition + new Vector2(0f, 0.2f);
		lineRenderer.positionCount = 2;
		lineRenderer.SetPosition(0, playerHandPosition);
		lineRenderer.SetPosition(1, rope.connectedAnchor);
		lineRenderer.enabled = true;
	}


	// 	// if (rope != null) {
	// 	// 	lineRenderer.enabled = true;
	// 	// 	lineRenderer.positionCount = 2;
	// 	// 	lineRenderer.SetPosition(0, gameObject.transform.position);
	// 	// 	lineRenderer.SetPosition(1, rope.connectedAnchor);
	// 	// 	animator.SetBool("ropeActive", true);
	// 	// } else {
	// 	// 	lineRenderer.enabled = false;
	// 	// 	animator.SetBool("ropeActive", false);
	// 	// }
	// }


	/*
	* Detects Touches on mobile phone
	* Creates rope on touch if no rope exists, else removes current rope
	*/
	void TouchDetection() {
		if (Input.touchCount > 0) {
			Touch touch = Input.GetTouch(0);
			if (touch.phase == TouchPhase.Began ||
				touch.phase == TouchPhase.Stationary ||
				touch.phase == TouchPhase.Moved) {

				touchPosition = Camera.main.ScreenToWorldPoint
				(new Vector2(touch.position.x, touch.position.y));


			} else if (touch.phase == TouchPhase.Ended) {
				bool grounded = gameObject.GetComponent<PlayerMovement>().isGrounded();
				if (!ropeActive && grounded) {
					touchPosition = Camera.main.ScreenToWorldPoint
					(new Vector2(touch.position.x, touch.position.y));
					ShootRope(touchPosition);
				} else if (ropeActive) {
					ResetRope();
				}
			}
		}
	}


	/*
	* Raycasts to clicked position if it collides with a wall
	* Adds new rope if successful while deleting previous rope
	* 
	* @param Player touch position on mobile screen
	*/
	void ShootRope(Vector2 touchPosition) {
		Vector2 direction = touchPosition - playerPosition;

		RaycastHit2D hit = Physics2D.Raycast (playerPosition, direction, 
							Mathf.Infinity, 1 << LayerMask.NameToLayer("Wall"));

		if (hit.collider != null) {

			transform.GetComponent<Rigidbody2D>().AddForce(
				new Vector2(0f, 5f), ForceMode2D.Impulse);
			playerMovement.ropeHook = hit.point;
			rope.enableCollision = true;
			rope.distance = Vector2.Distance(playerPosition, hit.point);
			rope.connectedAnchor = hit.point;
			rope.enabled = true;
			
		}
		// AssignPositions(playerPosition, rope.connectedAnchor);
	}


	/*
	* Destroys rope if exists
	*/
	void ResetRope() {
		rope.enabled = false;

		lineRenderer.enabled = false;
		lineRenderer.positionCount = 2;
		lineRenderer.SetPosition(0, playerPosition);
		lineRenderer.SetPosition(1, playerPosition);
		playerMovement.ropeHook = Vector2.zero;
	}


	/*
	* Client method to check for rope
	* Calls corresponding Client -> Server methods to update ropeActive boolean
	*/
	[Client]
	void ifRopeActive() {
		if (rope.enabled) {
			CmdRopeActive();
		} else {
			CmdRopeNotActive();
		}
	}

	/*
	* Command methods to update ropeActive and send data to server
	*/
	[Command]
	void CmdRopeActive() {
		ropeActive = true;
	}
	[Command]
	void CmdRopeNotActive() {
		ropeActive = false;
	}



	/*
	* Swinging Player Animation
	*/
	void animateSwing() {
		if (rope.enabled) {
			animator.SetBool("ropeActive", true);
		} else {
			animator.SetBool("ropeActive", false);
		}
	}

}