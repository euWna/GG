using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Update()
    {
        if(this.gameObject.transform.localPosition.y <= 2.0f)
        {
            Destroy(this.gameObject);
        }
    }
    
}
