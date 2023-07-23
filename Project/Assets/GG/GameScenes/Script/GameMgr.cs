using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;

public class GameMgr : MonoBehaviourPunCallbacks, IPunObservable
{
    public static GameMgr m_Instance = null;

    public Player m_LocalPlayer;
    public GameObject m_LocalPlayerObj;
    public string m_szPlayerPrefab = "Local_Player";


    void Awake()
    {
        Debug.LogWarning("호출");
        PhotonNetwork.Instantiate(m_szPlayerPrefab, Vector3.zero, Quaternion.identity);

        var duplicated = FindObjectsOfType<GameMgr>();

        if (duplicated.Length > 1)
        {
            Destroy(this.gameObject);
        }
        else if (null == m_Instance)
        {
            m_Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

    }

    void start()
    {

    }
    public static GameMgr Instance
    {
        get
        {
            if (null == m_Instance)
            {
                return null;
            }
            return m_Instance;
        }
    }
    public void Set_PlayerPos()
    {
        if (null != m_LocalPlayerObj)
            m_LocalPlayerObj.transform.position = Vector3.zero;
    }
    public void Destroy_Player()
    {
        Destroy(m_LocalPlayerObj);
    }

    public void Destroy_Myself()
    {
        Destroy(this.gameObject);
    }
    public void Set_Camera()
    {

        CinemachineVirtualCamera Temp = FindObjectOfType<CinemachineVirtualCamera>();
        if(null!=Temp && null != m_LocalPlayer)
            m_LocalPlayer.Set_Camera(Temp);
        
    }

    public void OnPhotonSerializeView(PhotonStream photonstream, PhotonMessageInfo photonmessageinfo)
    {

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
