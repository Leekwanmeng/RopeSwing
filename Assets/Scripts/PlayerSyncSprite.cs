using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class PlayerSyncSprite : NetworkBehaviour {

	[SyncVar (hook = "FacingCallback")]
    public bool netFacingRight = true;
 
 	[Command]
    public void CmdFlipSprite(bool facing) {
        netFacingRight = facing;
        flip();        
    }
 
    void FacingCallback(bool facing) {
        netFacingRight = facing;
        flip();
    }

    void flip() {
    	Vector3 SpriteScale = transform.localScale;
        if (netFacingRight) {
            SpriteScale.x = 1;
            transform.localScale = SpriteScale;
        } else {
            SpriteScale.x = -1;
            transform.localScale = SpriteScale;
        }
    }
}