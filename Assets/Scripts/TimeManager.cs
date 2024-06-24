using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private float timeLeft;
    [SerializeField] private bool secondPassed;
    // Start is called before the first frame update
    void Start()
    {
        timeLeft = GamePlayManager.Instance.gameLevels[GameManager.Instance.SelectedLevel].timeLeft;
    }

    // Update is called once per frame
    async void Update()
    {
        if (!UIManager.Instance.gamePaused)
        {
            if (timeLeft > 0)
            {
                if (!secondPassed)
                {
                    secondPassed = true;
                    await Task.Delay(1000);
                    timeLeft--;
                    secondPassed = false;
                }
            }
            else
            {
                if (!UIManager.Instance.levelFailed)
                {
                    UIManager.Instance.SetActiveCompleteScreen();
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
