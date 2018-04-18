using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformSync : MonoBehaviour
{




    void OnCollisionStay2D(Collision2D coll)
    {

        if(coll.gameObject.tag == "MovingPlatform" && coll.gameObject.GetComponent<PlatformController>().isMoving)
        {
            transform.SetParent(coll.gameObject.transform,true);
        }
        else
        {
            transform.SetParent(null, true);

        }
        
    }

    
    void OnCollisionExit2D(Collision2D coll)
    {

        if(coll.gameObject.tag == "MovingPlatform")
        {
            transform.parent = null;
        }

    }
    
}
