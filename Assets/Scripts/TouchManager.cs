using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{

    public Rigidbody selectedObject;
    Vector3 offset;
    Vector3 mousePosition;
    public float maxSpeed = 10;
    Vector2 mouseForce;
    Vector3 lastPosition;

    public LayerMask ignoreLayer;
    // Start is called before the first frame update
  
    // Update is called once per frame
    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (selectedObject)
        {
            if (selectedObject.GetComponent<Character>().eType == EnemyType.Big) return;
            selectedObject.transform.position = new Vector3(mousePosition.x, mousePosition.y,selectedObject.GetComponent<Character>().pathToRun.transform.position.z);  
        }

        if (selectedObject)
        {
            mouseForce = (mousePosition - lastPosition) / Time.deltaTime;
            mouseForce = Vector2.ClampMagnitude(mouseForce, maxSpeed);
            lastPosition = mousePosition;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Collider2D targetObject = Physics2D.OverlapPoint(mousePosition, ignoreLayer);
            if (targetObject)
            {
                selectedObject = targetObject.transform.gameObject.GetComponent<Rigidbody>();
                offset = selectedObject.transform.position - mousePosition;
            }
        }
        if (Input.GetMouseButtonUp(0) && selectedObject)
        {
            selectedObject.velocity = Vector2.zero;
            try
            {
                selectedObject.AddForce(mouseForce, ForceMode.Impulse);
            }
            catch (System.Exception)
            {

                
            }
            selectedObject = null;
        }
    }
   
}
