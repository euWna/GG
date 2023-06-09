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
        //진도 4 이상일 때만 물체 추락
        if( //(GroundShaker.magnitude >= 4) &&
            (collider.gameObject.CompareTag("AI") || collider.gameObject.CompareTag("Player")))
        {
            Debug.Log("Falling Trigger!");
            foreach(GameObject obj in FallObject)
            {
                //Destroy(obj.GetComponent<ConfigurableJoint>());
                obj.GetComponent<ConfigurableJoint>().zMotion
                = obj.GetComponent<ConfigurableJoint>().yMotion
                //= obj.GetComponent<ConfigurableJoint>().xMotion
                = ConfigurableJointMotion.Free;
            }
        }
    }
}
