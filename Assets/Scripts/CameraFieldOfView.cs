using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFieldOfView : MonoBehaviour
{
    public Collider col;

    void Start()
    {
        Vector3 mPos = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        mPos.z = 0;
        mPos.x += col.bounds.size.x / 2;
        mPos.y += col.bounds.size.y / 2;
        transform.position = mPos;
    }

    void Update()
    {
        
    }
}
