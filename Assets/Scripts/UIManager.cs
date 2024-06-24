using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [HideInInspector]
    public bool levelFailed;

    public GameObject fadeImage;
    public Text fadeImageText;

    public GameObject managers;

    [Header("Upgrade Components")]
    [SerializeField] private GameObject upgradeScreen;

    [Header("Stage Complete Screen")]
    [SerializeField] private GameObject stageCompleteScreen;

    [Header("Stage Failed Screen")]
    [SerializeField] private GameObject stageFailedScreen;

    [SerializeField] private GameObject pauseScreen;

    [SerializeField] private GameObject templeHealth;
    [SerializeField] private Image templeHealthBar;
    [SerializeField] private Text templeHealthText;

    [SerializeField] private Text casualties;

    [SerializeField] private Text scorePoints;

    public bool saveOnce;

    [NonSerialized] public bool gamePaused;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Time.timeScale = 1;
        SetLevelText();
        SetConvertedText();
        SetTempleHealth();
        SetCasualties();
        SetPoints();
        if (GameManager.Instance.ChkIfFail)
        {
            upgradeScreen.SetActive(true);
        }
    }

    private void SetLevelText()
    {
        fadeImage.SetActive(true);
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {

        fadeImageText.text = "STAGE " + (GameManager.Instance.SelectedLevel + 1);
        yield return new WaitForSeconds(1);
        fadeImage.SetActive(false);
        managers.SetActive(true);
    }

    #region GamePlay
    public void SetConvertedText()
    {
        for (int i = 0; i < DataManager.Instance.gData.convertedPlayers; i++)
        {
            SpawnManager.Instance.SpawnStickMan();
        }
    }

    public void OnClickArcherries()
    {
        if (DataManager.Instance.gData.templePurchased)
        {
            if (DataManager.Instance.gData.archerryPurchased)
            {
                if (DataManager.Instance.gData.convertedPlayers > 0)
                {
                    DataManager.Instance.gData.convertedPlayers--;
                    DataManager.Instance.gData.noOfArcherries++;
                    RefManager.Instance.SetActiveArcherFlag();
                    RefManager.Instance.SetArcherriesText();
                    SpawnManager.Instance.SpawnPlayers(PlayerType.Archerry, DataManager.Instance.gData.noOfArcherries - 1);
                }
            }
        }

    }

    public void OnClickBomberman()
    {
        if (DataManager.Instance.gData.templePurchased)
        {
            if (DataManager.Instance.gData.DemolationPurchased)
            {
                if (DataManager.Instance.gData.convertedPlayers > 0)
                {
                    DataManager.Instance.gData.convertedPlayers--;
                    SpawnManager.Instance.SpawnPlayers(PlayerType.Bomber);
                }
            }
        }
    }

    public void OnClickRepairer()
    {
        if (DataManager.Instance.gData.templePurchased)
        {
            if (DataManager.Instance.gData.WorkshopPurchased)
            {
                if (DataManager.Instance.gData.convertedPlayers > 0)
                {
                    DataManager.Instance.gData.convertedPlayers--;
                    DataManager.Instance.gData.noOfRepairer++;
                    RefManager.Instance.SetActiveRepairerFlag();
                    SpawnManager.Instance.SpawnPlayers(PlayerType.Repair);
                }
            }
        }
        RefManager.Instance.SetRepairerText();
    }


    public void SetTempleHealth()
    {
        float health = (DataManager.Instance.gData.templeCurrentHealth / DataManager.Instance.gData.templeTotalHealth);
        templeHealthBar.fillAmount = health;
        var cHealth = DataManager.Instance.gData.templeCurrentHealth > DataManager.Instance.gData.templeTotalHealth ? DataManager.Instance.gData.templeTotalHealth : DataManager.Instance.gData.templeCurrentHealth;
        templeHealthText.text = cHealth + " / " + DataManager.Instance.gData.templeTotalHealth;
    }

    public void SetPoints()
    {
        scorePoints.text = "" + DataManager.Instance.gData.scorePoints;
    }

    public void SetCasualties()
    {
        casualties.text = "Casualties: " + DataManager.Instance.gData.enemiesKilled;
    }

    public void OnClickPauseButton(bool chk)
    {
        Time.timeScale = chk ? 0 : 1;

    }

    private void PauseGame(bool chk)
    {
        pauseScreen.SetActive(chk);
    }

    public void OnClickPauseMenuButtons(string sceneName)
    {
        GameManager.Instance.LoadScene(sceneName);
    }

    #endregion

    #region General

    public void SetActiveCompleteScreen()
    {
        stageCompleteScreen.SetActive(true);
        RefManager.Instance.touchManager.selectedObject = null;
        Time.timeScale = 0;
        //GameManager.instance.StartGame(false);
    }

    public void SetActiveFailedScreen()
    {
        stageFailedScreen.SetActive(true);
        Time.timeScale = 0;
        //GameManager.instance.StartGame(false);
    }

    public void OnReviveGameAfterFail()
    {
        stageFailedScreen.SetActive(false);
        DataManager.Instance.gData.templeCurrentHealth += ((DataManager.Instance.gData.templeTotalHealth) * 10) / 100;
        levelFailed = false;
        SetTempleHealth();
        Time.timeScale = 1;
    }

    #endregion


    private void OnApplicationFocus(bool focus)
    {
        if (!upgradeScreen.activeInHierarchy && !stageCompleteScreen.activeInHierarchy && !stageFailedScreen.activeInHierarchy && !fadeImage.activeInHierarchy)
        {
            PauseGame(!focus);
            Time.timeScale = focus ? 1 : 0;
            gamePaused = !focus;
        }
    }

}
