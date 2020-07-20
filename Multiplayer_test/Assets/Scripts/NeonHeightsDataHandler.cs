using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class NeonHeightsDataHandler : NetworkBehaviour
{
    //Cole Lashley & Nick Baker

    //Stores synced data over the network. Each client has their own instance of the Data Handler
    // but the syncvars are synced by the server so that they are always the same
    // I'm not sure if this is the best way to do this, or if it is safe from data race conditions
    // or that it is bug free, but it is a way I have found that is scalable and easy to implement


    public class Player {
        public int playerNum;
        public int connID;
        public SelectedCharacter character;
        public TeamJoined team;

        public Player()
        {
            playerNum = 0;
            connID = -1;
            character = SelectedCharacter.Unassigned;
            team = TeamJoined.Unassigned;
        }

        public Player(int pPlayerNum, int pConnID)
        {
            playerNum = pPlayerNum;
            connID = pConnID;
            character = SelectedCharacter.Unassigned;
            team = TeamJoined.Unassigned;
        }

        public string toString()
        {
            string toReturn = "";
            toReturn += "Player " + playerNum;
            toReturn += "\n\tConnection ID: " + connID;
            toReturn += "\n\tCharacter: Unassigned";
            toReturn += "\n\tTeam: Unassigned";
            toReturn += "\n";
            return toReturn;
        }

    }

    public class SyncListPlayer : SyncList<Player> { }


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

    public const int MAX_PLAYERS = 8;

    public Text UIText;

    public NeonHeightsLobbyClient2 localClient;

    public int connectionId; 
    [SyncVar] public int numberOfPlayers;
    public SyncListPlayer players = new SyncListPlayer();
    public SyncListBool playersAdded = new SyncListBool();
    public SyncListInt playerConnectionIDs = new SyncListInt();

    // Start is called before the first frame update
    void Start()
    {
        localClient = GameObject.FindObjectOfType<NeonHeightsLobbyClient2>();

        connectionId = -1;
        numberOfPlayers = 0;
        playerConnectionIDs.Callback += OnPlayerConnectionIDsUpdated; //SyncList.Callback is called the variable is changed
        //on the server
    }

    public void setConnectionId(int connId)
    {
        connectionId = connId;
    }

    void OnPlayerConnectionIDsUpdated(SyncListInt.Operation op, int index, int oldItem, int newItem) //SyncList.Callback
    {
        //operation is enum that specifies the operation peformed
        //new item and old item are pretty self explanatory but index is the index of the item that was changed
        print("ConnectionID's Updated");
        switch (op)
        {
            case SyncListInt.Operation.OP_ADD:
                print("New Connection! Conn ID: " + newItem);
                break;
            case SyncListInt.Operation.OP_CLEAR:
                break;
            case SyncListInt.Operation.OP_INSERT:
                break;
            case SyncListInt.Operation.OP_REMOVEAT:
                break;
            case SyncListInt.Operation.OP_SET:
                break;
        }
    }

    void Update()
    {
        UpdateUIText();
    }

    void UpdateUIText()
    {
        string textBuilder = "Client Connection ID's: \n";
        foreach (int i in playerConnectionIDs)
            textBuilder += "\t" + i;
        textBuilder += "\nPlayers: \n";
        foreach (Player player in players)
            textBuilder += player.toString();
        textBuilder += "Press space to add a player.";
        UIText.text = textBuilder;
    }

    public bool AddPlayer(int connID)
    {
        bool toReturn = false;
        if (numberOfPlayers < MAX_PLAYERS)
        {
            numberOfPlayers++;
            players.Add(new Player(numberOfPlayers, connID));
            toReturn = true;
        }
        return toReturn;
    }

    public void resetData()
    {
        numberOfPlayers = 0;
        players = new SyncListPlayer();
        playersAdded = new SyncListBool();
        playerConnectionIDs = new SyncListInt();
    }

    public void RemoveConnection(int connID)
    {
        playerConnectionIDs.Remove(connID);
        foreach(Player player in players)
        {
            if (player.connID == connID)
                RemovePlayer(player);
        }
    }

    public void RemovePlayer(Player player)
    {
        players.Remove(player);
        /*numberOfPlayers = 0;
        foreach(Player p in players)
        {
            numberOfPlayers++;
            p.playerNum = numberOfPlayers; 
        }*/
    }

    public List<int> GetPlayerNumsForClient(int connID)
    {
        List<int> toReturn = new List<int>();
        foreach(Player player in players)
        {
            if (player.connID == connID)
                toReturn.Add(player.playerNum);
        }
        return toReturn;
    }

    public void checkForErrors()
    {

    }

}
