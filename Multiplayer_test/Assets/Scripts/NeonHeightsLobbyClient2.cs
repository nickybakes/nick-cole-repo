using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class NeonHeightsLobbyClient2 : NetworkBehaviour
{

    private NeonHeightsDataHandler dataHandler;
    public int connectionId; 
    // Start is called before the first frame update
    void Start()
    {
        dataHandler = GameObject.FindObjectOfType<NeonHeightsDataHandler>();
        if (isLocalPlayer)
        {
            print("Has Authority: " + hasAuthority);
            dataHandler = GameObject.FindObjectOfType<NeonHeightsDataHandler>();
            connectionId = dataHandler.playerConnectionIDs[dataHandler.playerConnectionIDs.Count - 1];
            // This might create an issue if two clients are added at almost the same time, but it also might be fine, I think it depends on what order the Start methods are called by unity
            //compared to when the datahandler adds conn ids
            print("My connection ID is " + connectionId);
            print("Lobby Client Here! My connection ID list looks a bit like: ");
            foreach (int i in dataHandler.playerConnectionIDs)
                print(i);
        }
        // a good way to go would be to have like the data handler store which connection ID each player belongs to and then the client
        // could easily see which data belongs to it because it can see all the data in the datahandler and it knows its
        // own connection id
        // at the start of the game scene, if each client instantiates its own characters inside this script, they
        // will all have proper network authority no questions asked but a better way might be to instantiate
        //inside of the networkmanager and then assign client authority, depending on what we can get to work
    }

    public void serverTest(int connID)
    {
        print("server test");
        print("Given connection id: " + connID);
        print("My connection id: " + connectionId);
    }

    [Command]
    void CmdGrantAuthority(GameObject target)
    {
        target.GetComponent<NetworkIdentity>().AssignClientAuthority(this.gameObject.GetComponent<NetworkIdentity>().connectionToClient);
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            if (Input.GetKeyDown("space"))
            {
                print("space key was pressed");
                CmdAddPlayer(connectionId);
            }
        }
    }

    [Command]
    void CmdAddPlayer(int connId)
    {
        print("Connection ID: " + connId);
        dataHandler.AddPlayer(connId);
    }

    /*
    Psuedocode

    OnSpaceBarPressed / On Join Game Locally pressed(){
        Instantiate(CursorPrefabWithNetworkIdentityandNetworkTransform);
        CmdAddPlayerInformationToDataHandler();
    }

    [Command] //to change any synced variable you must use decorator Command (and function must start with Cmd)
    CmdAddPlayerInformationToDataHandler{
        dataHandler.players.add(new Player(information info);
        dataHandler.numPlayers++;
    }

    (In Cursor Controller + PlayerCharacter and shtuff)
    I beleive (this hasnot been tested) that anything instantiated by this client has the right client authority
    if(isLocalPlayer){
        //do things 
    }


    */
}
