using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneTest : MonoBehaviour{

    public Button Button1;
    public Button Button2;
    





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

    [UnityTest]
    public IEnumerator _Test_Scene_Navigation()
    {
        /*
        
        GameObject gameObject = new GameObject();
        Canvas canvas = gameObject.GetComponent<Canvas>();

        Button playButton = canvas.GetComponentInChildren<Button>();
    */
        //System.Console.Write("What the freaking fuck");
        SceneManager.LoadScene(0);
        Scene current = SceneManager.GetActiveScene();
        System.Console.Write(current.name);
        Button1 = GameObject.Find("Play").GetComponent<Button>();
        
        Button1.onClick.Invoke();
        
        
        Assert.IsTrue(SceneManager.GetActiveScene().buildIndex == 3);

        yield return null;

    }





}
