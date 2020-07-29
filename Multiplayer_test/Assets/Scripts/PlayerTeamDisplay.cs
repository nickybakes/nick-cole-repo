using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTeamDisplay : MonoBehaviour
{

    public Sprite icon;
    public byte number;
    public string username;

    public Image iconDisplay;
    public Text usernameDisplay;
    public Text numberDisplay;

    public void Init(Sprite playerIcon, string playerName, byte playerNumber)
    {
        this.number = playerNumber;
        this.username = playerName;

        numberDisplay.text = "P" + (playerNumber + 1);
        usernameDisplay.text = playerName;
    }
}
