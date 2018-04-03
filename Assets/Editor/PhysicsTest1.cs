using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System;

namespace Tests
{

    public class PhysicsTest1
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


        [UnityTest]
        public IEnumerator _Test_TiltForce()
        {
            PlayerMovement player = new PlayerMovement();

            //to be fixed


            yield return null;


        }
        

        [UnityTest]
        public IEnumerator _Test_flip()
        {
            GameObject myplayer = new GameObject();
            PlayerMovement p = new PlayerMovement();
            myplayer.AddComponent(typeof(PlayerMovement));

            PlayerMovement playerScript = myplayer.GetComponent<PlayerMovement>();


            playerScript.SetDirection(true);
            playerScript.flipSprite(true);

            Assert.AreEqual(false, playerScript.facingRight);

            yield return null;
        }



        [UnityTest]
        public IEnumerator _Test_checkPlayerDirection1()
        {

            GameObject myplayer = new GameObject();
            PlayerMovement p = new PlayerMovement();
            myplayer.AddComponent(typeof(PlayerMovement));

            PlayerMovement playerScript = myplayer.GetComponent<PlayerMovement>();

            Vector2 testVelocity = new Vector2(0.06f,0);
            playerScript.ConstructCheckDirection(testVelocity,true);
            playerScript.checkPlayerDirection();

            Assert.AreEqual(true, playerScript.facingRight);


            yield return null;


        }

        [UnityTest]
        public IEnumerator _Test_checkPlayerDirection2()
        {
            GameObject myplayer = new GameObject();
            PlayerMovement p = new PlayerMovement();
            myplayer.AddComponent(typeof(PlayerMovement));

            PlayerMovement playerScript = myplayer.GetComponent<PlayerMovement>();

            Vector2 testVelocity = new Vector2(-0.06f, 0);
            playerScript.ConstructCheckDirection(testVelocity, true);
            playerScript.checkPlayerDirection();

            Assert.AreEqual(false, playerScript.facingRight);


            yield return null;

        }


        [UnityTest]
        public IEnumerator _Test_isGrounded()
        {
            //ask kwan meng how I can access the position of the "ground"
            GameObject myplayer = new GameObject();
            PlayerMovement p = new PlayerMovement();
            myplayer.AddComponent(typeof(PlayerMovement));

            PlayerMovement playerScript = myplayer.GetComponent<PlayerMovement>();

            Vector3 testVec = new Vector3(1, 100, 1);

            playerScript.SetPosition(testVec);
            Assert.AreEqual(false, playerScript.isGrounded());
            yield return null;


        }

        





    }







}
