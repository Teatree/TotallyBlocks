using UnityEngine;
using UnityEngine.Advertisements;
using System.Collections;

public class UnityAdsBanner : MonoBehaviour {

    private string banner = "Banner";
    public bool testMode = false;

#if UNITY_ANDROID
    public const string gameId = "3482883";
#elif UNITY_EDITOR
    public const string gameId = "1111111";
#endif

    // Use this for initialization
    void Awake()
    {
        Advertisement.Initialize(gameId, testMode);
        Advertisement.Banner.SetPosition(BannerPosition.TOP_CENTER);

        StartCoroutine(ShowBannerWhenReady());
    }

    IEnumerator ShowBannerWhenReady()
    {
        while (!Advertisement.IsReady(banner))
        {
            yield return new WaitForSeconds(0.5f);
        }
        Advertisement.Banner.Show(banner);
    }
}
