using UnityEngine;
using UnityEngine.SceneManagement;



public class Trigger : MonoBehaviour {


    public int triggerNumber;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(triggerNumber == 1)
            {
                GameObject.Find("MovingPlatform0").GetComponent<PlatformController>().triggered = true;
            }
            else if(triggerNumber == 2)
            {
                GameObject.Find("MovingPlatform1").GetComponent<PlatformController>().triggered = true;
            }
            else if(triggerNumber == 3)
            {
                GameObject.Find("FinalMovingPlatform").GetComponent<PlatformController>().triggered = true;
            }
            else if(triggerNumber == 4)
            {
                GameObject.Find("RoomOpenerMovingPlatform").GetComponent<PlatformController>().triggered = true;
            }
            else if(triggerNumber == 5)
            {
                //game win
                SceneManager.LoadScene(5);
            }else if (triggerNumber == 6)
            {
                //game loss
            }



            
            

        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (triggerNumber == 1)
        {
            GameObject.Find("MovingPlatform0").GetComponent<PlatformController>().triggered = false;
        }
        else if (triggerNumber == 2)
        {
            GameObject.Find("MovingPlatform1").GetComponent<PlatformController>().triggered = false;
        }
        else if (triggerNumber == 3)
        {
            GameObject.Find("FinalMovingPlatform").GetComponent<PlatformController>().triggered = false;
        }
        else if (triggerNumber == 4)
        {
            GameObject.Find("RoomOpenerMovingPlatform").GetComponent<PlatformController>().triggered = false;
        }
        
    }

    


}
