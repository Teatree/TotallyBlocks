using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Networking;

public class AnalyticsController : SceneSingleton<AnalyticsController> {

    // Use this for initialization
    void Awake()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
        }
        else
        {
            //Handle FB.Init
            FB.Init(() => {
                FB.ActivateApp();
            });
        }
    }

    IEnumerator WaitForRequest(WWW data)
    {
        yield return data; // Wait until the download is done
        if (data.error != null)
        {
            Debug.Log("There was an error sending request: " + data.error);
        }
        else
        {
            Debug.Log("WWW Request: " + data.text);
        }
    }

    private void OnApplicationPause(bool pause)
    {

        if (!pause)
        {
            //app resume
            if (FB.IsInitialized)
            {
                FB.ActivateApp();
            }
            else
            {
                //Handle FB.Init
                FB.Init(() => {
                    FB.ActivateApp();
                });
            }
        }
    }

    public class JsonData {
        public string playerID = "!%*(AS";
    }

    public class GeneralTrackingData : JsonData {
        public string playerStatus = "online / offline";
    }

    //public void LogPlayersOnline(string playerStatus)
    //{
    //    // Custom tracker
    //    if (!Application.isEditor)
    //    {
    //        GeneralTrackingData generalData = new GeneralTrackingData();
    //        generalData.playerID = PlayerController.player.PlayerID;
    //        generalData.playerStatus = playerStatus;

    //        PostRequest(generalData, "http://5.45.69.185:80/playersonline");
    //    }
    //}

    public class IncentivizedAdTrackingData : JsonData {
        public string ad_ID = "Box Ad";
    }

    //public void LogIncentivizedAdWatchedEvent(string ad_ID)
    //{
    //    if (!Application.isEditor)
    //    {
    //        var parameters = new Dictionary<string, object>();
    //        parameters["ad_id"] = ad_ID;
    //        FB.LogAppEvent(
    //            "IncentiziedVideoWatched",
    //            0,
    //            parameters
    //        );

    //        Analytics.CustomEvent("IncentiziedVideoWatched", new Dictionary<string, object>
    //        {
    //        { "ad_ID", ad_ID}
    //    });

    //        // Custom tracker
    //        IncentivizedAdTrackingData incentivizedAdData = new IncentivizedAdTrackingData();
    //        incentivizedAdData.playerID = PlayerController.player.PlayerID;
    //        incentivizedAdData.ad_ID = ad_ID;

    //        PostRequest(incentivizedAdData, "http://5.45.69.185:80/incentivizedad");
    //    }
    //}


    #region Levels
    public class LevelTrackingData : JsonData {
        public string levelNum = "Level 1, Level 32";
        public string levelResult = "Failed, Resterted, Finished";
        public string starsAmount = "1,2,3 stars at the end";
        public string numberOfShots = "numer of shots in string form";
    }

    public void LogLevelStatusEvent(string levelNum, int shotsCount, int starsCount, string levelResult)
    {
        if (!Application.isEditor)
        {
            // Facebook analytics
            var parameters = new Dictionary<string, object>() {
            {"levelNum2", levelNum }
        };
            FB.LogAppEvent(
                "LevelCompleted",
                0,
                parameters
            );

            // Unity Analytics
            Analytics.CustomEvent("LevelCompleted", new Dictionary<string, object>
            {
            { "levelNum", levelNum }
        });


            // Custom tracker
            LevelTrackingData levelData = new LevelTrackingData();
            //levelData.playerID = PlayerController.player.PlayerID;
            levelData.levelNum = levelNum;
            levelData.starsAmount = starsCount.ToString();
            levelData.levelResult = levelResult;
            levelData.numberOfShots = shotsCount.ToString();

            PostRequest(levelData, "http://5.45.69.185:80/level");
        }
    }
    #endregion

    private void PostRequest(JsonData data, string url)
    {
        string json = JsonUtility.ToJson(data);

        WWW www;
        Hashtable postHeader = new Hashtable();
        postHeader.Add("Content-Type", "application/json");

        // convert json string to byte
        var formData = System.Text.Encoding.UTF8.GetBytes(json);

        www = new WWW(url, formData, postHeader);
        StartCoroutine(WaitForRequest(www));
    }
}
