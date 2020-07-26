using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public Canvas displayCanvas;
    public Canvas gameBoard;

    public GameObject hexPrefab;

    private Hex[,] boardData;


    private bool mouseDragging;
    private Vector2 mouseDragStart;
    private Vector2 boardDragStart;


    // Start is called before the first frame update
    void Start()
    {
        SpawnHexes(10, 6);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.mouseScrollDelta.y < 0)
        {
            LeanTween.scale(gameBoard.gameObject, gameBoard.transform.localScale * .8f, .2f);
        }
        else if (Input.mouseScrollDelta.y > 0)
        {
            LeanTween.scale(gameBoard.gameObject, gameBoard.transform.localScale * 1.2f, .2f);
        }
        if (Input.GetMouseButtonDown(2))
        {
            mouseDragging = true;
            mouseDragStart = Input.mousePosition;
            boardDragStart = gameBoard.transform.position;
        }
        if(mouseDragging)
        {
            gameBoard.transform.position = (Vector3) (boardDragStart - mouseDragStart + (Vector2)Input.mousePosition);
        }
        if (Input.GetMouseButtonUp(2))
        {
            mouseDragging = false;
        }
    }

    public void SpawnHexes(int width, int height)
    {
        boardData = new Hex[width, height];

        for(int y = 0; y < boardData.GetLength(1); y++)
        {
            for (int x = 0; x < boardData.GetLength(0); x++)
            {
                GameObject spawnedHex = Instantiate(hexPrefab, gameBoard.transform);
                Hex hex = spawnedHex.GetComponent<Hex>();
                boardData[x, y] = hex;

                //if its an even number row
                if (y % 2 == 0)
                {
                    spawnedHex.transform.position = new Vector3(x * 200, gameBoard.pixelRect.height - y * 180, 0);
                }
                //if not, offset it by a bit
                else
                {
                    spawnedHex.transform.position = new Vector3(100 + x * 200, gameBoard.pixelRect.height - y * 180, 0);
                }

                //go through each side of the hex and connect its buttons up with the hexes next to it

                //top left edge of this hex becomes bot right of above hex
                if(y != 0)
                {
                    if (x != 0 && y % 2 == 0)
                    {
                        Destroy(hex.edgeButtons[(int)Edge.LeftTop]);
                        hex.edgeButtons[(int)Edge.LeftTop] = boardData[x, y - 1].edgeButtons[(int)Edge.RightBot];
                    }
                    if (y % 2 != 0)
                    {
                        Destroy(hex.edgeButtons[(int)Edge.LeftTop]);
                        hex.edgeButtons[(int)Edge.LeftTop] = boardData[x, y - 1].edgeButtons[(int)Edge.RightBot];
                    }
                    if (x != boardData.GetLength(0) - 1 && y % 2 != 0)
                    {
                        Destroy(hex.edgeButtons[(int)Edge.RightTop]);
                        hex.edgeButtons[(int)Edge.RightTop] = boardData[x, y - 1].edgeButtons[(int)Edge.LeftBot];
                    }
                    if (y % 2 == 0)
                    {
                        Destroy(hex.edgeButtons[(int)Edge.RightTop]);
                        hex.edgeButtons[(int)Edge.RightTop] = boardData[x, y - 1].edgeButtons[(int)Edge.LeftBot];
                    }
                }

                if (x != 0)
                {
                    Destroy(hex.edgeButtons[(int)Edge.LeftMid]);
                    hex.edgeButtons[(int)Edge.LeftMid] = boardData[x - 1, y].edgeButtons[(int)Edge.RightMid];
                }
            }
        }


        
    }
}
