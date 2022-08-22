using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    public static GameplayController Instance;

    //Others ...
    public Board board;
    public RaycastController raycastController;
    
    //...

    private void Awake()
    {
        //Khoi tao Singleton
        Instance = this;
        Begin();
    }

    private void Begin()
    {
        board.Begin();
    }
}
