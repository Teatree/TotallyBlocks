using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SceneSingleton<UIManager> {

    public Button restartButton;
    public GameObject restartPanel;
    public Slider feedbackSlider;

    public Image feedbackBg;
    public Text feedbackTxt;

    public Text scoreText;
    public Text restartScoreText;
    public Text restartHighScoreText;

    public GameObject fingerGuide;
    public GameObject startButton;

    private void Start()
    {
        scoreText.gameObject.SetActive(false);
        feedbackSlider.gameObject.SetActive(false);
    }

    public void ShowRestartUI()
    {
        scoreText.gameObject.SetActive(false);
        restartPanel.gameObject.SetActive(true);
    }

    public void HideRestartUI()
    {
        restartPanel.gameObject.SetActive(false);
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
        GameManager.Instance.gameState = GameManager.GameState.Active;

        fingerGuide.SetActive(false);
        startButton.SetActive(false);

        scoreText.gameObject.SetActive(true);
        feedbackSlider.gameObject.SetActive(true);
    }

    public void DisplayFeedbackMessage(string txt)
    {
        feedbackBg.gameObject.SetActive(true);

        feedbackBg.color = new Color(feedbackBg.color.r, feedbackBg.color.b, feedbackBg.color.g, 1);
        feedbackTxt.text = txt;
    }

    public void SetRestartScoreValues(int score, int highScore)
    {
        restartScoreText.text = ""+score;
        restartHighScoreText.text = "High: " + highScore;
    }
    // process restart button pressing
}