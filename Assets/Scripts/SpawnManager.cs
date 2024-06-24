using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public List<GameObject> enemies = new List<GameObject>();

    public List<GameObject> Enemy;
    public GameObject Player;
    public Path[] path;
    [SerializeField] private Transform enemiesParent;

    public List<Transform> archerryPosition;
    public Transform bomberManPosition;
    public Transform bomberManEndPoint;
    [SerializeField] private Transform stickManContent;
    [SerializeField] private GameObject stickManPrefab;
    public GameObject emptyTarget;

    public List<GameObject> stickMan = new List<GameObject>();
    int enemyIndex;
    // Start is called before the first frame update
    void Start()
    {
        enemyIndex = Random.Range(0, GamePlayManager.Instance.gameLevels[GameManager.Instance.SelectedLevel].enemies.Count);
        InvokeRepeating("SpawnEnemies", 1, GamePlayManager.Instance.gameLevels[GameManager.Instance.SelectedLevel].enemies[enemyIndex].enemyDelayTime);
    }

    public void SpawnStickMan()
    {
        var stickMan = Instantiate(stickManPrefab, stickManContent);
        stickMan.GetComponent<Button>().enabled = true;
        this.stickMan.Add(stickMan);
    }

    public async void SpawnEnemies()
    {
        var levelEnemies = GamePlayManager.Instance.gameLevels[GameManager.Instance.SelectedLevel].enemies;
        for (int i = 0; i < levelEnemies.Count; i++)
        {
            int selectedIndex = Random.Range(0, path.Length);
            int enemyIndex = 0;
            if (levelEnemies.Count > 0)
            {
                enemyIndex = Random.Range(0, levelEnemies.Count);
                if (enemyIndex == 2)
                {
                    int index = enemies.FindIndex(p => p.GetComponent<Character>().eType == EnemyType.Big);
                    if (index != -1)
                    {
                        do
                        {
                            enemyIndex = Random.Range(0, levelEnemies.Count);
                        } while (enemyIndex == 2);
                    }
                }
            }
            GameObject go = Instantiate(Enemy[enemyIndex], path[selectedIndex].startPointTransform.position, Quaternion.identity);
            var position = new Vector3(path[selectedIndex].startPointTransform.position.x, path[selectedIndex].startPointTransform.position.y, path[selectedIndex].transform.position.z);
            //var eType = (EnemyType)Random.Range(0,3);
            go.GetComponent<Character>().SetEnemyProperties(path[selectedIndex].pathMidPoint, levelEnemies[enemyIndex].speed, levelEnemies[enemyIndex].healthRate, position, path[selectedIndex], levelEnemies[enemyIndex].diePoints);
            if (enemiesParent == null)
                enemiesParent = new GameObject("Enemiies").transform;
            go.transform.SetParent(enemiesParent);
            enemies.Add(go);
            await Task.Delay((int)levelEnemies[enemyIndex].enemyDelayTime);
        }

    }


    public void SpawnPlayers(PlayerType pType, int index = 0)
    {
        var player = Instantiate(Player);
        if (enemiesParent == null)
            enemiesParent = new GameObject("Enemiies").transform;
        player.transform.SetParent(enemiesParent);
        player.GetComponent<Character>().pType = pType;
        if (pType == PlayerType.Archerry)
        {
            player.GetComponent<Character>().playerAnimator.enabled = true;
            if (index < archerryPosition.Count) { }
            else
            {
                index = Random.Range(0, archerryPosition.Count);
            }
            player.transform.SetPositionAndRotation(archerryPosition[index].position, archerryPosition[index].rotation);
        }
        else if (pType == PlayerType.Bomber)
        {
            player.transform.SetPositionAndRotation(bomberManPosition.position, bomberManPosition.rotation);
            var myPlayer = player.GetComponent<Character>();
            myPlayer.playerAnimator.enabled = true;
            myPlayer.bomberEndPoint = bomberManEndPoint;
            var bomber = myPlayer.bomb;
            bomber.SetActive(true);
            bomber.GetComponent<BomberMan>().enabled = true;
            bomber.GetComponent<BomberMan>().bombRadius = myPlayer.bomRadius;
            bomber.GetComponent<BomberMan>().player = player.transform;

        }
        else
        {
            player.transform.localScale = Vector3.zero;
            player.GetComponent<Character>().playerAnimator.enabled = false;
        }
    }
}
