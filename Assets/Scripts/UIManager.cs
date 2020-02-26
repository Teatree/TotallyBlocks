using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SceneSingleton<UIManager> {

    public Button restartButton;
    public Slider feedbackSlider;

    public Image feedbackBg;
    public Text feedbackTxt;

    public GameObject fingerGuide;
    public GameObject startButton;

    public void ShowRestartUI()
    {
        restartButton.gameObject.SetActive(true);
    }

    public void HideRestartUI()
    {
        restartButton.gameObject.SetActive(false);
    }

    public void UpdateFeedbackSliderUI(float val)
    {
        feedbackSlider.value = val;
    }

    public void ShowRewardedVideoAd()
    {
        UnityAdsController.Instance.ShowRewardVideoNastya();
    }

    public void RestartGame()
    {
        GameManager.Instance.RestartGame();
        HideRestartUI();
    }

    public void HideFinger()
    {
        fingerGuide.SetActive(false);
        startButton.SetActive(false);
    }

    public void DisplayFeedbackMessage(string txt)
    {
        feedbackBg.gameObject.SetActive(true);

        feedbackBg.color = new Color(feedbackBg.color.r, feedbackBg.color.b, feedbackBg.color.g, 1);
        feedbackTxt.text = txt;
    }
    // process restart button pressing
}
