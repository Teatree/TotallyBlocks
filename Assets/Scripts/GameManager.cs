using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SceneSingleton<GameManager>
{
    public CubeSpawner cubeSpawner;
    public List<Material> materials;
    public float enemyCubeSpeed;
    public float playerCubeSpeed;
    public float increment; // decrease increment over time
    public float lineMoveSpeedMin;
    public float lineMoveSpeedMax;
    float _lineMoveSpeed;

    public GameState gameState;
    //int colorTick;
    int materialIndex;
    int tick = 0;

    private void Start()
    {
        gameState = GameState.Paused;

        //colorTick = 0;
        materialIndex = Random.Range(0, 5);
    }

    public void GameManage()
    {
        // Spawning of Cubes
        if (gameState != GameState.Active)
            return;

        tick++;

        if (tick > 20)
        {
            cubeSpawner.SpawnLine();
            tick = 0;
        }
    }

    public void RestartGame()
    {
        Debug.Log("RESTARTED");
        DataController.SavePlayer(PlayerController.playerData);
        SceneManager.LoadScene(0);
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

    public void MoveCubeLine()
    {
        if (CubeSpawner.cubeLineByIndex[CubeSpawner.firstIndexAlive].transform.position.z > 3.5f)
        {
            _lineMoveSpeed = lineMoveSpeedMax;
        }
        else
        {
            _lineMoveSpeed = lineMoveSpeedMin;
        }

        foreach(int k in CubeSpawner.cubeLineByIndex.Keys)
        {
            Vector3 pos = CubeSpawner.cubeLineByIndex[k].transform.position;
            pos = new Vector3(pos.x, pos.y, pos.z - _lineMoveSpeed);

            CubeSpawner.cubeLineByIndex[k].transform.position = Vector3.MoveTowards(CubeSpawner.cubeLineByIndex[k].transform.position, pos, 0.05f);
        }
    }

    public void Lose(int score)
    {
        Debug.Log("You lose!");
        gameState = GameState.Lost;
        UIManager.Instance.ShowRestartUI();
        UIManager.Instance.SetRestartScoreValues(score, PlayerController.playerData.highestScore);
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
