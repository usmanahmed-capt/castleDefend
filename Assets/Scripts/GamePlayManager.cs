using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class GamePlayManager : MonoBehaviour
{
    public static GamePlayManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public List<int> diePoints = new List<int>
    {
        2,3,5
    };
    public List<GameLevels> gameLevels = new List<GameLevels>();
    void Start()
    {
        SetLevels();
        CreateAssets();
    }

    public async void CreateAssets()
    {
        var archer = DataManager.Instance.gData.noOfArcherries;
        if (archer > 0)
        {
            for (int i = 0; i < archer; i++)
            {
                var delayTime = Random.Range(0.0f, .7f);
                await Task.Delay(TimeSpan.FromSeconds(delayTime));
                SpawnManager.Instance.SpawnPlayers(PlayerType.Archerry, i);
            }
            RefManager.Instance.SetArcherriesText();
        }
        var repairer = DataManager.Instance.gData.noOfRepairer;
        if (repairer > 0)
        {
            for (int i = 0; i < repairer; i++)
            {
                SpawnManager.Instance.SpawnPlayers(PlayerType.Repair);
            }
            RefManager.Instance.SetRepairerText();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 25))
            {
                if (hit.collider != null)
                {
                    if (hit.collider.CompareTag("RedFlag"))
                    {
                        UIManager.Instance.OnClickArcherries();
                        if (SpawnManager.Instance.stickMan.Count > 0)
                        {
                            var sMan = SpawnManager.Instance.stickMan[0];
                            SpawnManager.Instance.stickMan.Remove(sMan);
                            Destroy(sMan);
                        }
                    }
                    if (hit.collider.CompareTag("GreenFlag"))
                    {
                        print("greenflag");
                        UIManager.Instance.OnClickRepairer();
                        if (SpawnManager.Instance.stickMan.Count > 0)
                        {
                            var sMan = SpawnManager.Instance.stickMan[0];
                            SpawnManager.Instance.stickMan.Remove(sMan);
                            Destroy(sMan);
                        }
                    }
                }
            }
        }
    }


    public void SetLevels()
    {
        for (int i = 0; i < gameLevels.Count; i++)
        {
            gameLevels[i].levelName = "Level " + (i + 1);

            for (int j = 0; j < gameLevels[i].enemies.Count; j++)
            {
                gameLevels[i].enemies[j].diePoints = diePoints[j];
            }
        }
    }


}
[Serializable]
public class GameLevels
{
    public string levelName;
    public float timeLeft;
    public int scoreVal;
    public float scoreMultiplier;
    public List<Enemy> enemies;

}

[Serializable]
public class Enemy
{
    public EnemyType type;
    public bool hasEnemy;
    public float speed;
    public float enemyDelayTime;
    public float enemyDelayCount;
    public int healthRate;
    public int diePoints;
}