using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
//Nick Baker

/// <summary>
/// This stores the players joined from a client
/// </summary>
public class NeonHeightsLobbyClient : NetworkBehaviour
{
    public GameObject PlayerLobbyCursorPrefab;
    private LobbyInput lobbyInput;
    private NeonHeightsLobbyManager lobbyManager;

    //this array and list keeps track of what controllers (including keyboard) have been used on this client in this game session
    private char[] controllers = { '1', '2', '3', '4', '5', '6', '7', '8', 'K' };
    private List<char> addedControllers;

    //this list lets us keep track of all player cursors in the lobby from this client
    public List<GameObject> playerCursors;

    // Start is called before the first frame update
    void Start()
    {
        lobbyManager = GameObject.FindObjectOfType<NeonHeightsLobbyManager>();
        playerCursors = new List<GameObject>();
        lobbyInput = new LobbyInput();
    }

    private void OnAddKeyboardPlayer()
    {
        if (!isLocalPlayer)
            return;

        JoinGameMessage message = new JoinGameMessage();
        message.keyboardControlled = true;
        connectionToServer.Send(message);
    }

    private void OnAddGamepadPlayer()
    {
        if (!isLocalPlayer)
            return;

        JoinGameMessage message = new JoinGameMessage();
        message.keyboardControlled = false;
        message.gamepadDeviceId = Gamepad.current.deviceId;
        connectionToServer.Send(message);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
            return;

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    //CmdAddPlayerLobbyCursor_Server();
        //    connectionToServer.Send(new JoinGameMessage());
        //}
        //InputUser.PerformPairingWithDevice(new Gamepad());
        //Gamepad g = new Gamepad();
        //int i = g.deviceId;
    }

    [Command]
    private void CmdAddPlayerLobbyCursor_Server()
    {
        //int playerIndex = lobbyManager.AttemptAddPlayer(this);
        //if (playerIndex != -1)
        //{
        //    GameObject spawnedCursor = Instantiate(PlayerLobbyCursorPrefab);
        //    spawnedCursor.GetComponent<PlayerLobbyCursor>().InitializePlayerCursor(playerIndex);
        //    playerCursors.Add(spawnedCursor);
        //    NetworkServer.AddPlayerForConnection(connectionToServer, spawnedCursor);
        //    NetworkServer.Spawn(spawnedCursor);
        //}
        GameObject spawnedCursor = Instantiate(PlayerLobbyCursorPrefab);
        //NetworkServer.AddPlayerForConnection(conn, spawnedCursor);
        NetworkServer.Spawn(spawnedCursor);
        spawnedCursor.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
    }
}
