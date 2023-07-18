using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChangeAvatar : MonoBehaviour
{
    public GameObject m_Avatar1;
    public GameObject m_Avatar2;

    private GameObject m_ActiveAvatar;
    private GameObject m_InactiveAvatar;

    public PhotonView m_PV;  
    public Player m_OwnPlayer;

    public void Change_Avatar()
    {//변경한 아바타가 모두에게 반영 되어야 함
        m_PV.RPC("Changing", RpcTarget.All);
    }

    void Start()
    {
        m_ActiveAvatar = m_Avatar1;
        m_InactiveAvatar = m_Avatar2;

        m_Avatar2.SetActive(false);

        m_OwnPlayer.Change_Animator(m_ActiveAvatar.GetComponent<Animator>());
    }

    void Update()
    {

    }

    [PunRPC]
    void Changing()
    {
        GameObject TempObj = m_ActiveAvatar;
        m_ActiveAvatar = m_InactiveAvatar;
        m_InactiveAvatar = TempObj;

        m_InactiveAvatar.SetActive(false);
        m_ActiveAvatar.SetActive(true);

        m_OwnPlayer.Change_Animator(m_ActiveAvatar.GetComponent<Animator>());
    }

}
