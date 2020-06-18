using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.InputSystem;
//Nick Baker

/// <summary>
/// This 
/// </summary>
public class NeonHeightsLobbyManager : NetworkBehaviour
{
    public enum TeamJoined
    {
        Red,
        Blue,
        Unassigned
    }

    public enum SelectedCharacter
    {
        Unassigned = -2,
        Random = -1,
        Poppy,
        Suit,
        Copper,
        Digit,
        Expo,
        Creed,
        Groove,
        Electra
    }

    public GameObject PlayerLobbyCursorPrefab;
    private const int MAX_PLAYERS = 8;

    [SyncVar] private int numberOfPlayers;
    [SyncVar] private SyncListBool playersAdded;
    [SyncVar] public SyncListInt playerConnectionIDs;
    [SyncVar] public SyncListInt playerTeams;
    [SyncVar] public SyncListInt selectedCharacters;

    // Start is called before the first frame update
    void Start()
    {
        playersAdded = new SyncListBool();
        playerConnectionIDs = new SyncListInt();
        playerTeams = new SyncListInt();
        selectedCharacters = new SyncListInt();

        for (int i = 0; i < MAX_PLAYERS; i++)
        {
            playersAdded.Add(false);
            playerTeams.Add((int)TeamJoined.Unassigned);
            selectedCharacters.Add((int) SelectedCharacter.Unassigned);
        }
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        NetworkServer.RegisterHandler<JoinGameMessage>(OnAddPlayer);
    }

    private int GetNextAvailableSlot()
    {
        for(int i = 0; i < MAX_PLAYERS; i++)
        {
            if (!playersAdded[i])
                return i;
        }
        throw new System.Exception("Game is full!");
    }

    void OnAddPlayer(NetworkConnection conn, JoinGameMessage message)
    {
        int playerIndex = AttemptAddPlayer();
        if (playerIndex != -1)
        {
            GameObject spawnedCursor = Instantiate(PlayerLobbyCursorPrefab);
            spawnedCursor.GetComponent<PlayerLobbyCursor>().InitializePlayerCursor(playerIndex, message.keyboard, message.gamepad);
            //NetworkServer.AddPlayerForConnection(conn, spawnedCursor);
            NetworkServer.Spawn(spawnedCursor);
            spawnedCursor.GetComponent<NetworkIdentity>().AssignClientAuthority(conn);
        }
    }

    public int AttemptAddPlayer()
    {
        try
        {
            int playerIndex = GetNextAvailableSlot();
            playersAdded[playerIndex] = true;
            numberOfPlayers++;
            return playerIndex;
        }
        catch (System.Exception e) {
            Debug.Log(e.Message);
        }
        return -1;
    }





    // Update is called once per frame
    void Update()
    {
        
    }
}

public class JoinGameMessage : MessageBase
{
    public bool keyboardControlled;
    public int gamepadDeviceId;
    public Gamepad gamepad;
    public Keyboard keyboard;
}
