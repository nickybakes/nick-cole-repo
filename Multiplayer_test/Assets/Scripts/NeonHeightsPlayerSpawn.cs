using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeonHeightsPlayerSpawn : MonoBehaviour
{
    public NeonHeightsDataHandler.TeamJoined team;
    private NeonHeightsSpawnManager spawnManager; 
    public int spawnNum;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject.GetComponent<SpriteRenderer>());
        spawnManager = GameObject.FindObjectOfType<NeonHeightsSpawnManager>();
        spawnManager.RegisterSpawn(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
