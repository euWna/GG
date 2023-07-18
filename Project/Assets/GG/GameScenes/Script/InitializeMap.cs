using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeMap : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameMgr.Instance.Set_PlayerPos();
        GameMgr.Instance.Set_Camera();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
