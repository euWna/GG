using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    public float m_fDamage = 10f;

    //private bool m_bIsCollided = false;

    private bool m_bQuakeStart = false;
    private Rigidbody m_Rigidbody;

    private Vector3 m_fGravity;

    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_fGravity = Physics.gravity;
    }

    void Update()
    {
        if(m_bQuakeStart)
        {
            m_Rigidbody.AddForce(m_fGravity);
        }
    }

    public float Get_Damage()
    {
        //if (!m_bIsCollided)//이미 맞았던 물체와 또 맞으면 안되니까
        //    return 0;
        //else
        //{
            //m_bIsCollided = true;
            return m_fDamage;
        //}
    }
}
