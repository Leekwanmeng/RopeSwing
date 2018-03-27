using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCamera : MonoBehaviour {

	/*Public Fields*/
	public float interpVelocity;
	public float minDistance;
	public float followDistance;
	public GameObject player;
	public Vector3 offset;

	Vector3 targetPos;

	// Use this for initialization
	void Start () {
		targetPos = transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (player) {
			Vector3 posNoZ = transform.position;
			posNoZ.z = player.transform.position.z;

			Vector3 targetDirection = (player.transform.position - posNoZ);

			interpVelocity = targetDirection.magnitude * 5f;

			targetPos = transform.position + (targetDirection.normalized * interpVelocity * Time.deltaTime); 

			transform.position = Vector3.Lerp( transform.position, targetPos + offset, 0.25f);

		}
	}

	public void setPlayer(GameObject player) {
		this.player = player;
	}
}
