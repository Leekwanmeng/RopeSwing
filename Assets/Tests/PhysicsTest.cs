using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;


namespace Tests
{

    public class NewEditModeTest
    {


        // A UnityTest behaves like a coroutine in PlayMode
        // and allows you to yield null to skip a frame in EditMode
        [UnityTest]
        public IEnumerator Moves_Along_X_Axis_For_Horizontal_Input()
        {
            // Use the Assert class to test conditions.
            // yield to skip a frame
            var player = new GameObject().AddComponent<PlayerMovement>();
            yield return null;

            Assert.AreEqual(1, player.transform.position.x, 0.1f);


        }
    }







}
