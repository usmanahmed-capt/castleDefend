using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Character1 : MonoBehaviour
{
    public Path pathToRun;
    public Transform rayPoint;
    public bool isLiftedUp = false;
    public float speedToMove = 10f;
    Transform endPoint;
    RaycastHit hit;
    float prevY;
    float currentY;
    public float airTime;
    float targetAirTime = 1.5f;
    [SerializeField]
    bool movingUp = false;
    [SerializeField]
    Animator playerAnimator;
    public GameObject deathObject;

    // Start is called before the first frame update
    void Start()
    {
        endPoint = this.pathToRun.endPointTransform;
    }

    // Update is called once per frame
    void Update()
    {
      
        if (!isLiftedUp)
        {
            gameObject.transform.localPosition = Vector3.MoveTowards(transform.position, endPoint.transform.position, speedToMove * Time.deltaTime);

            Debug.Log("End "+IsReachedEnd());
            if(IsReachedEnd())
            {
                playerAnimator.SetBool("HitWall", true);
            }
            else
            {
                playerAnimator.SetBool("HitWall", false);
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
                Debug.Log("Die ->"+ airTime);
                Destroy(gameObject);
            }
            else
            {
                playerAnimator.SetBool("falling", false);

                if (airTime > 0.1f)
                {

                    Debug.Log("Survive -> "+airTime);
                    airTime = 0;
                    prevY = transform.position.y;
                }
            }
        }

        if(gameObject.transform.position.y < this.pathToRun.transform.position.y)
        {
            gameObject.transform.localPosition = new Vector3(transform.position.x, this.pathToRun.transform.position.y+1,this.pathToRun.transform.position.z);
            FindObjectOfType<TouchManager>().selectedObject = null;
        }
    }


    public bool IsGrounded()
    {
        Debug.DrawRay(rayPoint.position, -Vector3.up, Color.yellow);

        if (Physics.Raycast(rayPoint.position, -Vector3.up,.5f))
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
        Debug.DrawRay(rayPoint.position, Vector3.right, Color.red,1f);

        if (Physics.Raycast(rayPoint.position, Vector3.right,out hit, 1f))
        {
            if(hit.collider.tag == "Gate")
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
        if (other.gameObject.tag == "Temple")
        {
            if (GameManager.Instance.conversionInProgress == false)
            {
                Debug.Log("Start Conversion");
                GameManager.Instance.StartConversion();
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Another Conversion in process");
            }
        }
    }

}



