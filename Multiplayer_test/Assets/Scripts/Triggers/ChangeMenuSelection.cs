using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMenuSelection : MenuSelection
{
    public enum LobbyMenu
    {
        TeamJoin,
        CharacterSelect
    }

    public LobbyMenu menu;

    public override void SelectAction(PlayerLobbyCursor cursor)
    {
        

    }
}
