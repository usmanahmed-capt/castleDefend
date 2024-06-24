using System;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public enum CharacterType
{
    Player, Enemy
};

public enum EnemyType
{
    Smaller, Small, Big
};

public enum PlayerType
{
    Archerry,
    Bomber,
    Repair
};

public class Character : MonoBehaviour
{
    public Path pathToRun;
    public Transform rayPoint;
    public bool isLiftedUp = false;
    public float speedToMove = 10f;
    public float bomberSpeed = 10f;
    Transform endPoint;
    RaycastHit hit;
    internal float prevY;
    internal float currentY;
    public float airTime;
    float targetAirTime = 1.2f;
    [SerializeField]
    internal bool movingUp = false;
    [SerializeField]
    internal Animator playerAnimator;
    public GameObject deathObject;
    public Transform arrowPos;

    public CharacterType cType;
    public PlayerType pType;
    public EnemyType eType;
    public int limitOfArcherries;
    public int bomRadius;
    [SerializeField] private Transform arrowContent;
    public Transform bomberEndPoint;
    [SerializeField] private GameObject archerryArrowPrefab;
    [SerializeField] private GameObject archerryBow;
    [SerializeField] private GameObject additionalArrows;

    public Transform pathPoint;

    public int enemyHealth;

    // Start is called before the first frame update
    void Start()
    {
        if (cType == CharacterType.Enemy)
        {
            endPoint = this.pathToRun.endPointTransform;
        }
    }
    bool isDown;

    public void SetEnemyProperties(Transform pathPoint, float speedToMove, int enemyHealth, Vector3 position, Path pathToRun, int diePoints)
    {
        this.pathPoint = pathPoint;
        this.speedToMove = speedToMove;
        this.enemyHealth = enemyHealth;
        transform.position = position;
        this.pathToRun = pathToRun;
        enemyDiePoints = diePoints;
    }

    // Update is called once per frame
    async void Update()
    {
        if (cType == CharacterType.Enemy)
        {
            playerAnimator.enabled = true;
            if (!isLiftedUp)
            {
                gameObject.transform.localPosition = Vector3.MoveTowards(transform.position, endPoint.transform.position, speedToMove * Time.deltaTime);

                if (IsReachedEnd())
                {
                    if (eType == EnemyType.Small)
                    {
                        playerAnimator.SetBool("HitAttack", true);
                    }
                    else
                    {
                        playerAnimator.SetBool("HitWall", true);
                    }
                }
                else
                {
                    if (eType == EnemyType.Small)
                    {

                        playerAnimator.SetBool("HitAttack", false);
                    }
                    else
                    {
                        playerAnimator.SetBool("HitWall", false);
                    }
                }
            }
            isLiftedUp = IsGrounded();

            if (isLiftedUp)
            {
                playerAnimator.SetBool("falling", true);

                currentY = transform.position.y;
                if (currentY < prevY && FindObjectOfType<TouchManager>().selectedObject == null)
                {
                    movingUp = false;
                    if (!movingUp)
                    {
                        airTime += Time.deltaTime;
                    }
                }
                else
                {
                    movingUp = true;
                    prevY = currentY;
                }
            }
            else
            {
                playerAnimator.SetBool("falling", false);

                if (airTime >= targetAirTime)
                {
                    Instantiate(deathObject, playerAnimator.transform.position, playerAnimator.transform.rotation);
                    //Debug.Log("Die ->" + airTime);
                    SpawnManager.Instance.enemies.Remove(this.gameObject);
                    DataManager.Instance.gData.scorePoints += enemyDiePoints;
                    UIManager.Instance.SetPoints();
                    DataManager.Instance.gData.enemiesKilled += 1;
                    GameManager.Instance.currentLevelKillings += 1;
                    UIManager.Instance.SetCasualties();
                    Destroy(gameObject);
                }
                else
                {
                    playerAnimator.SetBool("falling", false);

                    if (airTime > 0.1f)
                    {

                        airTime = 0;
                        prevY = transform.position.y;
                    }
                }
            }

            if (gameObject.transform.position.y < this.pathToRun.transform.position.y)
            {
                gameObject.transform.localPosition = new Vector3(transform.position.x, this.pathToRun.transform.position.y + 1, this.pathToRun.transform.position.z);
                FindObjectOfType<TouchManager>().selectedObject = null;
            }
        }
        else
        {
            if (pType == PlayerType.Archerry)
            {
                if (limitOfArcherries > 0)
                {
                    var TargetObject = GetTarget();
                    if (TargetObject != null)
                    {
                        playerAnimator.SetBool("ShootArrow", true);
                        archerryBow.SetActive(true);
                        additionalArrows.SetActive(true);
                        if (SpawnManager.Instance.enemies.Count > 2)
                        {

                            var arrow = Instantiate(archerryArrowPrefab, arrowPos.position, arrowPos.rotation).transform;
                            arrow.GetComponent<ArrowController>().TargetObject = TargetObject;
                            arrow.SetParent(arrowContent);
                            limitOfArcherries--;
                            int delayTime = Convert.ToInt32(delayArcherInSeconds * 1000);
                            await Task.Delay(delayTime);
                            limitOfArcherries++;
                        }
                    }
                    else
                    {
                        playerAnimator.SetBool("ShootArrow", true);
                        archerryBow.SetActive(true);
                        additionalArrows.SetActive(true);

                        var arrow = Instantiate(archerryArrowPrefab, arrowPos.position, arrowPos.rotation).transform;
                        arrow.GetComponent<ArrowController>().TargetObject = SpawnManager.Instance.emptyTarget.transform;
                        arrow.SetParent(arrowContent);
                        limitOfArcherries--;
                        int delayTime = Convert.ToInt32(delayArcherInSeconds * 1000);
                        await Task.Delay(delayTime);
                        limitOfArcherries++;
                    }
                }
            }
            else if (pType == PlayerType.Bomber)
            {
                if (!reached)
                {
                    if (playerAnimator != null)
                        playerAnimator.SetBool("WalkBeforeBomb", true);
                    gameObject.transform.localPosition = Vector3.MoveTowards(transform.position, bomberEndPoint.transform.position, bomberSpeed * Time.deltaTime);
                    if (Vector3.Distance(transform.localPosition, bomberEndPoint.transform.position) < 0.2f)
                    {
                        reached = true;
                        if (playerAnimator != null)
                            playerAnimator.SetBool("WalkBeforeBomb", false);
                        Destroy(gameObject);
                    }
                }
            }
            else
            {
                if (!revived)
                {
                    revived = true;
                    DataManager.Instance.gData.templeCurrentHealth += 10;
                    UIManager.Instance.SetTempleHealth();
                    await Task.Delay(1000);
                    revived = false;
                }
            }
        }
    }

    private Transform GetTarget()
    {
        var enemies = SpawnManager.Instance.enemies;
        GameObject target = null;
        var eIndex = Random.Range(0, enemies.Count);
        //if (Vector3.Distance(enemies[eIndex].transform.position, transform.position) < 2)
        //{}
        if (enemies.Count > 0)
            target = enemies[eIndex];
        return target.transform;
    }

    private bool revived;

    private bool reached = false;
    [SerializeField] private float delayArcherInSeconds;
    public GameObject bomb;

    public bool IsGrounded()
    {
        Debug.DrawRay(rayPoint.position, -Vector3.up, Color.yellow);

        if (Physics.Raycast(rayPoint.position, -Vector3.up, 1f))
        {

            return false;

        }
        else
        {
            return true;
        }
    }

    public bool IsReachedEnd()
    {
        Debug.DrawRay(rayPoint.position, Vector3.right, Color.red, 1f);

        if (Physics.Raycast(rayPoint.position, Vector3.right, out hit, 1f))
        {
            if (hit.collider.tag == "Gate")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    private void OnMouseDown()
    {
        FindObjectOfType<TouchManager>().selectedObject = gameObject.GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (cType == CharacterType.Enemy)
        {
            if (other.gameObject.tag == "Temple")
            {
                if (DataManager.Instance.gData.templePurchased)
                {
                    if (GameManager.Instance.conversionInProgress == false)
                    {
                        GameManager.Instance.StartConversion();
                        SpawnManager.Instance.enemies.Remove(this.gameObject);
                        DataManager.Instance.gData.scorePoints += enemyDiePoints;
                        UIManager.Instance.SetPoints();
                        DataManager.Instance.gData.enemiesKilled += 1;
                        GameManager.Instance.currentLevelKillings += 1;
                        UIManager.Instance.SetCasualties();
                        Destroy(gameObject);
                    }
                    else
                    {
                        transform.localPosition = pathPoint.position;
                    }
                }
                else
                {
                    transform.localPosition = pathPoint.position;
                }
            }
            else if (other.gameObject.tag == "ResetPoint")
            {
                transform.localPosition = pathPoint.position;
            }
        }
    }
    public int enemyDiePoints;


    private void OnCollisionEnter(Collision collision)
    {
        if (cType == CharacterType.Enemy)
        {
            if (collision.gameObject.CompareTag("Arrow"))
            {
                if (enemyHealth <= 0)
                {
                    Instantiate(deathObject, playerAnimator.transform.position, playerAnimator.transform.rotation);
                    SpawnManager.Instance.enemies.Remove(this.gameObject);
                    DataManager.Instance.gData.scorePoints += enemyDiePoints;
                    UIManager.Instance.SetPoints();
                    DataManager.Instance.gData.enemiesKilled += 1;
                    GameManager.Instance.currentLevelKillings += 1;
                    UIManager.Instance.SetCasualties();
                    Destroy(gameObject);
                }
                else
                {
                    enemyHealth -= 5;
                    collision.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    collision.gameObject.GetComponent<Collider>().enabled = false;
                    if (enemyHealth <= 0)
                    {
                        Instantiate(deathObject, playerAnimator.transform.position, playerAnimator.transform.rotation);
                        SpawnManager.Instance.enemies.Remove(this.gameObject);
                        DataManager.Instance.gData.scorePoints += enemyDiePoints;
                        UIManager.Instance.SetPoints();
                        DataManager.Instance.gData.enemiesKilled += 1;
                        GameManager.Instance.currentLevelKillings += 1;
                        UIManager.Instance.SetCasualties();
                        Destroy(gameObject);
                    }
                }
                Destroy(collision.gameObject, 0.2f);
            }
        }
    }
    private bool doorKnocked;

    private async void OnTriggerStay(Collider collision)
    {
        if (collision.CompareTag("KnockDoor"))
        {
            if (!UIManager.Instance.levelFailed)
            {
                if (!doorKnocked)
                {
                    doorKnocked = true;
                    if (this.CompareTag(UTILS.ENEMY_SMALLER))
                    {
                        if (DataManager.Instance.gData.templeCurrentHealth >= 10)
                            DataManager.Instance.gData.templeCurrentHealth -= 10;
                        else
                        {
                            UIManager.Instance.levelFailed = true;
                            UIManager.Instance.SetActiveFailedScreen();
                        }
                    }
                    else if (this.CompareTag(UTILS.ENEMY_SMALL))
                    {
                        if (DataManager.Instance.gData.templeCurrentHealth >= 15)
                            DataManager.Instance.gData.templeCurrentHealth -= 15;
                        else
                        {
                            UIManager.Instance.levelFailed = true;
                            UIManager.Instance.SetActiveFailedScreen();
                        }
                    }
                    else if (this.CompareTag(UTILS.ENEMY_BIG))
                    {
                        if (DataManager.Instance.gData.templeCurrentHealth >= 30)
                            DataManager.Instance.gData.templeCurrentHealth -= 30;
                        else
                        {
                            UIManager.Instance.levelFailed = true;
                            UIManager.Instance.SetActiveFailedScreen();
                        }
                    }
                    UIManager.Instance.SetTempleHealth();
                    await Task.Delay(1500);
                    doorKnocked = false;
                }
            }
        }
    }
}



