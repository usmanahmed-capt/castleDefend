using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RefManager : MonoBehaviour
{
    public GameObject bombParticlePrefab;
    public GameObject convertingParticle;

    [SerializeField] private GameObject archeryFlag;
    [SerializeField] private TextMeshPro archeriesText;
    [SerializeField] private GameObject repairerFlag;
    [SerializeField] private TextMeshPro repairerText;

    public TouchManager touchManager;

    public static RefManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        if (DataManager.Instance.gData.archerryPurchased)
        {
            SetActiveArcherFlag();
        }
        if (DataManager.Instance.gData.WorkshopPurchased)
        {
            SetActiveRepairerFlag();
        }
    }

    public void SetActiveArcherFlag()
    {
        archeryFlag.SetActive(true);
    }

    public void SetActiveRepairerFlag()
    {
        repairerFlag.SetActive(true);
    }

    public void SetArcherriesText()
    {
        archeriesText.text = DataManager.Instance.gData.noOfArcherries.ToString();
    }

    public void SetRepairerText()
    {
        repairerText.text = DataManager.Instance.gData.noOfRepairer.ToString();
    }
}
