using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIButton : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_InputField m_RoomCode;

    private ChangeAvatar m_LocalChangeAvatar;

    void Start()
    {
        
    }

    public void Lobby_Multi()
    {
        Debug.Log("Multi Playing Mode");
        NetworkManager.Instance.ConnectToServer();
    }
    public void Lobby_Single()
    {
        Debug.Log("Single Playing Mode");
        SceneManager.LoadScene("Lobby");
    }
    public void Exit_Game()
    {
        Debug.Log("Exit");

        Application.Quit();
    }

    public void Change_Avatar()
    {
        if(null == m_LocalChangeAvatar)
        {
            m_LocalChangeAvatar = GameMgr.Instance.m_LocalPlayerObj.GetComponentInChildren<ChangeAvatar>();
        }
        m_LocalChangeAvatar.Change_Avatar();
    }

    ///multi///
    public void Exit_Room()
    {
        NetworkManager.Instance.LeaveRoom();
        GameMgr.Instance.Destroy_Player();
        GameMgr.Instance.Destroy_Myself();
    }

    public void Exit_RoomCode()
    {
        Debug.Log("Exit_Multi");
        NetworkManager.Instance.LeaveLobby();
    }
 
    public void Multi_EnterCode()
    {
        NetworkManager.Instance.JoinRoom(m_RoomCode);
    }

    public void Multi_CreateRoom()
    {//나중에 초대 코드로
        NetworkManager.Instance.CreateRoom(m_RoomCode);
    }

    public void Multi_ExitGame()
    {
        Debug.Log("Multi_ExitGame");
        SceneManager.LoadScene("MultiLobby");
    }

}
