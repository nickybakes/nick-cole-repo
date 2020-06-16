using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class ClientLobbyManager : NetworkBehaviour
{
    private Canvas LobbyUICanvas;
    public GameObject PlayerLobbyCursorPrefab;

    // Start is called before the first frame update
    void Start()
    {
        LobbyUICanvas = GameObject.FindObjectOfType<Canvas>();
        ClientScene.RegisterPrefab(PlayerLobbyCursorPrefab);
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject spawnedCursor = Instantiate(PlayerLobbyCursorPrefab, LobbyUICanvas.transform);
            NetworkServer.Spawn(spawnedCursor);
        }
    }
}
