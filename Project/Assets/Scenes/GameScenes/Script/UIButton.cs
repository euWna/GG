using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UIButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Lobby_Multi()
    {
        Debug.Log("Multi Playing Mode");
        SceneManager.LoadScene("Lobby");
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
}
