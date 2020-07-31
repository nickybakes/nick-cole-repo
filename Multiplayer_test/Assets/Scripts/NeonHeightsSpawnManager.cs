using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeonHeightsSpawnManager : MonoBehaviour
{
    public const int RED = 0, BLUE = 1;
    public Vector3 defaultSpawn = new Vector3(0, 0, 93);
    private int redIndex, blueIndex;
    private bool initialized = false;

    public class PlayerSpawn {
        public Vector3 position;

        public PlayerSpawn(Vector3 position)
        {
            this.position = position;
        }
    }

    public PlayerSpawn[] championSpawns, underdogSpawns;

    // Start is called before the first frame update
    void Start()
    {
        InitializeSpawns();
    }

    private void InitializeSpawns()
    {
        if (!initialized)
        {
            championSpawns = new PlayerSpawn[] { null, null, null, null };
            underdogSpawns = new PlayerSpawn[] { null, null, null, null };
            redIndex = 0;
            blueIndex = 0;
            initialized = true;
        }
    }

    public void RegisterSpawn(NeonHeightsPlayerSpawn spawn)
    {
        InitializeSpawns();
        PlayerSpawn curPlayer = new PlayerSpawn(spawn.transform.position);

        if (spawn.team == TeamJoined.Champions)
            AddSpawn(championSpawns,curPlayer);
        else if (spawn.team == TeamJoined.Underdogs)
            AddSpawn(underdogSpawns, curPlayer);
    }

    public void AddSpawn(PlayerSpawn[] curTeam, PlayerSpawn curSpawn)
    {
        print("SpawnManager: " + curTeam);
        for(int i = 0; i < curTeam.Length; i++)
        {
            if(curTeam[i] != null)
            {
                curTeam[i] = curSpawn;
                break;
            }
        }
    }

    public Vector3 GetNextSpawn(TeamJoined curTeam)
    {
        Vector3 toReturn = defaultSpawn;
        PlayerSpawn[] curTeamSpawns = championSpawns;
        if (curTeam == TeamJoined.Champions)
        {
            curTeamSpawns = championSpawns;

            int index = redIndex;

            if (curTeamSpawns[index] != null)
                toReturn = curTeamSpawns[index].position;
            if (index < curTeamSpawns.Length - 1 && curTeamSpawns[index + 1] != null)
                redIndex++;
        }
        else if(curTeam == TeamJoined.Underdogs)
        {
            curTeamSpawns = underdogSpawns;
            int index = blueIndex;

            if (curTeamSpawns[index] != null)
                toReturn = curTeamSpawns[index].position;
            if (index < curTeamSpawns.Length - 1 && curTeamSpawns[index + 1] != null)
                blueIndex++;
        }

        return toReturn;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
