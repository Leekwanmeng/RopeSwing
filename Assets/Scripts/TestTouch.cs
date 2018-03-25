using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestTouch : MonoBehaviour {

	public Text m_Text1;
	public Text m_Text2;

    void Update()
    {
        Touch touch = Input.GetTouch(0);

        //Update the Text on the screen depending on current position of the touch each frame
        m_Text1.text = "Touch Position : " + touch.position;
        m_Text2.text = "World cam : " + Camera.main.ScreenToWorldPoint
				(new Vector2(touch.position.x, touch.position.y));
    }
}
