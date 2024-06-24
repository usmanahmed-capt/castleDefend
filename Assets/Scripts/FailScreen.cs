using AdmobAds;
using UnityEngine;
using UnityEngine.UI;

public class FailScreen : MonoBehaviour
{
    [SerializeField] private Text casualties;
    [SerializeField] private Text finalScore;

    [SerializeField] private GameObject reviveButton;
    [SerializeField] private Image homeButton;
    [SerializeField] private Sprite homeSprite;
    [SerializeField] private Sprite skipSprite;

    // Start is called before the first frame update
    void Start()
    {
        SetGameScoreValues();
    }

    private void OnEnable()
    {
        SetReviveButtn();
    }

    public void SetGameScoreValues()
    {
        casualties.text = "Casualties: " + GameManager.Instance.currentLevelKillings;
        finalScore.text = "Final Score: " + GameManager.Instance.currentLevelKillings * GamePlayManager.Instance.gameLevels[GameManager.Instance.SelectedLevel].scoreVal * GamePlayManager.Instance.gameLevels[GameManager.Instance.SelectedLevel].scoreMultiplier;
    }

    private void SetReviveButtn()
    {
        if (AdsManager.Instance.retryRevives > 0)
        {
            reviveButton.SetActive(true);
            homeButton.sprite = skipSprite;
        }
        else
        {
            reviveButton.SetActive(false);
            homeButton.sprite = homeSprite;
        }
    }

    public void OnClickWatchAdVideo()
    {
        AdsManager.Instance.ShowRewardedAd();
    }

    public void OnClickOkButton()
    {
        GameManager.Instance.ChkIfFail = true;
        GameManager.Instance.LoadScene("MenuScene");
    }
}
