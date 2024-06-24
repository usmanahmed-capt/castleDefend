using System.Threading.Tasks;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public Transform TargetObject;
    private Rigidbody rb;

    [SerializeField] float lerpTime = 1f;   // Time it takes to get to the position - acts a speed

    float currentLerpTime;
    bool isLerping;
    Vector3 startPos;
    Vector3 endPos;


    void Awake()
    {
        startPos = transform.position;  // Our current position.  You can update this however.  just examples.
        endPos = transform.position + transform.up;    // I just made this up, it's wherever you want the object to go
    }


    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    bool once;

    async void FixedUpdate()
    {
        //reset when we press spacebar
        if (!once)
        {
            once = true;
            currentLerpTime = 0f;
            isLerping = true;
        }

        if (isLerping)
        {
            //increment timer once per frame
            currentLerpTime += Time.deltaTime;
            if (currentLerpTime > lerpTime)
            {
                currentLerpTime = lerpTime;
                isLerping = false;
            }

            //lerp
            float percentComplete = currentLerpTime / lerpTime;
            this.rb.MovePosition(Vector3.Lerp(startPos, endPos, percentComplete));

            await Task.Delay(100);
            //rb.isKinematic = true;
            MoveTowardsPosition();

        }
    }

    public float arrowMoveSpeed = 3f;
    public void MoveTowardsPosition()
    {
        if(this.TargetObject != null)
        this.rb.MovePosition(this.TargetObject.position);
        //transform.localPosition = Vector3.MoveTowards(transform.position, TargetObject.position, arrowMoveSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        /*if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Hit Enemy");

            Destroy(gameObject, 5f);
        }*/
    }
}
