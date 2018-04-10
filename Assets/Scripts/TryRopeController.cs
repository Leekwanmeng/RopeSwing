using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class TryRopeController : NetworkBehaviour {

	/*Public Fields*/
	[SyncVar]
	public bool ropeActive;
	[SyncVar (hook = "OnRopeActve")]
	public Vector2 startPosition;
	[SyncVar]
	public Vector2 endPosition;
	public DistanceJoint2D rope;
	public LineRenderer lineRenderer;
	public PlayerMovement playerMovement;

	/*Private Fields*/
	private Animator animator;
	private Vector2 touchPosition;
	private LineSpawn lineSpawn;

	public override void OnStartLocalPlayer() {
         Camera.main.GetComponent<SmoothCamera>().setPlayer(gameObject);
    }

	void Start() {
		lineRenderer = GetComponent<LineRenderer>();
		animator = GetComponent<Animator>();
		playerMovement = GetComponent<PlayerMovement>();
		rope = GetComponent<DistanceJoint2D>();
		rope.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!isLocalPlayer) {
			return;
		}
		TouchDetection();
		ifRopeActive();
		// CmdRenderRope();
		animateSwing();
	}

	// Update per physics frame
	void FixedUpdate() {
	}

	// Last update
	void LateUpdate() {
		
	}

	[Client]
	void AssignPositions(Vector2 startPos, Vector2 endPos) {
		CmdPositions(startPos, endPos);
	}


	[Command]
	void CmdPositions(Vector2 startPos, Vector2 endPos) {
		startPosition = startPos;
		endPosition = endPos;
	}



	void OnRopeActve(Vector2 startPos) {
		if (!ropeActive) {
			lineRenderer.enabled = false;
		}
		startPosition = startPos;
		lineRenderer.enabled = true;
		lineRenderer.positionCount = 2;
		lineRenderer.SetPosition(0, startPosition);
		lineRenderer.SetPosition(1, rope.connectedAnchor);
	}


	/*
	* Updates after all update functions called
	* Adds LineRenderer to existing rope (from player to anchor)
	*/
	// [Command]
	// void CmdRenderRope() {
	// 	GameObject line = (GameObject)Instantiate(
	// 				linePrefab,
	// 				transform.position,
	// 				transform.rotation);

	// 	lineSpawn = line.GetComponent<LineSpawn>();
	// 	if (rope != null) {
	// 		lineSpawn.setStartPosition(transform.position);
	// 		lineSpawn.setEndPosition(rope.connectedAnchor);
	// 		// lineSpawn.RenderRope();
	// 	} 
	// 	// else {
	// 		// lineSpawn.UnrenderRope();
	// 	// }

	// 	NetworkServer.SpawnWithClientAuthority(line, gameObject);


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
					DestroyRope();
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
		// Vector2 mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Vector2 playerPosition = gameObject.transform.position;
		Vector2 direction = touchPosition - playerPosition;

		RaycastHit2D hit = Physics2D.Raycast (playerPosition, direction, 
							Mathf.Infinity, 1 << LayerMask.NameToLayer("Wall"));

		if (hit.collider != null) {
			rope.enabled = true;
			rope.enableCollision = true;
			rope.distance = hit.distance;
			rope.connectedAnchor = hit.point;
			playerMovement.ropeHook = hit.point;
		}

		AssignPositions(playerPosition, rope.connectedAnchor);
	}


	/*
	* Destroys rope if exists
	*/
	void DestroyRope() {
		rope.enabled = false;
	}


	/*
	* Client method to check for rope
	* Calls corresponding Client -> Server methods to update ropeActive boolean
	*/
	[Client]
	void ifRopeActive() {
		if (rope.enabled == true) {
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
		if (ropeActive) {
			animator.SetBool("ropeActive", true);
		} else {
			animator.SetBool("ropeActive", false);
		}
	}

}
