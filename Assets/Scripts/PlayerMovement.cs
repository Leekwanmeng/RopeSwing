using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	/*Public Fields*/
	public float forceToAdd = 20;
	public float tiltThreshold = 0.4f;

	void Start () {
		//gives it force
		GetComponent<Rigidbody2D>().velocity = Vector2.up * 5;
	}


	void Update () {
		tiltForce();
	}

	void tiltForce() {
		if (Input.acceleration.x > tiltThreshold) {
			GetComponent<Rigidbody2D>().AddForce(Vector2.right*forceToAdd);
		}
		else if (Input.acceleration.x < -tiltThreshold) { 
			GetComponent<Rigidbody2D>().AddForce(Vector2.left*forceToAdd);
		}
	}
}
