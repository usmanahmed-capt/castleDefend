using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeScreenHandler : MonoBehaviour
{
    [SerializeField] private GameData gData;

    [Header("Upgrade Components")]
    [SerializeField] private Image upgradeScreenTempleHealthBar;
    [SerializeField] private Text upgradeScreenTempleHealthText;
    [SerializeField] private Text upgradeScreenSpendablePoints;

    [SerializeField] private List<Button> buildingsButton;
    [SerializeField] private List<Image> buildingsButtonImages;
    [SerializeField] private List<Sprite> buildingsImages;

    [SerializeField] private GameObject notEnoughPoints;

    // Start is called before the first frame update
    void Start()
    {
        UpgradeScreenSetTempleHealth();
        UpdateBuildingEntries();
        SetSpendablePoints();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateBuildingEntries()
    {
        buildingsButton[0].interactable = !gData.templePurchased;
        buildingsButton[1].interactable = !gData.archerryPurchased;
        buildingsButton[2].interactable = !gData.DemolationPurchased;
        buildingsButton[3].interactable = !gData.WorkshopPurchased;

        if (gData.templePurchased)
        {
            buildingsButtonImages[0].GetComponent<Image>().sprite = buildingsImages[0];
        }
        if (gData.archerryPurchased)
        {
            buildingsButtonImages[1].GetComponent<Image>().sprite = buildingsImages[1];
        }
        if (gData.DemolationPurchased)
        {
            buildingsButtonImages[2].GetComponent<Image>().sprite = buildingsImages[2];
        }
        if (gData.WorkshopPurchased)
        {
            buildingsButtonImages[3].GetComponent<Image>().sprite = buildingsImages[3];
        }
        for (int i = 0; i < buildingsButtonImages.Count; i++)
        {
            buildingsButtonImages[i].SetNativeSize();
        }

    }

    public void UpgradeScreenSetTempleHealth()
    {
        float health = (gData.templeCurrentHealth / gData.templeTotalHealth);
        upgradeScreenTempleHealthBar.fillAmount = health;
        var cHealth = gData.templeCurrentHealth > gData.templeTotalHealth ? gData.templeTotalHealth : gData.templeCurrentHealth;
        upgradeScreenTempleHealthText.text = cHealth + " / " + gData.templeTotalHealth;
    }

    public async void OnClickUpGradeEntries(int index)
    {
        if (gData.upgradesPrice[index] <= gData.scorePoints)
        {
            switch (index)
            {
                case 0:
                    {
                        gData.templeCurrentHealth += 50;
                    }
                    break;
                case 1:
                    {
                        gData.templeCurrentHealth += 250;
                    }
                    break;
                case 2:
                    {
                        gData.templeCurrentHealth += 1000;
                    }
                    break;
                case 3:
                    {
                        gData.templeTotalHealth += 100;
                    }
                    break;
                case 4:
                    {
                        gData.templeTotalHealth += 1000;
                    }
                    break;
                default:
                    break;
            }
            gData.scorePoints -= gData.upgradesPrice[index];
            UpgradeScreenSetTempleHealth();
            UIManager.Instance.SetTempleHealth();
        }
        else
        {
            Debug.Log("Not Enough Score Points");
            notEnoughPoints.SetActive(true);
            await Task.Delay(1000);
            notEnoughPoints.SetActive(false);
        }
        SetSpendablePoints();
    }

    public async void OnClickBuildingEntries(int index)
    {

        if (gData.upgradesBuildingPrice[index] <= gData.scorePoints)
        {
            switch (index)
            {
                case 0:
                    {
                        gData.templePurchased = true;
                    }
                    break;
                case 1:
                    {
                        gData.archerryPurchased = true;
                        RefManager.Instance.SetActiveArcherFlag();
                    }
                    break;
                case 2:
                    {
                        gData.DemolationPurchased = true;
                    }
                    break;
                case 3:
                    {
                        gData.WorkshopPurchased = true;
                        RefManager.Instance.SetActiveRepairerFlag();
                    }
                    break;
                default:
                    break;
            }
            buildingsButton[index].interactable = false;
            buildingsButtonImages[index].GetComponent<Image>().sprite = buildingsImages[index];
            buildingsButtonImages[index].SetNativeSize();
            gData.scorePoints -= gData.upgradesBuildingPrice[index];
        }
        else
        {
            Debug.Log("Not Enough Score Points");
            notEnoughPoints.SetActive(true);
            await Task.Delay(1000);
            notEnoughPoints.SetActive(false);
        }
        SetSpendablePoints();
    }

    public void SetSpendablePoints()
    {
        upgradeScreenSpendablePoints.text = "Spendable Points: " + gData.scorePoints;
    }

    public void OnClickHomeButton()
    {
        // Add your Logic here
        GameManager.Instance.LoadScene("MenuScene");
    }

    public void OnClickOkButton()
    {
        //DataManager.Instance.LoadData();
        GameManager.Instance.ChkIfFail = false;
        GameManager.Instance.LoadScene("GameScene");
    }
}
