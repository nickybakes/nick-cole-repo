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
    private NeonHeightsLobbyManager lobbyManager;

    //this array and list keeps track of what controllers (including keyboard) have been used on this client in this game session
    private char[] controllers = { '1', '2', '3', '4', '5', '6', '7', '8', 'K' };
    private List<char> addedControllers;

    //this list lets us keep track of all player cursors in the lobby from this client
    public List<GameObject> playerCursors;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        ClientScene.RegisterPrefab(PlayerLobbyCursorPrefab);
        lobbyManager = GameObject.FindObjectOfType<NeonHeightsLobbyManager>();
        playerCursors = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
            return;

        foreach(GameObject playerCursor in playerCursors)
        {
            playerCursor.GetComponent<PlayerLobbyCursor>().UpdateInput();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            int playerIndex = lobbyManager.AttemptAddPlayer(this);
            if (playerIndex != -1)
            {
                CmdAddPlayerLobbyCursor_Server(playerIndex);
            }
        }
    }

    [Command]
    private void CmdAddPlayerLobbyCursor_Server(int playerIndex)
    {
        GameObject spawnedCursor = Instantiate(PlayerLobbyCursorPrefab);
        spawnedCursor.GetComponent<PlayerLobbyCursor>().InitializePlayerCursor(playerIndex);
        playerCursors.Add(spawnedCursor);
        NetworkServer.Spawn(spawnedCursor, connectionToClient);
    }
}
