using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatusUI : MonoBehaviour
{

    public CharacterStatus m_PlayerStatus;

    public TextMeshProUGUI m_HPUI;
    public TextMeshProUGUI m_StaminaUI;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int fMaxHP = (int)m_PlayerStatus.Get_MaxHP();
        int fHP = (int)m_PlayerStatus.Get_HP();
        m_HPUI.text =  "HP "+ fHP.ToString() +" / " + fMaxHP.ToString();

        int fMaxStamina = (int)m_PlayerStatus.Get_MaxStamina();
        int fStamina = (int)m_PlayerStatus.Get_Stamina();
        m_StaminaUI.text = "Stamina " + fStamina.ToString() + " / " + fMaxStamina.ToString();
    }
}
