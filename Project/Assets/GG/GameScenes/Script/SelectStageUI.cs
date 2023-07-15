using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SelectStageUI : MonoBehaviour
{

    public TextMeshProUGUI SelectedStage;
    public Image mapImage;
    public Sprite subway, apart;
    
    // Start is called before the first frame update

    private int m_iStageIndex = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Game_Start()
    {
        SceneManager.LoadScene(SelectedStage.text);
    }

    //멀티 게임 임시
    public void Temp_StartGame()
    {
        SceneManager.sceneLoaded += NetworkManager.Instance.OnSceneLoaded;
        SceneManager.LoadScene("Multi_Subway");
    }
    public void Exit_Stage()
    {
        SceneManager.LoadScene("Lobby");
    }

    public void Change_Stage()
    {
        m_iStageIndex = (m_iStageIndex + 1) % 2;
        switch(m_iStageIndex)
        {
            case 0:
                SelectedStage.text = "Multi_Subway";//잠깐 멀티로
                mapImage.sprite = subway;

                break;
            case 1:
                SelectedStage.text = "Apartment";
                mapImage.sprite = apart;
                break;
        }
        //SelectedStage.text = sz_SelectedStage;
    }
}
