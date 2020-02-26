using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

    private static Dictionary<string, bool> sceneStateTracker;
    public static SceneController sceneController;
    bool gameStart;
    public static string initScene = "";

    private static bool shouldShowRestartIntersticial;
    public static int shouldShowLevelIntersticial = 2; // counter of how many times player should load level before he is shown an ad
    public static int shouldShowLevelIntersticialcounter;

    public void Awake()
    {
        Application.targetFrameRate = 60;
        LoadScene("Game");
    }

    private void UnloadScene(string scene)
    {
        //Debug.Log("> u >" + sceneStateTracker[scene]);
        StartCoroutine(Unload(scene));
    }

    public void UnloadScene(int scene)
    {
        StartCoroutine(Unload(scene));
    }

    public void LoadScene(string scene)
    {
            //Debug.Log("> l >" + scene);
        SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
    }

    private IEnumerator Unload(string scene)
    {
        yield return null;
        SceneManager.UnloadSceneAsync(scene);
    }

    private IEnumerator Unload(int scene)
    {
        yield return null;
        SceneManager.UnloadSceneAsync(scene);
    }
}
