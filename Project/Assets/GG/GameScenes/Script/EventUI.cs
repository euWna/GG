using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate_and_Over()
    {
        gameObject.SetActive(true);
        Invoke("ChangeScene", 3f);
    }
    void ChangeScene()
    {
        SceneManager.LoadScene("Lobby");
    }
}
