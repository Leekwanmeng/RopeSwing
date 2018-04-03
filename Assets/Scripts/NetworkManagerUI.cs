using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class NetworkManagerUI : NetworkManager {

	public void StartupHost()
    {

        System.Console.Write("Entered StartupHost");
        SetPort();
        NetworkManager.singleton.StartHost();


    }


    public void JoinGame()
    {
        SetIPAddress();
        SetPort();
        NetworkManager.singleton.StartClient();

    }


    void SetIPAddress()
    {
        string ipAddress = GameObject.Find("InputFieldIPAddress").
            transform.FindChild("Text").GetComponent<Text>().text;

        NetworkManager.singleton.networkAddress = ipAddress;
    }

    void SetPort()
    {

        NetworkManager.singleton.networkPort = 7777;
    }


    //was a potential fix to the problem with disconnect
    /*
    int _changedScene = 0;
    void Update()
    {
        if (_changedScene == -1)
            return;
        if (_changedScene == 0)
        {
            SetupMenuSceneButtons();
        }
        else if (_changedScene == 1)
        {
            SetupOtherSceneButtons();
        }
        _changedScene = -1;
    }
    void OnLevelWasLoaded(int level)
    {
        _changedScene = level;
    }

        */

    /*
    void OnLevelWasLoaded(int level)
    {
        if(level == 0)
        {
            SetupMenuSceneButtons();


        }else
        {
            SetupOtherSceneButtons();


        }

    }
    */

    void SetupMenuSceneButtons()
    {

        GameObject.Find("ButtonStartHost").GetComponent<Button>().
            onClick.RemoveAllListeners();

        GameObject.Find("ButtonStartHost").GetComponent<Button>().
            onClick.AddListener(StartupHost);

        GameObject.Find("ButtonJoinGame").GetComponent<Button>().
            onClick.RemoveAllListeners();

        GameObject.Find("ButtonJoinGame").GetComponent<Button>().
            onClick.AddListener(JoinGame);

    }


    //Disconnect Button(Optional)
    
    void SetupOtherSceneButtons()
    {
        GameObject.Find("ButtonDisconnect").GetComponent<Button>().
            onClick.RemoveAllListeners();

        GameObject.Find("ButtonDisconnect").GetComponent<Button>().
            onClick.AddListener(NetworkManager.singleton.StopHost);


    }
    

    




}
