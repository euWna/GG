using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class StatusBar : MonoBehaviour
{
    public Image m_HPImage;

    public Status m_PlayerStatus;

    
    private float m_fLeft;
    void Start()
    {
       
        m_fLeft = 13;
    }

    // Update is called once per frame
    void Update()
    {
       
        float fRatio = m_PlayerStatus.Get_HP() / m_PlayerStatus.Get_MaxHP();

        //Debug.Log("Right : " + m_HPImage.rectTransform.offsetMin.x + " fRatio: "+fRatio);

        m_HPImage.rectTransform.offsetMin = new Vector2(m_fLeft * (1f- fRatio), m_HPImage.rectTransform.offsetMax.y);
    }

   
}
