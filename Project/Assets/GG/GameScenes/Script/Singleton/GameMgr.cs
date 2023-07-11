using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : MonoBehaviour
{
    public static GameMgr m_Instance = null;

    public GameObject m_LocalPlayer;

    public static GameMgr Instance
    {
        get
        {
            if(m_Instance == null)
            {
                return null;
            }
            return m_Instance;
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
