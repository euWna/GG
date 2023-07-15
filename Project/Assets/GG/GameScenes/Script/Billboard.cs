using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform m_MainCamTransform;
    void Start()
    {
        m_MainCamTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(m_MainCamTransform.position);
    }
}
