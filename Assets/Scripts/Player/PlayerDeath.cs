using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class PlayerDeath : NetworkBehaviour {

	/* Public fields*/
	[SyncVar]
	public bool dead;

	/*
	* Upon entering DeathZone trigger
	* Applies to spikes and falling off the map
	*/
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag("DeathZone")) {
			setDeath();
			checkDeath();
		}
	}

	/*
	* Server method call
	* Checks death and executes client RPC (RPC runs on client)
	*/
	void checkDeath() {
		if (!isServer) {
			return;
		}
		if (dead) {
			RpcRespawn();
		}

	}

	/*
	* Local method call
	*/
	void setDeath() {
		CmdSetDead();
	}

	/*
	* Command method, runs on server to update SyncVar dead
	*/
	[Command]
	void CmdSetDead() {
		dead = true;
	}

	/*
	* Command method, runs on server to update SyncVar dead
	*/
	[Command]
	void CmdSetNotDead() {
		dead = false;
	}

	/*
	* Called on Server, invoked on Clients
	*/
	[ClientRpc]
    void RpcRespawn() {
        if (isLocalPlayer) {
            // move back to zero location
            transform.position = Vector3.zero;
        }
    }


}
