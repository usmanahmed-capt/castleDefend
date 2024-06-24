using AdmobAds;
using System;
using UnityEngine;
using UnityEngine.UI;

public class StageCompleteScreen : MonoBehaviour
{

    [SerializeField] private GameData gData;

    [SerializeField] private Text StagePoints;
    [SerializeField] private Text KillPoints;
    [SerializeField] private Text UnitUnKeep;
    [SerializeField] private Text SpendablePoints;
    [SerializeField] private Text TotalPoints;

    // Start is called before the first frame update
    void Start()
    {
        SetGameScoreValues();
    }

    public void SetGameScoreValues()
    {
        var stagePoints = GameManager.Instance.currentLevelKillings * GamePlayManager.Instance.gameLevels[GameManager.Instance.SelectedLevel].scoreVal * GamePlayManager.Instance.gameLevels[GameManager.Instance.SelectedLevel].scoreMultiplier;
        StagePoints.text = "Stage Points: " + Convert.ToInt32(stagePoints);
        var killBonus = GameManager.Instance.currentLevelKillings * GamePlayManager.Instance.gameLevels[GameManager.Instance.SelectedLevel].scoreVal;
        KillPoints.text = "Kill Bonus: " + killBonus;
        var unitUpKeep = (gData.noOfArcherries * 20) + (gData.noOfDemolition * 25) + (gData.noOfRepairer * 30);
        if (unitUpKeep <= 0) unitUpKeep *= -1;
        UnitUnKeep.text = "Unit UpKeep: " + unitUpKeep;
        var scorePoints = gData.scorePoints;
        SpendablePoints.text = "Spendable Points: " + scorePoints;
        var totalPoints = stagePoints + killBonus + (unitUpKeep) + scorePoints;
        TotalPoints.text = "Total Points: " + Convert.ToInt32(totalPoints);
        gData.scorePoints = Convert.ToInt32(totalPoints);

    }

    public void OnClickOkButton()
    {
        if (GameManager.Instance.SelectedLevel % 2 == 1)
        {
            GameManager.Instance.isRevive = false;
            AdsManager.Instance.ShowInterstitial();
        }
        GameManager.Instance.IncrementLevel();
    }

    public void OnClickSaveButton()
    {
        DataManager.Instance.SaveData();
    }

}
