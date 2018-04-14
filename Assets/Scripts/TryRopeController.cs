using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class TryRopeController : NetworkBehaviour {

	/*Public Fields*/
	[SyncVar]
	public bool ropeActive;
	[SyncVar (hook = "HookSetStartPosition")]
	public Vector2 startPosition;
	[SyncVar (hook = "HookSetEndPosition")]
	public Vector2 endPosition;
	[SyncVar (hook = "HookRenderLine")]
	public bool lineRendererEnable;
	[SyncVar]
	public Vector2 ropeJointAnchor;

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
	private float maxRopeDistance = 10f;
	private Vector2 ropeToHandOffset = new Vector2(0f, 0.5f);

	public override void OnStartLocalPlayer() {
         Camera.main.GetComponent<SmoothCamera>().setPlayer(gameObject);
    }

	void Awake() {
		rope = GetComponent<DistanceJoint2D>();
		lineRenderer = GetComponent<LineRenderer>();
		animator = GetComponent<Animator>();
		lineRenderer.enabled = false;
		lineRenderer.positionCount = 2;
		rope.enabled = false;
		playerPosition = transform.position;
		playerMovement = GetComponent<PlayerMovement>();
		
	}
	
	// Update is called once per frame
	void Update () {
		playerPosition = transform.position;
		TouchDetection();
		ifRopeActive();
		SetStartEnd();		
	}

	void LateUpdate() {
		SetEnable();
		animateSwing();
	}




	/*
	* Client method to check for rope
	* Calls corresponding Client -> Server methods to update ropeActive boolean
	*/
	[Client]
	void ifRopeActive() {
		if (!isLocalPlayer) {
			return;
		}
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
	




	[Client]
	void SetStartEnd() {
		if (!isLocalPlayer) {
			return;
		}
		if (rope.enabled) {
			CmdSetStartPosition();
			CmdSetEndPosition(rope.connectedAnchor);
		}
	}

	[Command]
	void CmdSetStartPosition() {
		startPosition = playerPosition + ropeToHandOffset;
	}

	[Command]
	void CmdSetEndPosition(Vector2 connectedAnchor) {
		endPosition = connectedAnchor;
	}

	// HOOK
	void HookSetStartPosition(Vector2 position) {
		// Only update SyncVar if loacl client (it auto-updates in server/host)
		if (!isServer) {
			startPosition = position;
		}
		lineRenderer.SetPosition(0, startPosition);

	}

	// HOOK
	void HookSetEndPosition(Vector2 position) {
		// Only update SyncVar if loacl client (it auto-updates in server/host)
		if (!isServer) {
			endPosition = position;
		}
		if (endPosition != Vector2.zero) {
			lineRenderer.SetPosition(1, endPosition);
		}
	}

	


	[Client]
	void SetEnable() {
		if (!isLocalPlayer) {
			return;
		}
		if (ropeActive) {
			CmdLineRendererEnable();
		} else {
			CmdLineRendererDisable();
		}
	}

	[Command]
	void CmdLineRendererEnable() {
		lineRendererEnable = true;
	}

	[Command]
	void CmdLineRendererDisable() {
		lineRendererEnable = false;
	}

	// HOOK
	// Lets server see itself and client see server
	void HookRenderLine(bool enable) {
		// Only update SyncVar if local client (it auto-updates in server/host)
		if (!isServer) {
			lineRendererEnable = enable;		
		}
		
		if (lineRendererEnable) {
			lineRenderer.enabled = true;
		} else {
			lineRenderer.enabled = false;
		}
	}




	/*
	* Detects Touches on mobile phone
	* Creates rope on touch if no rope exists, else removes current rope
	*/
	void TouchDetection() {
		if (!isLocalPlayer) {
			return;
		}
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

		RaycastHit2D hitWall = Physics2D.Raycast (playerPosition, direction, 
							maxRopeDistance, 1 << LayerMask.NameToLayer("Wall"));

		if (hitWall.collider != null) {

			transform.GetComponent<Rigidbody2D>().AddForce(
				new Vector2(0f, 4f), ForceMode2D.Impulse);
			playerMovement.ropeHook = hitWall.point;
			rope.enableCollision = true;
			rope.distance = Vector2.Distance(playerPosition + ropeToHandOffset, hitWall.point);
			rope.connectedAnchor = hitWall.point;
			rope.enabled = true;
			
		}
	}


	/*
	* Destroys rope if exists
	*/
	void ResetRope() {
		rope.enabled = false;
		rope.distance = 0f;
		rope.connectedAnchor = Vector2.zero;
		playerMovement.ropeHook = Vector2.zero;
	}



	/*
	* Swinging Player Animation
	*/
	void animateSwing() {
		if (!isLocalPlayer) {
			return;
		}
		if (rope.enabled) {
			animator.SetBool("ropeActive", true);
		} else {
			animator.SetBool("ropeActive", false);
		}
	}

}