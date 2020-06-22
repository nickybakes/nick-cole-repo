using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NeonHeightsDataHandler : NetworkBehaviour
{
    //Cole Lashley & Nick Baker

    //Stores synced data over the network. Each client has their own instance of the Data Handler
    // but the syncvars are synced by the server so that they are always the same
    // I'm not sure if this is the best way to do this, or if it is safe from data race conditions
    // or that it is bug free, but it is a way I have found that is scalable and easy to implement


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

    [SyncVar] public int numberOfPlayers;
    public SyncListBool playersAdded = new SyncListBool();
    public SyncListInt playerConnectionIDs = new SyncListInt();
    public SyncListInt playerTeams = new SyncListInt();
    public SyncListInt selectedCharacters = new SyncListInt();

    // Start is called before the first frame update
    void Start()
    {
        playerConnectionIDs.Callback += OnPlayerConnectionIDsUpdated; //SyncList.Callback is called the variable is changed
        //on the server
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

}
