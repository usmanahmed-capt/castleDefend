using UnityEngine;

public class BomberMan : MonoBehaviour
{
    public Transform player;
    public float bombRadius;
    // Start is called before the first frame update
    void Start()
    {
        bombParticlePrefab = RefManager.Instance.bombParticlePrefab;
    }

    public bool bombBlast;
    private GameObject bombParticlePrefab;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100))
            {
                print("Raycast hit");
                if (hit.collider != null)
                {
                    print("Collider is not null");
                    if (hit.collider.CompareTag("Player"))
                    {
                        print("Collider hit on player");
                        if (hit.collider.gameObject.GetComponent<Character>().pType == PlayerType.Bomber)
                        {
                            bombBlast = true;
                        }
                        else { bombBlast = false; }
                    }
                    else { bombBlast = false; }
                }
                else
                {
                    bombBlast = false;
                }
            }
        }

        if (bombBlast)
        {
            bombBlast = false;
            var bombParticle = Instantiate(bombParticlePrefab);
            bombParticle.transform.localPosition = player.position;
            var overlapColliders = Physics.OverlapSphere(transform.position, bombRadius);
            foreach (var col in overlapColliders)
            {
                if (col.CompareTag("Enemy"))
                {
                    print("enemies");
                    var ch = col.GetComponent<Character>();
                    var deathObj = Instantiate(ch.deathObject, ch.playerAnimator.transform.position, ch.playerAnimator.transform.rotation);
                    SpawnManager.Instance.enemies.Remove(col.gameObject);
                    DataManager.Instance.gData.scorePoints += 200;
                    UIManager.Instance.SetPoints();
                    DataManager.Instance.gData.enemiesKilled += 1;
                    GameManager.Instance.currentLevelKillings += 1;
                    UIManager.Instance.SetCasualties();
                    Destroy(col.gameObject);
                }
                else if (col.CompareTag("Player"))
                {
                    if (col.GetComponent<Character>().pType == PlayerType.Bomber)
                    {
                        Destroy(col.gameObject);
                    }
                }
            }
            Destroy(bombParticle, 2f);
            Destroy(this.gameObject, 2);
        }
    }
}
