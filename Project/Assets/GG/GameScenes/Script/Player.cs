using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public float m_fSpeed;
    public Transform m_CameraTransform;
    public EventUI m_ClearUI;

    private Rigidbody m_Rigidbody;
    private Vector3 m_vMoveVec;
    private Animator m_Animator;

    private bool m_bIsRun = false;
    private bool m_bIsSprint = false;

    private float m_fTotalSpeed;

    private CharacterStatus m_Status;

    void Start()
    {
        m_Animator = GetComponentInChildren<Animator>();
        m_Status = GetComponentInChildren<CharacterStatus>();
        m_Rigidbody = GetComponent<Rigidbody>();
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
        if (Input.GetKey(KeyCode.W))
        {
            m_vMoveVec += m_CameraTransform.forward;
            m_fTotalSpeed = m_fSpeed;
            m_bIsRun = true;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            m_vMoveVec -= m_CameraTransform.forward;
            m_fTotalSpeed = m_fSpeed;
            m_bIsRun = true;

        }
        if (Input.GetKey(KeyCode.D))
        {
            m_vMoveVec += m_CameraTransform.right;
            m_fTotalSpeed = m_fSpeed;
            m_bIsRun = true;

        }
        else if (Input.GetKey(KeyCode.A))
        {
            m_vMoveVec -= m_CameraTransform.right;
            m_fTotalSpeed = m_fSpeed;
            m_bIsRun = true;

        }

        if(Input.GetKey(KeyCode.O))
        {
            m_Status.Set_Damage(100f);
        }

        m_vMoveVec = m_vMoveVec.normalized;
        Vector3 vPlayerRight = Vector3.Cross(Vector3.up, m_vMoveVec);
        m_vMoveVec = Vector3.Cross(vPlayerRight, Vector3.up).normalized;

        m_Animator.SetBool("IsRun", m_bIsRun);

    }
    public void Set_Dead()
    {
        m_Animator.SetTrigger("Death");
    }
    private void Move()
    {
        m_fTotalSpeed = 0f;
        Get_KeyInput();
        Run();
        //transform.position += m_vMoveVec * m_fTotalSpeed * Time.deltaTime;
        transform.LookAt(transform.position + m_vMoveVec);
        m_Rigidbody.AddForce(m_vMoveVec * m_fTotalSpeed);
        m_Rigidbody.AddForce(Physics.gravity);
        m_Rigidbody.angularVelocity = new Vector3(0f, 0f, 0f);

        if (Mathf.Abs(m_Rigidbody.velocity.x) > m_fTotalSpeed)
        {
            m_Rigidbody.velocity = new Vector3(Mathf.Sign(m_Rigidbody.velocity.x) * m_fTotalSpeed, m_Rigidbody.velocity.y, m_Rigidbody.velocity.z);
        }
        else if (Mathf.Abs(m_Rigidbody.velocity.z) > m_fTotalSpeed)
        {
            m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_Rigidbody.velocity.y, Mathf.Sign(m_Rigidbody.velocity.z) * m_fTotalSpeed);
        }
        else if (m_Rigidbody.velocity.y > m_fTotalSpeed)
        {
            m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, Mathf.Sign(m_Rigidbody.velocity.y) * m_fTotalSpeed, m_Rigidbody.velocity.z);
        }
    }

    private void Run()
    {
        m_bIsSprint = false;
        if (m_bIsRun && m_Status.Is_Usable())
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                m_bIsSprint = true;
                m_fTotalSpeed *= 1.5f;
                m_Status.Use_Stamina(25f);

            }
        }
        m_Animator.SetBool("IsSprint", m_bIsSprint);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("stair"))
        {
            m_Rigidbody.AddForce(transform.forward * m_fTotalSpeed * 0.4f, ForceMode.VelocityChange);
        }
        else if (collision.gameObject.CompareTag("Obstacle"))//낙하물 충돌
        {
            float fDamage = collision.gameObject.GetComponent<FallingObject>().Get_Damage();
            m_Status.Set_Damage(fDamage);
        }
        else if (collision.gameObject.CompareTag("Goal"))
        {
            m_ClearUI.Activate_and_Over();
        }
    }
}
