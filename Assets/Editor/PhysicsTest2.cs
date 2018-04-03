using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class PhysicsTest2 {

    [Test]
    public void PhysicsTest2SimplePasses() {
        // Use the Assert class to test conditions.
    }

    // A UnityTest behaves like a coroutine in PlayMode
    // and allows you to yield null to skip a frame in EditMode
    [UnityTest]
    public IEnumerator _Test_setPlayer() {
        GameObject player = new GameObject();
        player.AddComponent<SmoothCamera>();
        player.AddComponent<PlayerMovement>();
        SmoothCamera playerScript = player.GetComponent<SmoothCamera>();
        PlayerMovement playerPosition = player.GetComponent<PlayerMovement>();




        playerScript.setPlayer(player);
        Assert.AreEqual(player, playerScript.getPlayer());


        yield return null;
    }


    [UnityTest]
    public IEnumerator _BlackBox_Test_Camera_Movements()
    {




        yield return null;


    }






}
