using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.InputSystem;
//Nick Baker & Cole Lashley

/// <summary>
///  Message of the Day: 
///  I bust therefore I brown - Buster Brown
/// </summary>
public class NeonHeightsLobbyManager : NetworkManager
{

    //Side note: Offline lobby is just online lobby where IP = "localhost"


    // Issue: Server crashes when hosted on built version and client in unity editor attempts to join but it works
    // the other way around. Needs to be tested with a built client and a built server to confirm that the issue
    // is the unity editor client

    public NeonHeightsDataHandler dataHandler;
    private int numPlayersSpawned;
    // Start is called before the first frame update
    public override void Start()
    {
        try
        {
            dataHandler = GameObject.Find("NetworkDataHandler").GetComponent(typeof(NeonHeightsDataHandler)) as NeonHeightsDataHandler;
            dataHandler.SetNetworkManager(this);
        } catch (Exception e)
        {
            dataHandler = null;
        }
        print("dataHandler: " + dataHandler);
        base.Start(); // this starts the server on the NetworkManager base class
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        if (dataHandler == null)
        {
            print("looking for datahandler");
            dataHandler = GameObject.Find("NetworkDataHandler").GetComponent(typeof(NeonHeightsDataHandler)) as NeonHeightsDataHandler;
            dataHandler.SetNetworkManager(this);
        }

        Transform startPos = base.GetStartPosition();
        GameObject player = startPos != null
            ? Instantiate(playerPrefab, startPos.position, startPos.rotation)
            : Instantiate(playerPrefab);

        NetworkServer.AddPlayerForConnection(conn, player);
        player.GetComponent<NeonHeightsLobbyClient2>().connectionId = conn.connectionId;
        player.GetComponent<NeonHeightsLobbyClient2>().serverTest(conn.connectionId);

        print("connectionID's Updated on server");
        AddPlayerData(conn.connectionId);
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        RemovePlayerData(conn.connectionId);
    }

    public override void OnStopServer()
    {
        dataHandler.resetData();
    }

    void RemovePlayerData(int connID)
    {
        dataHandler.RemoveConnection(connID);
    }

    void AddPlayerData(int connID)
    {
        dataHandler.playerConnectionIDs.Add(connID);
        // tbh I'm not sure what reference this datahandler refers to. 
        // All I know for sure is that the network manager is run on the server,
        // And if any instance of the dataHandler has its variables changed then all of them will sync
        // I think that this is a separate instance of dataHandler that runs on the server
        // either that or it's the first one that is created by the first client, not sure


        // add whatever data is needed 
    }

    public void StartGame()
    {
        print("Network Manager StartGame called");
        string nextScene = dataHandler.nextLevel;

        //for strikeout, nextLevel could equal something like
        //"strikeout;map1,map2,map3" and this function could parse that string, recognizing
        //that strikeout is the mode and that every map following, separated by a comma is a map 
        //that can't be chosen
        // then the server could choose a map excluding those
        //with a special method or just by calling ChooseRandomScene until the map isn't one of the struck out ones
        // i'm thinking special method is the way to go bc ChooseRandomScene will be slow as fuck, especially if it needs to 
        //look in the map directory to see which maps are available

        if (nextScene == "random") // code for choosing random scene goes here
            nextScene = "TestGameScene"; // ChooseRandomScene();
    
        //try
        ServerChangeScene(nextScene);
        //catch(Exception e){
        //level name was not found
        //nextScene = ChooseRandomScene();
        //ServerChangeScene(nextScene);
        //}
    }
}