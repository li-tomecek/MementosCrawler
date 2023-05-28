using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    //-------------------------
    //       GRID DATA
    //-------------------------
    public int rows = 1;
    public int columns = 1;

    [HideInInspector]   public float tileWidth;
    [HideInInspector]   public float tileHeight;


}
