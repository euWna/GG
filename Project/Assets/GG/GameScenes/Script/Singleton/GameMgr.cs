using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameMgr : MonoBehaviourPunCallbacks, IPunObservable
{
    public static GameMgr m_Instance = null;

    public GameObject m_LocalPlayer;
    public string m_szPlayerPrefab = "Local_Player";


    public static GameMgr Instance
    {
        get
        {
            if(m_Instance == null)
            {
                m_Instance = FindObjectOfType<GameMgr>();
            }
            return m_Instance;
        }
    }

    public void OnPhotonSerializeView(PhotonStream photonstream, PhotonMessageInfo photonmessageinfo)
    {

    }

    void Awake()
    {
        Debug.LogWarning("호출");
        PhotonNetwork.Instantiate(m_szPlayerPrefab, Vector3.zero, Quaternion.identity);// 현재 isMine인 플레이어 로드

        if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
