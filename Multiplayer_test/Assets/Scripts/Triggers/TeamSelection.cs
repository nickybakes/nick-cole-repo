using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public enum TeamJoined
{
    Champions,
    Underdogs,
    Unassigned
}

public class TeamSelection : MenuSelection
{

    public TeamJoined team;

    public void MouseSelectAction()
    {
        NeonHeightsLobbyClient2 clientHandler = GameObject.FindObjectOfType<NeonHeightsDataHandler>().GetLocalClient();
        PlayerLobbyCursor cursor = clientHandler.GetKeyboardPlayer();
        if(cursor != null)
            SelectAction(cursor);
    }

    public override void SelectAction(PlayerLobbyCursor cursor)
    {
        cursor.SetTeam(team);
    }
}
