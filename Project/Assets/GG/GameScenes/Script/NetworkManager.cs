using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    private static NetworkManager m_Instance = null;
    public const int m_iMaxPlayer = 8;


    public TMP_InputField m_RoomCode, m_NickNameInput;

    void Awake()
    {
        var duplicated = FindObjectsOfType<NetworkManager>();

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
    public static NetworkManager Instance
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

    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(960, 600, false);

    }
    void Update()
    {
       // Debug.Log(PhotonNetwork.NetworkClientState.ToString());
    }
    //////////////////////////////////////////////////////////////

    public void ConnectToServer()
    {
        PhotonNetwork.ConnectUsingSettings();

        //바로 서버 연결
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("서버접속완료");
        PhotonNetwork.LocalPlayer.NickName = "";//플레이어 이름 임의로
        JoinLobby();
    }

    private void JoinLobby() => PhotonNetwork.JoinLobby();
    public override void OnJoinedLobby()
    {
        Debug.Log("로비접속완료");
        SceneManager.LoadScene("RoomCode");
    }




    public void CreateRoom(TMP_InputField In_RoomCode)
    {
        m_RoomCode = In_RoomCode;
        PhotonNetwork.CreateRoom(m_RoomCode.text, new RoomOptions { MaxPlayers = m_iMaxPlayer });
    }
    public override void OnCreatedRoom()
    {
        Debug.Log("방 생성");
    }

    public void JoinRoom(TMP_InputField In_RoomCode)
    {
        m_RoomCode = In_RoomCode;
        PhotonNetwork.JoinRoom(m_RoomCode.text);
    }
    public override void OnJoinedRoom()
    {
        //모든 접속하는 플레이어에 대해서 실행
        Debug.Log("방 입장");
        PhotonNetwork.LoadLevel("MultiLobby");
        
    }
    
    public void StartGame(string In_StageName )
    {
        PhotonNetwork.LoadLevel(In_StageName);
    }

    public void LeaveRoom()
    {
        m_RoomCode = null;
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        Debug.Log("방 나감");
    }
//======================================================
    public void LeaveLobby()
    {
        Debug.Log("로비 나가는 중");
        PhotonNetwork.LeaveLobby();
    }
    public override void OnLeftLobby()
    {
        Debug.Log("로비나감");
        Disconnect();
    }

    

    private void Disconnect() => PhotonNetwork.Disconnect();
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("연결끊김");
        SceneManager.LoadScene("MenuUI");
    }



    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("방입장실패");
        JoinLobby();
    }

    [ContextMenu("정보")]
    void Info()
    {
        if (PhotonNetwork.InRoom)
        {
            print("현재 방 이름 : " + PhotonNetwork.CurrentRoom.Name);
            print("현재 방 인원수 : " + PhotonNetwork.CurrentRoom.PlayerCount);
            print("현재 방 최대인원수 : " + PhotonNetwork.CurrentRoom.MaxPlayers);

            string playerStr = "방에 있는 플레이어 목록 : ";
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) playerStr += PhotonNetwork.PlayerList[i].NickName + ", ";
            print(playerStr);
        }
        else
        {
            print("접속한 인원 수 : " + PhotonNetwork.CountOfPlayers);
            print("방 개수 : " + PhotonNetwork.CountOfRooms);
            print("모든 방에 있는 인원 수 : " + PhotonNetwork.CountOfPlayersInRooms);
            print("로비에 있는지? : " + PhotonNetwork.InLobby);
            print("연결됐는지? : " + PhotonNetwork.IsConnected);
        }
    }
}
