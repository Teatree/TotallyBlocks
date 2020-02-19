using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }

    public CubeSpawner cubeSpawner;
    public List<Material> materials;
    public float enemyCubeSpeed;
    public float playerCubeSpeed;
    public float increment; // decrease increment over time

    public GameState gameState;
    //int colorTick;
    int materialIndex;
    int tick = 0;

    private void Awake()
    {
        gameState = GameState.Active;

        //colorTick = 0;
        materialIndex = Random.Range(0, 5);

        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void GameManage()
    {
        // Spawning of Cubes
        tick++;

        if (tick > 20)
        {
            cubeSpawner.SpawnLine();
            tick = 0;
        }
    }

    public Material PickRandomColour()
    {
        Material mat;

        //colorTick++;
        //Debug.Log("colorTick: " + colorTick);
        //if(colorTick > 6)
        //{
            int prevIndex = materialIndex;
            materialIndex = Random.Range(0, 5);
            if(prevIndex == materialIndex)
            {
                materialIndex = Random.Range(0, 5);
            }
            mat = materials[materialIndex];
            //colorTick = 0;
       // }
        //else
        //{
           // mat = materials[materialIndex];
        //}
        return mat;
    }

    public void Lose()
    {
        Debug.Log("You lose!");
        gameState = GameState.Lost;
    }

    public enum GameState{
        Lost,
        Win,
        Active,
        Paused
    }

    public void IncreaseSpeed()
    {
        enemyCubeSpeed += increment;
    }

    private void Update()
    {
        GameManage();
    }
}
