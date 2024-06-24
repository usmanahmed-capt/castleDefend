using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [NonSerialized]
    public bool conversionInProgress = false;
    public int SelectedLevel;
    [NonSerialized] public bool ChkIfFail; 

    [NonSerialized]
    public int currentLevelKillings;
    [NonSerialized] public bool isRevive;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }else Destroy(this.gameObject);
    }

    private void Start()
    {

    }

    public void IncrementLevel()
    {
        if (!UIManager.Instance.saveOnce)
        {
            SelectedLevel++;
            UIManager.Instance.saveOnce = true;
        }
    }

    public void StartConversion()
    {
        conversionInProgress = true;
        RefManager.Instance.convertingParticle.SetActive(true);
        Invoke("StopConversion", 10);
    }

    public void StopConversion()
    {
        DataManager.Instance.gData.convertedPlayers += 1;
        //if (DataManager.Instance.gData.DemolationPurchased)
        //{
            SpawnManager.Instance.SpawnStickMan();
      //  }
        conversionInProgress = false;
        RefManager.Instance.convertingParticle.SetActive(false);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

}