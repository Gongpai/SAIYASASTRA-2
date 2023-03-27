using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageFollowWorldSpace : MonoBehaviour
{
    [SerializeField] public Transform lookATransform;

    [SerializeField] public Vector3 offSet;

    [SerializeField] public float m_Set_Offset = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(lookATransform.position + new Vector3(offSet.x, offSet.y + m_Set_Offset, offSet.z));

        if (transform.position != pos)
        {
            transform.position = pos;
        }
    }
}
