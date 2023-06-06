using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playercamera : MonoBehaviour
{
    public Transform m_PlayerTransform;
    public Vector3 m_vOffset;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = m_PlayerTransform.position + m_vOffset;

        Vector3 vLookatPosition = m_PlayerTransform.position + new Vector3(0f, 1.5f, 0f);
        transform.LookAt(vLookatPosition);
    }
}
