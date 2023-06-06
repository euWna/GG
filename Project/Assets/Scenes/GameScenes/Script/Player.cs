using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public float m_fSpeed = 3f;
    public Transform m_CameraTransform;

    private Vector3 m_vMoveVec;
    private Animator m_Animator;

    private bool m_bIsRun = false;

    void Start()
    {
        m_Animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();   
    }

    private void Get_KeyInput()
    {
        m_vMoveVec = Vector3.zero;
        m_bIsRun = false;
        if(Input.GetKey(KeyCode.W))
        {
            m_vMoveVec += m_CameraTransform.forward;
            m_bIsRun = true;
        }
        else if(Input.GetKey(KeyCode.S))
        {
            m_vMoveVec -= m_CameraTransform.forward;
            m_bIsRun = true;

        }
        if (Input.GetKey(KeyCode.D))
        {
            m_vMoveVec += m_CameraTransform.right;
            m_bIsRun = true;

        }
        else if(Input.GetKey(KeyCode.A))
        {
            m_vMoveVec -= m_CameraTransform.right;
            m_bIsRun = true;

        }
        
        m_vMoveVec = m_vMoveVec.normalized;
        Vector3 vPlayerRight = Vector3.Cross(Vector3.up, m_vMoveVec);
        m_vMoveVec = Vector3.Cross(vPlayerRight, Vector3.up).normalized;

        m_Animator.SetBool("IsRun", m_bIsRun);

    }
    private void Move()
    {
        Get_KeyInput();
        transform.position += m_vMoveVec * m_fSpeed * Time.deltaTime;
        transform.LookAt(transform.position + m_vMoveVec);
    }
}
