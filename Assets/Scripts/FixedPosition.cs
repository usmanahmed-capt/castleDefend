using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedPosition : MonoBehaviour
{
    private RectTransform m_RectTransform;
    [SerializeField] private float fixedPositionY;
    // Start is called before the first frame update
    void Start()
    {
        m_RectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        var pos = new Vector2(0, fixedPositionY);
        m_RectTransform.localPosition = pos;
    }
}
