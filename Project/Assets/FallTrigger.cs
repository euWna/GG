using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallTrigger : MonoBehaviour
{
    public GameObject[] FallObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.CompareTag("AI") || collider.gameObject.CompareTag("Player"))
        {
            Debug.Log("Falling Trigger!");
            foreach(GameObject obj in FallObject)
            {
                obj.GetComponent<ConfigurableJoint>().yMotion = ConfigurableJointMotion.Free;
            }
        }
    }
}
