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
        // a good way to go would be to have like the data handler store which connection ID each player belongs to and then the client
        // could easily see which data belongs to it because it can see all the data in the datahandler and it knows its
        // own connection id
        // at the start of the game scene, if each client instantiates its own characters inside this script, they
        // will all have proper network authority no questions asked but a better way might be to instantiate
        //inside of the networkmanager and then assign client authority, depending on what we can get to work

        print("My connection ID is " + connectionId);
        print("Lobby Client Here! My connection ID list looks a bit like: ");
        foreach (int i in dataHandler.playerConnectionIDs)
            print(i);
    }

    // Update is called once per frame
    void Update()
    {
        
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
