using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class TeamSelection : MenuSelection
{
    public enum TeamJoined
    {
        Champions,
        Underdogs,
        Unassigned
    }

    public TeamJoined team;

    public override void SelectAction(PlayerLobbyCursor cursor)
    {
        print("Join team " + team.ToString());
        if (team == TeamJoined.Champions)
        {
        }
        else
        {
        }

    }
}
