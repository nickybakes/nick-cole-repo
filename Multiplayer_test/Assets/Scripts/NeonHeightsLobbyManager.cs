using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
//Nick Baker

/// <summary>
/// This 
/// </summary>
public class NeonHeightsLobbyManager : NetworkRoomManager
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

    private const int MAX_PLAYERS = 8;

    private int numberOfPlayers;
    private bool[] playersAdded;
    public int[] playerConnectionIDs;
    public TeamJoined[] playerTeams;
    public SelectedCharacter[] selectedCharacters;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        playersAdded = new bool[MAX_PLAYERS];
        playerConnectionIDs = new int[MAX_PLAYERS];
        playerTeams = new TeamJoined[MAX_PLAYERS];
        selectedCharacters = new SelectedCharacter[MAX_PLAYERS];

        for (int i = 0; i < MAX_PLAYERS; i++)
        {
            selectedCharacters[i] = SelectedCharacter.Unassigned;
        }
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

    public int AttemptAddPlayer(NeonHeightsLobbyClient client)
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
