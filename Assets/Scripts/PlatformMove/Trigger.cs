using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour {

	
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            System.Console.Write("We are triggering the event");
            GameObject.Find("Moving Platform").GetComponent<PlatformController>().triggered = true;

        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            GameObject.Find("Moving Platform").GetComponent<PlatformController>().triggered = false;

        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
    }



}
