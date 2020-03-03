using UnityEngine;
using UnityEngine.Advertisements;
using System.Collections;

public class UnityAdsBanner : MonoBehaviour {


#if UNITY_ANDROID
    public const string gameId = "3482883";
#elif UNITY_EDITOR
    public const string gameId = "1111111";
#endif

    public string placementId = "Banner";
    public bool testMode = true;

    void Start()
    {
        Advertisement.Initialize(gameId, testMode);
        StartCoroutine(ShowBannerWhenReady());
    }

    IEnumerator ShowBannerWhenReady()
    {
        while (!Advertisement.IsReady(placementId))
        {
            yield return new WaitForSeconds(0.5f);
        }
        Advertisement.Banner.Show(placementId);
    }

















    //// Use this for initialization
    //void Awake()
    //{
    //    Advertisement.Initialize(gameId, testMode);
    //    Advertisement..SetPosition(BannerPosition.TOP_CENTER);

    //    StartCoroutine(ShowBannerWhenReady());
    //}

    //IEnumerator ShowBannerWhenReady()
    //{
    //    while (!Advertisement.IsReady(banner))
    //    {
    //        yield return new WaitForSeconds(0.5f);
    //    }
    //    Advertisement.Banner.Show(banner);
    //}
}
