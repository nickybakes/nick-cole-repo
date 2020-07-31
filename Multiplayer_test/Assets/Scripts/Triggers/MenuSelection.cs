using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MenuSelection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public abstract void SelectAction(PlayerLobbyCursor cursor);

    public virtual void HoverAction(PlayerLobbyCursor cursor)
    {

    }

    public virtual void ExitAction(PlayerLobbyCursor cursor)
    {

    }
}
