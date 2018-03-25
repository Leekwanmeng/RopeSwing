using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeController : MonoBehaviour {

	/*Public Fields*/
	public LineRenderer lineRenderer;
	public bool ropeActive;

	/*Private Fields*/
	private SpringJoint2D rope;

	void Start() {
		lineRenderer = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			if (!ropeActive) {
				ShootRope();
			} else {
				DestroyRope();
			}
		}
	}

	// Update per physics frame
	void FixedUpdate() {
	}

	// Last update
	void LateUpdate() {
		RenderRope();
	}


	/*
	* Updates after all update functions called
	* Adds LineRenderer to existing rope (from player to anchor)
	*/
	void RenderRope() {
		if (rope != null) {
			lineRenderer.enabled = true;
			lineRenderer.positionCount = 2;
			lineRenderer.SetPosition(0, gameObject.transform.position);
			lineRenderer.SetPosition(1, rope.connectedAnchor);
		} else {
			lineRenderer.enabled = false;
		}
	}


	/*
	* Raycasts to clicked position if it collides with a wall
	* Adds new rope if successful while deleting previous rope
	*/
	void ShootRope() {
		Vector2 mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Vector2 position = gameObject.transform.position;
		Vector2 direction = mousePosition - position;

		RaycastHit2D hit = Physics2D.Raycast (position, direction, 
			Mathf.Infinity, 1 << LayerMask.NameToLayer("Wall"));
		// Debug.DrawLine(position, hit.point, Color.cyan, 100f);
		// print(hit.point);

		if (hit.collider != null) {
			print("fired");
			rope = gameObject.AddComponent<SpringJoint2D>();
			rope.enableCollision = false;
			rope.frequency = 0.5f;
			rope.connectedAnchor = hit.point;
			rope.enabled = true;
			ropeActive = true;
		}
	}


	/*
	* Destroys rope if exists
	*/
	void DestroyRope() {
		GameObject.DestroyImmediate(rope);
		ropeActive = false;
	}


}
