using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIButton : MonoBehaviour
{
    // Start is called before the first frame update
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

    public void Exit_Lobby()
    {
        Debug.Log("Exit Lobby");
        SceneManager.LoadScene("MenuUI");
    }

    public void Exit_Multi()
    {
        Debug.Log("Exit_Multi");
        NetworkManager.Instance.LeaveLobby();
    }

    ///multi///
    public void Multi_EnterCode(InputField In_RoomCode)
    {
        NetworkManager.Instance.JoinRoom(In_RoomCode);
    }
}
