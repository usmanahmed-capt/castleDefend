using AdmobAds;
using System.Threading.Tasks;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject noGameSaved;

    #region MainScreen

    public void OnClickMainScreenNewGameButton()
    {
        GameManager.Instance.ChkIfFail = false;
        Time.timeScale = 1;
        DataManager.Instance.ResetData();
        GameManager.Instance.LoadScene("GameScene");

    }

    public async void OnClickMainScreenLoadGameButtonAsync()
    {
        if (PlayerPrefs.GetString("saveGame", "false") != "true")
        {
            Debug.Log("No Game Saved");
            try
            {
                noGameSaved.SetActive(true);
                await Task.Delay(1000);
                noGameSaved.SetActive(false);
            }
            catch (System.Exception)
            {

                
            }
            return;
        }
        Time.timeScale = 1;
        DataManager.Instance.LoadData();
        GameManager.Instance.LoadScene("GameScene");
    }

    public void OnClickMainScreenHelpButton()
    {
        // Add your Logic
    }
    #endregion
}
