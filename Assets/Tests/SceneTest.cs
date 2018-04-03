using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;



public class SceneTest {

	[Test]
	public void SceneTestSimplePasses() {
		// Use the Assert class to test conditions.
	}

    // A UnityTest behaves like a coroutine in PlayMode
    // and allows you to yield null to skip a frame in EditMode
    [UnityTest]
    public IEnumerator _Test_Scene_Loading()
    {


        
        Scene testScene = SceneManager.GetActiveScene();

        var loading = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);

        yield return loading;
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(1));
        System.Console.Write("It sets active scene");

        Assert.IsTrue(SceneManager.GetActiveScene().buildIndex == 1);


        SceneManager.SetActiveScene(testScene);
        yield return SceneManager.UnloadSceneAsync(1);

    }
}
