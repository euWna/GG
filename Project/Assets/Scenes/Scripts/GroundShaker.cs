using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundShaker : MonoBehaviour
{
    //How strong is the earthquake?
    public static float magnitude = 4; //Not the same magnitude people talk about in an actual earthquakes
    public float slowDownFactor = 0.1f;
    public GameObject Target;

    private Vector3 originalPosition;


    void Start()
    {
        //Debug.Log("start position");
        originalPosition = Target.transform.localPosition;
        //Debug.Log(originalPosition);

        magnitude = Random.Range(1, 8);
        Debug.Log(magnitude);
    }

    void FixedUpdate()
    {
        //Debug.Log(magnitude);

        Vector2 randomPos = Random.insideUnitCircle * magnitude;

        float randomY = Random.Range(-1f, 1f) * magnitude;

        float randomX = Mathf.Lerp(transform.localPosition.x, randomPos.x, Time.deltaTime * slowDownFactor); ;
        float randomZ = Mathf.Lerp(transform.localPosition.z, randomPos.y, Time.deltaTime * slowDownFactor);

        randomY = Mathf.Lerp(transform.localPosition.y, randomY, Time.deltaTime * slowDownFactor * 0.1f);

        Vector3 moveVec = new Vector3(randomX, randomY, randomZ);

        transform.localPosition = originalPosition + moveVec;
        transform.localRotation = Quaternion.Euler(moveVec);

        //Debug.Log(transform.localPosition);
    }
}
