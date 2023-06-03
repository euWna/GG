using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gshake : MonoBehaviour
{
    //How strong is the earthquake?
    public float magnitude; //Not the same magnitude people talk about in an actual earthquakes
    public float slowDownFactor = 0.1f;

    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.position;
    }

    void FixedUpdate()
    {
        Debug.Log(magnitude);

        Vector2 randomPos = Random.insideUnitCircle * magnitude * 40;

        float randomY = Random.Range(-1f, 1f) * magnitude * 40;

        float randomX = Mathf.Lerp(transform.position.x, randomPos.x, Time.deltaTime * slowDownFactor);
        float randomZ = Mathf.Lerp(transform.position.z, randomPos.y, Time.deltaTime * slowDownFactor);

        randomY = Mathf.Lerp(transform.position.y, randomY, Time.deltaTime * slowDownFactor * 0.1f);

        Vector3 moveVec = new Vector3(randomX * 0.6f, randomY * 0.6f, randomZ * 0.6f);
        Vector3 moveVecR = new Vector3(randomX * 1.2f, randomY * 1.2f, randomZ * 1.2f);

        transform.position = originalPosition + moveVec;
        transform.rotation = Quaternion.Euler(moveVecR);
    }
}

