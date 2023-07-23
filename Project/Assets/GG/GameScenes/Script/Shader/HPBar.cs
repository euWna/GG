using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class HPBar : MonoBehaviour
{
    public Image m_Image;
    public CharacterStatus m_Status;
   
    Transform m_MainCamTransform;
    float m_fHPRatio;

    void Start()
    {
        m_MainCamTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        m_fHPRatio = m_Status.Get_HP() / m_Status.Get_MaxHP();
        m_Image.fillAmount = m_fHPRatio;   
    }
    //쉐이더 변수 값 지정하는 타이밍. 만약에 일반 hlsl과 같다면 같은 쉐이더를 공유한다면 문제 발생
    //
    private void OnPreRender()
    {
        //m_Image.material.SetFloat("g_fRatio", m_fHPRatio);
    }

}
