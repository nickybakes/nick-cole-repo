using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public class CharacterSelection : MenuSelection
{
    public SelectedCharacter selectedCharacter;

    public void MouseSelectAction()
    {
        NeonHeightsLobbyClient2 clientHandler = GameObject.FindObjectOfType<NeonHeightsDataHandler>().GetLocalClient();
        PlayerLobbyCursor cursor = clientHandler.GetKeyboardPlayer();
        if(cursor != null)
            SelectAction(cursor);
    }

    public override void SelectAction(PlayerLobbyCursor cursor)
    {
        cursor.SetCharacter(selectedCharacter);

    }
}
