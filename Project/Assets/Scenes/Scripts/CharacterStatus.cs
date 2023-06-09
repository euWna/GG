using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatus : MonoBehaviour
{
    public Player m_Target;
    public EventUI m_EventUI;
    public float m_fSPRecover;

    public float m_fMaxHP;
    public float m_fMaxStamina;

    private bool m_bIsUsable = true;
    private float m_fHP;
    private float m_fStamina;

    void Start()
    {
        m_fHP = m_fMaxHP;
        m_fStamina = m_fMaxStamina;
    }

    private void Update()
    {
        Recover_Stamina();
    }

    public float Get_HP()
    {
        return m_fHP;
    }
    public float Get_Stamina()
    {
        return m_fStamina;
    }

    public float Get_MaxHP()
    {
        return m_fMaxHP;
    }
    public float Get_MaxStamina()
    {
        return m_fMaxStamina;
    }

    public void Set_Damage(float fDamage)
    {
        m_fHP -= fDamage;
        if(0f >= m_fHP)
        {
            m_fHP = 0;
            m_Target.Set_Dead();
            m_EventUI.Activate_and_Over();
        }
    }
    public bool Is_Usable()
    {
        return m_bIsUsable;
    }
    public void Use_Stamina(float fStamina)
    {
        m_fStamina -= fStamina*Time.deltaTime;
        if(0f> m_fStamina)
        {
            m_fStamina = 0;
            m_bIsUsable = false;
        }
    }

    private void Recover_Stamina()
    {
        m_fStamina += m_fSPRecover*Time.deltaTime;
        if (m_fStamina > 10f)
            m_bIsUsable = true;

        if (m_fStamina > m_fMaxStamina)
            m_fStamina = m_fMaxStamina;
    }
}
