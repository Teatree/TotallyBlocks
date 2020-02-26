using UnityEngine;

using UnityEngine.Monetization;

public class UnityAdsController : SceneSingleton<UnityAdsController> {
    private string gameId = "3482883";
    private string reviveVideo = "rewardedVideo";

    public static bool AdsLoaded;

    //intersticials
    private string intersticialVideo = "video";

    void Start()
    {
#pragma warning disable CS0618 // Type or member is obsolete
        if (Monetization.isSupported)
#pragma warning restore CS0618 // Type or member is obsolete
        {
#pragma warning disable CS0618 // Type or member is obsolete
            Monetization.Initialize(gameId, false);
#pragma warning restore CS0618 // Type or member is obsolete
        }

        AdsLoaded = Application.internetReachability != NetworkReachability.NotReachable;
    }

    public void ShowRewardVideoNastya()
    {
        ShowAdCallbacks options = new ShowAdCallbacks();
        options.finishCallback = HandleRewardVideoNastya;
#pragma warning disable CS0618 // Type or member is obsolete
        ShowAdPlacementContent ad = Monetization.GetPlacementContent(reviveVideo) as ShowAdPlacementContent;
#pragma warning restore CS0618 // Type or member is obsolete
        ad.Show(options);

        Debug.Log("give reward for video!");

        //AnalyticsController.Instance.LogIncentivizedAdWatchedEvent("More HC AD");
    }

    void HandleRewardVideoNastya(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
            Debug.LogWarning(" REWARD!");
            UIManager.Instance.RestartGame();
        }
        else if (result == ShowResult.Skipped)
        {
            Debug.LogWarning("The player skipped the video - DO NOT REWARD!");
        }
        else if (result == ShowResult.Failed)
        {
            Debug.LogError("Video failed to show");
        }
    }


    public void ShowIntersticialVideoAd()
    {
        ShowAdCallbacks options = new ShowAdCallbacks();
        options.finishCallback = HandleIntersticialVideoAd;

#pragma warning disable CS0618 // Type or member is obsolete
        ShowAdPlacementContent ad = Monetization.GetPlacementContent(intersticialVideo) as ShowAdPlacementContent;
#pragma warning restore CS0618 // Type or member is obsolete
        ad.Show(options);
    }

    void HandleIntersticialVideoAd(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
            SceneController.sceneController.LoadScene("Game");
        }
        else if (result == ShowResult.Skipped)
        {
            SceneController.sceneController.LoadScene("Game");
        }
        else if (result == ShowResult.Failed)
        {
            SceneController.sceneController.LoadScene("Game");
        }
    }

}
