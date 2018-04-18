using System.Collections;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

namespace Tests
{

    public class SpawnerTests
    {
        [UnityTest]
        public IEnumerator _Instantiates_GameObject_From_Prefab()
        {
            var playerPrefab = Resources.Load("tests/player");
            var playerSpawner = new GameObject().AddComponent<PlayerMovement>();
            playerSpawner.Construct();
            yield return null;
            var spawnedPlayer = GameObject.FindWithTag("Player");
            var prefabOfTheSpawnedPlayer = PrefabUtility.GetPrefabParent(spawnedPlayer);


            Assert.AreEqual(playerPrefab, prefabOfTheSpawnedPlayer);


        }


        [UnityTest]
        public IEnumerator _Instantiates_GameObject_At_Position()
        {
            var playerPrefab = Resources.Load("tests/player");
            var playerSpawner = new GameObject().AddComponent<PlayerMovement>();
            playerSpawner.Construct();
            var spawnedPlayer = GameObject.FindWithTag("Player");
            var expectedPosition = new Vector3(-3, 0, 0);
            Assert.AreEqual(expectedPosition, spawnedPlayer.transform.position);



            yield return null;
        }



        //to discard changes done to project, only needed in play mode
        /*
        [TearDown]
        public void AfterEveryTest()
        {
            foreach (var gameObject in GameObject.FindGameObjectsWithTag("Player"))
                Object.Destroy(gameObject);
            foreach (var gameObject in Object.FindObjectOfType<PlayerMovement>())
                Object.Destroy((UnityEngine.Object)gameObject);


        }
        */


        




    }




}
