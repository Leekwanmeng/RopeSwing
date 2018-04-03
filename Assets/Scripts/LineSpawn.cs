using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class LineSpawn : NetworkBehaviour {

	// [SyncVar (hook = "syncVarSetStart")]
	public Vector2 startPosition;
	// [SyncVar (hook = "syncVarSetStart")]
	public Vector2 endPosition;

	private LineRenderer lineRenderer;


	// Use this for initialization
	void Start () {
		lineRenderer = this.GetComponent<LineRenderer>();
		lineRenderer.positionCount = 2;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/*
	* Enables LineRenderer
	*/
	[ClientRpc]
	void RpcRenderRope() {	
		lineRenderer.SetPosition(0, startPosition);
		lineRenderer.SetPosition(1, endPosition);
		lineRenderer.enabled = true;
	}

	/*
	* Disables LineRenderer
	*/
	[ClientRpc]
	void RpcUnrenderRope() {
		lineRenderer.enabled = false;
	}

	public void RenderRope() {
		if (isServer) {
			RpcRenderRope();
		}
	}

	public void UnrenderRope() {
		if (isServer) {
			RpcUnrenderRope();
		}
	}

	/*
	* Public method to set start anchor (Player's position) of line
	*
	* @param Position to set as start
	*/
	public void setStartPosition(Vector2 position) {
		startPosition = position;
	}

	/*
	* Public method to set end anchor (Collision of Raycast) of line
	*
	* @param Position to set as end
	*/
	public void setEndPosition(Vector2 position) {
		endPosition = position;
	}

	/*
	* SyncVar method to set start anchor (Player's position) of line
	*
	* @param Position to set as start
	*/
	// void syncVarSetStart(Vector2 position) {
	// 	lineRenderer.SetPosition(0, position);
	// }

	/*
	* SyncVar method to set end anchor (Collision of Raycast) of line
	*
	* @param Position to set as end
	*/
	// void syncVarSetEnd(Vector2 position) {
	// 	lineRenderer.SetPosition(1, position);
	// }
}
