﻿using System;
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
            toReturn += "\n\tCharacter: " + character;
            toReturn += "\n\tTeam: Unassigned: " + team;
            toReturn += "\n";
            return toReturn;
        }

    }

    public class SyncListPlayer : SyncList<Player> { }

    public const int MAX_PLAYERS = 8;

    public const int EVENT_NONE = 0, EVENT_PLAYER_ULT = 1;
    public const int COUNTDOWN_LENGTH = 5;

    public Text UIText, CountdownText;

    [SyncVar] public int countdownTimer;
    [SyncVar] public int curEvent, lastEvent;
    [SyncVar] public int eventParams;
    float curCountdownTime = 0, eventTimer = -1;

    public NeonHeightsLobbyManager networkManager;
    public NeonHeightsSpawnManager spawnManager;
    public NeonHeightsLobbyClient2 localClient;
    public int connectionId;
    
    public SyncListPlayer players = new SyncListPlayer();
    public SyncListBool playersAdded = new SyncListBool();
    public SyncListInt playerConnectionIDs = new SyncListInt();
    public GameObject[] playerObjects;
    [SyncVar] public int numberOfPlayers;

    [SyncVar] public string nextLevel;
    [SyncVar] public bool gameStarted;

    // Start is called before the first frame update
    void Start()
    {
        countdownTimer = COUNTDOWN_LENGTH;
        curEvent = EVENT_NONE;
        lastEvent = EVENT_NONE;
        eventParams = 0;
        DontDestroyOnLoad(this.gameObject);
        if (isServer)
        {
            playerObjects = new GameObject[] { null, null, null, null, null, null, null, null };
        }
        else
        {
            playerObjects = null;
            networkManager = null;
        }
        spawnManager = null;
        localClient = GameObject.FindObjectOfType<NeonHeightsLobbyClient2>();

        connectionId = -1;
        numberOfPlayers = 0;
        nextLevel = "random";
        gameStarted = false;
        playerConnectionIDs.Callback += OnPlayerConnectionIDsUpdated; //SyncList.Callback is called the variable is changed
        //on the server
    }

    public void SetLocalClient(NeonHeightsLobbyClient2 localClient)
    {
        this.localClient = localClient;
    }

    public void SetNetworkManager(NeonHeightsLobbyManager networkManager)
    {
        print("Setting network Manager");
        this.networkManager = networkManager;
    }

    public NeonHeightsLobbyClient2 GetLocalClient()
    {
        return localClient;
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

    // Event/ Countdown stuff, can be changed/ removed pretty easily

    void Update()
    {
        UpdateUIText();
        if (gameStarted && countdownTimer >= 0)
            Countdown();
        else
        {
            if (CountdownText != null)
                CountdownText.text = "";
        }

        /* Events can be used to do things like server and client animations or basically
         * handle things that need to be handled in real time by both server and client 
         *
         * curEvent is the current event that is going to be run
         *  It is important that when making new events you check for curEvent == EVENT_NONE before setting curEvent
         *  I dont think there will be enough events that this will be a problem, and I think in most cases we don't 
         *  want other events to be called while there is an event happening, but if there is a situation where we do
         *  we can make an event queue and make curEvent eventQueue.pop
         *  
         *  eventParams are the parameters of the event
         *  
         *  eventTimer can be used to time how long the event should be and end it when the timer reaches a certain point
         *  
         *  anything that involves changing of variables or instantiating must have a check for isServer and 
         *  every event must end with 
         *  if(isServer)
         *      EndEvent();
         * 
         */
        switch (curEvent)
        {
            case EVENT_PLAYER_ULT:
                HandlePlayerUlt(eventParams);
                break;
            default:
                if(lastEvent != EVENT_NONE)
                    EventCleanup();
                break;
        }
    }

    void EventCleanup()
    {
        switch (lastEvent)
        {
            case EVENT_PLAYER_ULT:
                CountdownText.text = "";
                break;
        }
        lastEvent = EVENT_NONE;
    }

    void EndEvent()
    {
        lastEvent = curEvent;
        curEvent = EVENT_NONE;
        eventParams = 0;
        eventTimer = -1;
    }

    void HandlePlayerUlt(int pNum)
    {
        if (eventTimer == -1)
            eventTimer = Time.time;

        string ultText = "PLAYER " + pNum + " ULTIMATE!";

        CountdownText.text = ultText;

        if (isServer && ((Time.time - eventTimer) >= 2))
        {
            EndEvent();
        }
    }

    void Countdown()
    {
        if (CountdownText == null)
        {
            try
            {
                CountdownText = GameObject.Find("CountdownText").GetComponent<Text>();
            }
            catch (Exception e)
            {
                return;
            }
        }

        print("Countdown: " + CountdownText);

        print("Countdown Timer: " + countdownTimer);

        if (countdownTimer == COUNTDOWN_LENGTH && curCountdownTime == 0)
            curCountdownTime = Time.time;
        if (countdownTimer > 0)
            CountdownText.text = countdownTimer.ToString();
        else
        {
            CountdownText.text = "Start!";
            if (!localClient.playersCanMove)
                localClient.SetPlayersCanMove();
        }

        print("Cur countdown time: " + curCountdownTime);
        print("Time left: " + (Time.time - curCountdownTime));

        if((Time.time - curCountdownTime) >= 2)
        {
            curCountdownTime = Time.time;

            if (countdownTimer == 0)
            {
                CountdownText.text = "";
                print("Called");
            }

            if (isServer)
                countdownTimer--;
        }
    }

    // Pre Game Lobby

    void UpdateUIText()
    {
        if (UIText != null)
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
    }

    public int AddPlayer(int connID, GameObject playerCursor)
    {
        int toReturn = -1;
        if (numberOfPlayers < MAX_PLAYERS)
        {
            numberOfPlayers++;
            int curNum = 0;
            for(int i = 0; i < playerObjects.Length; i++)
            {
                if(playerObjects[i] == null)
                {
                    curNum = i;
                    break;
                }
            }
            print("curNum+1: " + (curNum + 1));
            players.Add(new Player(curNum+1, connID));
            toReturn = curNum + 1;
        }

        if(toReturn != -1)
            AddCursorObject(playerCursor, toReturn);

        return toReturn;
    }

    public void AddCursorObject(GameObject playerCursor, int pNum)
    {
        playerObjects[pNum - 1] = playerCursor;
        print("Adding to index " + (pNum - 1));
    }

    public void AddPlayerObject(GameObject player, int pNum)
    {
        playerObjects[pNum - 1] = player;
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
        print("Num Players: " + players.Count);
        playerConnectionIDs.Remove(connID);
        for(int i = 0; i < players.Count; i++)
        {
            Player player = players[i];
            print("one player found for removed connection");
            if (player.connID == connID)
            {
                print("PlayerNum: " + player.playerNum);
                RemovePlayer(player);
                i--;
            }
            print("Continuing loop");
        }
        print("Num Players at End: " + players.Count);
    }

    public void RemovePlayer(Player player)
    {
        print("RemovePlayerCalled: " + player);
        players.Remove(player);
        numberOfPlayers--;

        GameObject cursor = playerObjects[player.playerNum-1];

        int curPlayerNum = 0;
        if (cursor.GetComponent<PlayerLobbyCursor>() != null)
            curPlayerNum = cursor.GetComponent<PlayerLobbyCursor>().GetPlayerNum();
        else if (cursor.GetComponent<NeonHeightsPlayer>() != null)
            curPlayerNum = cursor.GetComponent<NeonHeightsPlayer>().GetPlayerNum();
        else
            print("something has gone wrong!!");

        if (cursor != null && curPlayerNum == player.playerNum)
        {
            playerObjects[player.playerNum - 1] = null;
            Destroy(cursor);
        }
        else
            print("Something has gone wrong!!: " + cursor);

    }

    public void RemovePlayer(int pNum)
    {
        Player curPlayer = null;
        foreach(Player player in players)
        {
            if (player.playerNum == pNum)
                curPlayer = player;
        }
        if (curPlayer != null)
            RemovePlayer(curPlayer);
    }

    public void SetCharacter(int pNum, SelectedCharacter toSet)
    {
        Player curPlayer = GetPlayer(pNum);
        if (curPlayer == null)
            return;

        curPlayer.character = toSet;

    }

    public void SetTeam(int pNum, TeamJoined toSet)
    {
        Player curPlayer = GetPlayer(pNum);
        if (curPlayer == null)
            return;

        curPlayer.team = toSet;
    }

    // Game

    public void PrepareToStartGame()
    {
        print("Data Handler PrepareToStartGame called");
        UIText = null;
        gameStarted = true;
        playerObjects = new GameObject[] { null, null, null, null, null, null, null, null };
        networkManager.StartGame();
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
        print("Nothing here yet");
    }

    public Vector3 GetPlayerSpawn(int pNum)
    {
        if (spawnManager == null)
            spawnManager = GameObject.FindObjectOfType<NeonHeightsSpawnManager>();

        Player curPlayer = null;

        foreach (Player player in players) {
            if(player.playerNum == pNum)
            {
                curPlayer = player;
                break;
            }
        }

        Vector3 toReturn = spawnManager.GetNextSpawn(curPlayer.team);
        return toReturn;
    }

    private Player GetPlayer(int pNum)
    {
        Player toReturn = null;
        foreach(Player player in players)
        {
            if(player.playerNum == pNum)
            {
                toReturn = player;
                break;
            }
        }
        return toReturn;
    }

    public void PlayerUltimate(int pNum)
    {
        if(curEvent == EVENT_NONE)
        {
            curEvent = EVENT_PLAYER_ULT;
            eventParams = pNum;
        }
    }

}
