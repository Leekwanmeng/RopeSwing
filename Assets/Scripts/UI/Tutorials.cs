using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Tutorials : MonoBehaviour {

    public void Next()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

    public void Back()
    {


        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void BackToMainMenu()
    {

        SceneManager.LoadScene("NetworkMenu");
        GameObject.Find("Create").GetComponent<Button>().onClick
            .RemoveAllListeners();

        GameObject.Find("Create").GetComponent<Button>().
            onClick.AddListener(GameObject.Find("NetworkManager").GetComponent<MatchMakerHelper>().CreateInternetMatch);

        GameObject.Find("Join").GetComponent<Button>().
            onClick.RemoveAllListeners();

        GameObject.Find("Join").GetComponent<Button>().
            onClick.AddListener(GameObject.Find("NetworkManager").GetComponent<MatchMakerHelper>().FindInternetMatch);

    }

    public void goToTutorial2()
    {
        SceneManager.LoadScene("Tutorial2");

    }

    public void goToTutorial4()
    {
        SceneManager.LoadScene("Tutorial4");

    }

    public void goToTutorial5()
    {
        SceneManager.LoadScene("Tutorial2");

    }

    public void goToTutorial6()
    {
        SceneManager.LoadScene("Tutorial2");

    }





}
