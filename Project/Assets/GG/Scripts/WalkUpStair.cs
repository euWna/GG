using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkUpStair : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody m_RigidBody;
    private Animator m_Animator;

    float m_fSpeed = 1.5f;
    void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        m_Animator = GetComponent<Animator>();
        m_Animator.SetBool("IsRun", true);
    }

    // Update is called once per frame
    void Update()
    {
        m_RigidBody.AddForce(transform.forward*m_fSpeed, ForceMode.VelocityChange);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Goal"))
        {
            m_fSpeed *= -1;
        }
    }
}
