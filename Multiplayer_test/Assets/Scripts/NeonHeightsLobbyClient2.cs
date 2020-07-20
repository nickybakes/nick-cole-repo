using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class NeonHeightsLobbyClient2 : NetworkBehaviour
{
    public const int GAMEPAD = 1, KEYBOARD = 0;
    private NeonHeightsDataHandler dataHandler;
    public int connectionId;
    private int numGamePads, numKeyboards;
    public GameObject PlayerCursor;
    public InputAction anyButtonPressedGamePad, anyButtonPressedKeyboard;
    // Start is called before the first frame update
    void Start()
    {
        numGamePads = 0;
        numKeyboards = 0;
        dataHandler = GameObject.FindObjectOfType<NeonHeightsDataHandler>();
        if (!isLocalPlayer)
            return;

        if (dataHandler.playerConnectionIDs.Count > 0)
            connectionId = dataHandler.playerConnectionIDs[dataHandler.playerConnectionIDs.Count - 1];
        else
            connectionId = 0;

        dataHandler.setConnectionId(connectionId);
        // This might create an issue if two clients are added at almost the same time, but it also might be fine, I think it depends on what order the Start methods are called by unity
        //compared to when the datahandler adds conn ids
        print("My connection ID is " + connectionId);

        // a good way to go would be to have like the data handler store which connection ID each player belongs to and then the client
        // could easily see which data belongs to it because it can see all the data in the datahandler and it knows its
        // own connection id
        // at the start of the game scene, if each client instantiates its own characters inside this script, they
        // will all have proper network authority no questions asked but a better way might be to instantiate
        //inside of the networkmanager and then assign client authority, depending on what we can get to work

        anyButtonPressedGamePad = new InputAction(binding: "/<Gamepad>/<button>");
        anyButtonPressedGamePad.performed += ctx => OnPlayerJoined(1);
        anyButtonPressedGamePad.Enable();

        anyButtonPressedKeyboard = new InputAction(binding: "/<Keyboard>/<button>");
        anyButtonPressedKeyboard.performed += ctx => OnPlayerJoined(0);
        anyButtonPressedKeyboard.Enable();

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

    public void OnPlayerJoined(int device)
    {
        if (!Application.isFocused)
            return;

        bool addPlayer = false;
        if (device == GAMEPAD && numGamePads < Gamepad.all.Count)
        {
            numGamePads++;
            addPlayer = true;
        }
        else if (device == KEYBOARD && numKeyboards == 0)
        {
            numKeyboards++;
            addPlayer = true;
        }

        if (addPlayer)
            CmdAddPlayer(connectionId, this.gameObject, dataHandler.numberOfPlayers);
    }

    [Command]
    void CmdAddPlayer(int connId, GameObject owner, int playerNum)
    {
        print("Connection ID: " + connId);
        if (dataHandler.AddPlayer(connId))
        {
            ClientScene.RegisterPrefab(PlayerCursor);
            GameObject curCursor = Instantiate(PlayerCursor, new Vector3(0, 0, 50), Quaternion.identity);
            curCursor.GetComponent<PlayerLobbyCursor>().InitializePlayerCursor(playerNum, true, 0);
            NetworkServer.Spawn(curCursor, owner);

        }
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
