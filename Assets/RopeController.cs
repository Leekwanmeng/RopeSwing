using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeController : MonoBehaviour {

	public GameObject ropeShooter;
	public LineRenderer lineRenderer;

	private SpringJoint2D rope;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			Fire();
		}
	}

	void FixedUpdate() {
	}

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

	void Fire() {
		Vector2 mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Vector2 position = ropeShooter.transform.position;
		Vector2 direction = mousePosition - position;
		print("Mouse position: " + mousePosition);
		print("position: " + position);
		print("Direction: " + direction);


		RaycastHit2D hit = Physics2D.Raycast (position, direction, 
			Mathf.Infinity, 1 << LayerMask.NameToLayer("Wall"));
		Debug.DrawLine(position, hit.point, Color.cyan, 100f);
		print(hit.point);

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
