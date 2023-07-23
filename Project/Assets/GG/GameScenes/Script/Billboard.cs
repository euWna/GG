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
        if(null == m_MainCamTransform)
            m_MainCamTransform = Camera.main.transform;

        transform.LookAt(m_MainCamTransform.position);
       // transform.rotation = Quaternion.Euler(0f, 180f, 0f);
    }
}
