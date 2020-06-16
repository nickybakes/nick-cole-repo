using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
//Nick Baker

/// <summary>
/// This stores the players joined from a client
/// </summary>
public class NeonHeightsLobbyClient : NetworkRoomPlayer
{
    public GameObject PlayerLobbyCursorPrefab;

    //this array and list keeps track of what controllers (including keyboard) have been used on this client in this game session
    private char[] controllers = { '1', '2', '3', '4', '5', '6', '7', '8', 'K' };
    private List<char> addedControllers;

    //this list lets us keep track of all players in the lobby from this client
    public List<GameObject> players;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        ClientScene.RegisterPrefab(PlayerLobbyCursorPrefab);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //ClientScene.AddPlayer(ClientScene.readyConnection);
            ////ClientScene.AddPlayer(connectionToClient);
            GameObject spawnedCursor = Instantiate(PlayerLobbyCursorPrefab);
            spawnedCursor.GetComponent<PlayerLobbyCursor>().InitializePlayerCursor(2);
            Debug.Log(connectionToServer);
            NetworkServer.AddPlayerForConnection(connectionToServer, spawnedCursor);
        }
    }
}
