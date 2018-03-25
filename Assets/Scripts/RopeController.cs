using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeController : MonoBehaviour {

	/*Public Fields*/
	public GameObject ropeShooter;
	public LineRenderer lineRenderer;

	/*Private Fields*/
	private SpringJoint2D rope;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			ShootRope();
		}
	}

	void FixedUpdate() {
	}

	/*
	* Updates after all update functions called
	* Adds LineRenderer to existing rope (from player to anchor)
	*/
	void LateUpdate() {
		if (rope != null) {
			lineRenderer.enabled = true;
			lineRenderer.positionCount = 2;
			lineRenderer.SetPosition(0, ropeShooter.transform.position);
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
		Vector2 position = ropeShooter.transform.position;
		Vector2 direction = mousePosition - position;

		RaycastHit2D hit = Physics2D.Raycast (position, direction, 
			Mathf.Infinity, 1 << LayerMask.NameToLayer("Wall"));
		// Debug.DrawLine(position, hit.point, Color.cyan, 100f);
		// print(hit.point);

		if (hit.collider != null) {
			print("fired");
			SpringJoint2D newRope = ropeShooter.AddComponent<SpringJoint2D>();
			newRope.enableCollision = false;
			newRope.frequency = 0.5f;
			newRope.connectedAnchor = hit.point;
			newRope.enabled = true;

			GameObject.DestroyImmediate(rope);
			rope = newRope;
		}




	}
}
