using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class TryRopeController : NetworkBehaviour {

	/*Public Fields*/
	[SyncVar]
	public bool ropeActive;
	[SyncVar (hook = "SetStartPosition")]
	public Vector2 startPosition;
	[SyncVar (hook = "SetEndPosition")]
	public Vector2 endPosition;
	[SyncVar (hook = "RenderLine")]
	public bool lineRendererEnable;

	public DistanceJoint2D rope;
	public LineRenderer lineRenderer;
	public GameObject ropeRendererPrefab;
	

	/*Private Fields*/
	private Animator animator;
	private Vector2 touchPosition;
	private Vector2 playerPosition;
	private PlayerMovement playerMovement;
	private GameObject ropeRendererObject;
	private LineRenderer ropeRenderer;
	private float maxRopeDistance = 15f;
	private Vector2 ropeToHandOffset = new Vector2(0f, 0.4f);

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
		SetStartEnd();
		// if (ropeActive) {
		// 	ropeRenderer.enabled = true;
		// 	ropeRenderer.positionCount = 2;
		// 	ropeRenderer.SetPosition(0, playerPosition);
		// 	ropeRenderer.SetPosition(1, playerPosition);
		// }
		// CmdSpawn();
		CheckRender();
		animateSwing();
		
	}

	// HOOK
	void SetStartPosition(Vector2 position) {
		lineRenderer.SetPosition(0, position);
	}

	// HOOK
	void SetEndPosition(Vector2 position) {
		lineRenderer.SetPosition(1, position);
	}

	// HOOK
	void RenderLine(bool enable) {		
		if (enable) {
			lineRenderer.enabled = true;
		} else {
			lineRenderer.enabled = false;
		}
	}


	// [Command]
	// void CmdSpawn() {
	//     ropeRendererObject = (GameObject) Instantiate(
	//        ropeRendererPrefab, 
	//        (Vector2) transform.position + new Vector2(0f, 0.3f), 
	//        Quaternion.identity);

	    
	// 	ropeRenderer = ropeRendererObject.GetComponent<LineRenderer>();
	// 	ropeRenderer.enabled = true;
	// 	ropeRenderer.positionCount = 2;
	// 	ropeRenderer.SetPosition(0, playerPosition);
	// 	ropeRenderer.SetPosition(1, rope.connectedAnchor);
		

	//     NetworkServer.SpawnWithClientAuthority(ropeRendererObject, connectionToClient);
	// }


	void CheckRender() {
		if (!isServer) {
			return;
		}

		if (ropeActive) {
			RpcRenderRope();
		} else {
			RpcUnrenderRope();
		}
	}

	[ClientRpc]
	void RpcRenderRope() {
		if (isLocalPlayer) {
			lineRenderer.enabled = true;
		}
	}

	[ClientRpc]
	void RpcUnrenderRope() {
		if (isLocalPlayer) {
			lineRenderer.enabled = false;
		}	
	}



	void SetStartEnd() {
		if (ropeActive) {
			CmdSetEndPosition();
			CmdSetStartPosition();
			CmdLineRendererEnable();
		} else {
			CmdLineRendererDisable();
			CmdResetStartPosition();
			CmdResetEndPosition();
		}
	}

	[Command]
	void CmdSetStartPosition() {
		startPosition = playerPosition + ropeToHandOffset;
	}

	[Command]
	void CmdResetStartPosition() {
		startPosition = Vector2.zero;
	}

	[Command]
	void CmdSetEndPosition() {
		endPosition = rope.connectedAnchor;
	}

	[Command]
	void CmdResetEndPosition() {
		endPosition = Vector2.zero;
	}

	[Command]
	void CmdLineRendererEnable() {
		lineRendererEnable = true;
	}

	[Command]
	void CmdLineRendererDisable() {
		lineRendererEnable = false;
	}



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
							maxRopeDistance, 1 << LayerMask.NameToLayer("Wall"));

		if (hit.collider != null) {

			transform.GetComponent<Rigidbody2D>().AddForce(
				new Vector2(0f, 5f), ForceMode2D.Impulse);
			playerMovement.ropeHook = hit.point;
			rope.enableCollision = true;
			rope.distance = Vector2.Distance(playerPosition + ropeToHandOffset, hit.point);
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