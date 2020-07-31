using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : MenuSelection
{
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

    public SelectedCharacter selectedCharacter;

    public override void SelectAction(PlayerLobbyCursor cursor)
    {


    }
}
