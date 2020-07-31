using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.SceneManagement;

public class NeonHeightsLobbyClient2 : NetworkBehaviour
{
    public const int GAMEPAD = 1, KEYBOARD = 0;
    private NeonHeightsDataHandler dataHandler;
    public int connectionId;
    private int numGamePads, numKeyboards;
    public GameObject PlayerCursor;
    public GameObject Player;
    public InputAction anyButtonPressedGamePad, anyButtonPressedKeyboard;
    private List<PlayerLobbyCursor> playerCursors;
    private List<NeonHeightsPlayer> players;
    private Dictionary<int, InputDevice> deviceList;
    private bool gameStarted;
    public bool playersCanMove;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        playerCursors = new List<PlayerLobbyCursor>();
        players = new List<NeonHeightsPlayer>();
        numGamePads = 0;
        numKeyboards = 0;
        dataHandler = GameObject.FindObjectOfType<NeonHeightsDataHandler>();
        SceneManager.activeSceneChanged += OnSceneChanged;
        gameStarted = false;
        playersCanMove = false;
        if (!isLocalPlayer)
            return;

        deviceList = new Dictionary<int, InputDevice>();

        if (dataHandler.playerConnectionIDs.Count > 0)
            connectionId = dataHandler.playerConnectionIDs[dataHandler.playerConnectionIDs.Count - 1];
        else
            connectionId = 0;

        dataHandler.setConnectionId(connectionId);
        dataHandler.SetLocalClient(this);
        // This might create an issue if two clients are added at almost the same time, but it also might be fine, I think it depends on what order the Start methods are called by unity
        //compared to when the datahandler adds conn ids
        print("My connection ID is " + connectionId);

        // a good way to go would be to have like the data handler store which connection ID each player belongs to and then the client
        // could easily see which data belongs to it because it can see all the data in the datahandler and it knows its
        // own connection id
        // at the start of the game scene, if each client instantiates its own characters inside this script, they
        // will all have proper network authority no questions asked but a better way might be to instantiate
        //inside of the networkmanager and then assign client authority, depending on what we can get to work

        anyButtonPressedGamePad = new InputAction(binding: "/<Gamepad>/buttonSouth");
        anyButtonPressedGamePad.performed += ctx => OnPlayerJoined(1);
        anyButtonPressedGamePad.Enable();

        anyButtonPressedKeyboard = new InputAction(binding: "/<Keyboard>/Enter");
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
        bool keyBoardControlled = false;
        if (device == GAMEPAD && numGamePads < Gamepad.all.Count && !isInDevices(Gamepad.current))
        {
            numGamePads++;
            addPlayer = true;
        }
        else if (device == KEYBOARD && numKeyboards == 0 && !isInDevices(Keyboard.current))
        {
            print("adding keyboard");
            numKeyboards++;
            keyBoardControlled = true;
            addPlayer = true;
        }

        if (addPlayer)
            CmdAddPlayer(connectionId, this.gameObject, keyBoardControlled);
    }

    bool isInDevices(InputDevice newDevice)
    {
        toReturn = false;
        foreach(InputDevice device in deviceList.Values)
        {
            if(device.deviceId == newDevice.deviceId)
            {
                toReturn = true;
                break;
            }
        }
        return toReturn;
    }

    [Command]
    void CmdAddPlayer(int connId, GameObject owner, bool keyBoardControlled)
    {
        print("Connection ID: " + connId);
        ClientScene.RegisterPrefab(PlayerCursor);
        GameObject curCursor = Instantiate(PlayerCursor, new Vector3(0, 0, 50), Quaternion.identity);
        int pNum = dataHandler.AddPlayer(connId, curCursor);
        if (pNum != -1)
        {
            curCursor.GetComponent<PlayerLobbyCursor>().InitializePlayerCursor(pNum, keyBoardControlled);
            NetworkServer.Spawn(curCursor, owner);
            dataHandler.AddCursorObject(curCursor, pNum);
        }
        else
        {
            Destroy(curCursor);
        }
    }

    [Command]
    void CmdAddPlayerInGame(GameObject owner, int pNum)
    {
        print("CmdAddPlayerInGameCalled");
        ClientScene.RegisterPrefab(Player);
        GameObject curPlayer = Instantiate(Player, dataHandler.GetPlayerSpawn(pNum), Quaternion.identity);
        curPlayer.GetComponent<NeonHeightsPlayer>().InitializePlayer(pNum);
        NetworkServer.Spawn(curPlayer, owner);
        dataHandler.AddPlayerObject(curPlayer, pNum);
    }

    [Command]
    void CmdRemovePlayer(int pNum)
    {
        dataHandler.RemovePlayer(pNum);
    }

    public void AddCursor(PlayerLobbyCursor cursor)
    {
        print("AddCursor called");
        playerCursors.Add(cursor);
        foreach(PlayerLobbyCursor curCursor in playerCursors)
        {
            curCursor.PairToDevice();
        }
        GetDeviceList();
    }

    public void AddPlayer(NeonHeightsPlayer player)
    {
        players.Add(player);
        foreach(NeonHeightsPlayer curPlayer in players)
        {
            InputDevice curDevice = null;
            if (deviceList.TryGetValue(curPlayer.GetPlayerNum(),out curDevice))
            {
                curPlayer.PairToDevice(curDevice);
            }
            else
                print("Something went wrong in AddPlayer");
        }
    }

    public void PlayerLeave(int pNum, bool controlledByKeyboard)
    {
        print("Player Leave Called");
        foreach (PlayerLobbyCursor cursor in playerCursors)
        {
            if (cursor.GetPlayerNum() == pNum)
            {
                playerCursors.Remove(cursor);
                break;
            }
        }
        CmdRemovePlayer(pNum);
        if (controlledByKeyboard)
        {
            print("removing a keyboard");
            numKeyboards--;
        }
        else
            numGamePads--;
        GetDeviceList();
    }

    public void GetDeviceList()
    {
        deviceList.Clear();
        foreach(PlayerLobbyCursor cursor in playerCursors)
        {
            deviceList.Add(cursor.GetPlayerNum(), cursor.GetDevice());
        }
    }

    public void OnSceneChanged(Scene current, Scene next)
    {
        if (!isLocalPlayer)
            return;

        print("Scene changed spotted in lobbyClient");

        print(dataHandler.gameStarted);

        gameStarted = true;
        AddAllCharacters();
    }

    public void AddAllCharacters()
    {
        print("AddAllCharacters called: " + deviceList.Count);
        foreach(KeyValuePair<int, InputDevice> player in deviceList)
        {
            print("Player: " + player);
            CmdAddPlayerInGame(this.gameObject, player.Key);
        }
    }

    public void PrepareToStartGame()
    {
        print("prepareToStartGame called");
        playerCursors = null;
        CmdPrepareToStartGame();
    }

    public void SetPlayersCanMove()
    {
        if (playersCanMove)
            return;
        foreach(NeonHeightsPlayer curPlayer in players)
        {
            curPlayer.canMove = true;
        }
        playersCanMove = true;
    }

    [Command]
    public void CmdPrepareToStartGame()
    {
        print("CmdPrepareToStartGame");
        dataHandler.PrepareToStartGame();
    }

    public void PlayerUltimate(int pNum)
    {
        CmdPlayerUltimate(pNum);
    }

    [Command]
    public void CmdPlayerUltimate(int pNum)
    {
        dataHandler.PlayerUltimate(pNum);
    }
}
