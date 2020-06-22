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
    // Start is called before the first frame update
    public override void Start()
    {
        print("dataHandler: " + dataHandler);
        base.Start(); // this starts the server on the NetworkManager base class
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn); // instantiates the lobby client 2 prefab
        // in the future we can replace this with custom code that does the same as this but the way we want it
        // if necessary
        print("connectionID's Updated on server");
        AddPlayerData(conn.connectionId);
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


}