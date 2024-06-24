using System;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    private void Awake()
    {
        data = new Data();
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public GameData gData;

    public Data data;
    public void SaveData()
    {
        PlayerPrefs.SetString("saveGame", "true");

        if (!UIManager.Instance.saveOnce)
        {
            GameManager.Instance.SelectedLevel += 1;
            PlayerPrefs.SetInt("SelectedLevel", GameManager.Instance.SelectedLevel);
            UIManager.Instance.saveOnce = true;
        }

        //gData.templeCurrentHealth = data.templeCurrentHealth;
        SaveIntoPlayerPrefs(UTILS.TEMPLECURRENTHEALTH, gData.templeCurrentHealth);

        //gData.templeTotalHealth = data.templeTotalHealth;
        SaveIntoPlayerPrefs(UTILS.TEMPLETOTALHEALTH, gData.templeTotalHealth);

        //gData.scorePoints = data.scorePoints;
        SaveIntoPlayerPrefs(UTILS.SCOREPOINTS, gData.scorePoints);

        //gData.convertedPlayers = data.convertedPlayers;
        SaveIntoPlayerPrefs(UTILS.CONVERTEDPLAYERS, gData.convertedPlayers);

        //gData.noOfArcherries = data.noOfArcherries;
        SaveIntoPlayerPrefs(UTILS.NOOFARCHERRIES, gData.noOfArcherries);

        //gData.noOfRepairer = data.noOfRepairer;
        SaveIntoPlayerPrefs(UTILS.NOOFREPAIRER, gData.noOfRepairer);

        //gData.enemiesKilled = data.enemiesKilled;
        SaveIntoPlayerPrefs(UTILS.ENEMIESKILLED, gData.enemiesKilled);



        //gData.templePurchased = data.templePurchased;
        SaveBooleanIntoPlayerPrefs(UTILS.TEMPLEPURCHASED, gData.templePurchased);

        //gData.archerryPurchased = data.archerryPurchased;
        SaveBooleanIntoPlayerPrefs(UTILS.ARCHERRYPURCHASED, gData.archerryPurchased);

        //gData.DemolationPurchased = data.DemolationPurchased;
        SaveBooleanIntoPlayerPrefs(UTILS.DEMOLATIONPURCHASED, gData.DemolationPurchased);

        //gData.WorkshopPurchased = data.WorkshopPurchased;
        SaveBooleanIntoPlayerPrefs(UTILS.WORKSHOPPURCHASED, gData.WorkshopPurchased);
    }

    public void LoadData()
    {

        GameManager.Instance.SelectedLevel = PlayerPrefs.GetInt("SelectedLevel", 0);

        var cHealth = LoadFromPlayerPrefs(UTILS.TEMPLECURRENTHEALTH);
        if (cHealth > 0)
        { gData.templeCurrentHealth = cHealth; }
        else data.templeCurrentHealth = 1000;

        var tHealth = LoadFromPlayerPrefs(UTILS.TEMPLETOTALHEALTH);
        if (tHealth > 0)
            gData.templeTotalHealth = tHealth;
        else data.templeTotalHealth = 1000;

        var sPoints = LoadFromPlayerPrefs(UTILS.SCOREPOINTS);
        gData.scorePoints = Convert.ToInt32(sPoints);
        data.scorePoints = gData.scorePoints;

        var cPlayer = LoadFromPlayerPrefs(UTILS.CONVERTEDPLAYERS);
        gData.convertedPlayers = Convert.ToInt32(cPlayer);
        data.convertedPlayers = gData.convertedPlayers;

        var noArchers = LoadFromPlayerPrefs(UTILS.NOOFARCHERRIES);
        gData.noOfArcherries = Convert.ToInt32(noArchers);
        data.noOfArcherries = gData.noOfArcherries;

        var noRepairs = LoadFromPlayerPrefs(UTILS.NOOFREPAIRER);
        gData.noOfRepairer = Convert.ToInt32(noRepairs);
        data.noOfRepairer = gData.noOfRepairer;

        var eKilled = LoadFromPlayerPrefs(UTILS.ENEMIESKILLED);
        gData.enemiesKilled = Convert.ToInt32(eKilled);
        data.enemiesKilled = gData.enemiesKilled;

        var tPurchased = LoadBooleanFromPlayerPrefs(UTILS.TEMPLEPURCHASED);
        gData.templePurchased = tPurchased;
        data.templePurchased = gData.templePurchased;

        var archer = LoadBooleanFromPlayerPrefs(UTILS.ARCHERRYPURCHASED);
        gData.archerryPurchased = archer;
        data.archerryPurchased = gData.archerryPurchased;

        var demolation = LoadBooleanFromPlayerPrefs(UTILS.DEMOLATIONPURCHASED);
        gData.DemolationPurchased = demolation;
        data.DemolationPurchased = gData.DemolationPurchased;

        var workShop = LoadBooleanFromPlayerPrefs(UTILS.WORKSHOPPURCHASED);
        gData.WorkshopPurchased = workShop;
        data.WorkshopPurchased = gData.WorkshopPurchased;

    }

    private void SaveIntoPlayerPrefs(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }

    private void SaveBooleanIntoPlayerPrefs(string key, bool value)
    {
        PlayerPrefs.SetInt(key, value ? 1 : 0);
    }

    private float LoadFromPlayerPrefs(string key)
    {
        return PlayerPrefs.GetFloat(key);
    }

    private bool LoadBooleanFromPlayerPrefs(string key)
    {
        return PlayerPrefs.GetInt(key) == 1;
    }

    public void ResetData()
    {
        gData.templeCurrentHealth = 1000;
        gData.templeTotalHealth = 1000;
        gData.scorePoints = 0;
        gData.convertedPlayers = 0;
        gData.noOfArcherries = 0;
        gData.noOfRepairer = 0;
        gData.enemiesKilled = 0;

        gData.templePurchased = false;
        gData.archerryPurchased = false;
        gData.DemolationPurchased = false;
        gData.WorkshopPurchased = false;
    }
}
