using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class PlayerDeath : NetworkBehaviour {

	/* Public fields*/
	[SyncVar]
	public bool dead;

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag("DeathZone")) {
			setDeath();
			checkDeath();
		}
	}

	void checkDeath() {
		if (!isServer) {
			return;
		}
		if (dead) {
			RpcRespawn();
		}

	}

	[Client]
	void setDeath() {
		CmdSetDead();
	}

	[Command]
	void CmdSetDead() {
		dead = true;
	}

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
