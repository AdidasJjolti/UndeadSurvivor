using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PoolManager pool;
    public Player player;

    public float gameTime;
    public float maxGameTime;

    void Awake()
    {
        instance = this;
        maxGameTime = 20f;
    }

    void Update()
    {
        gameTime = gameTime + Time.deltaTime;
        if(gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
        }
    }
}
