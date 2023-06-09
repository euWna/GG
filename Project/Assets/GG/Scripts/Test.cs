using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class Test : MonoBehaviour
{
    public float speed = 10.0f;
    public Rigidbody rb;
    public Transform obstacle;
    public Component temp;
    public GameObject hit;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        hit = GetComponentInChildren<RayPerceptionOutput.RayOutput>().HitGameObject;
        if (hit.gameObject.tag == "obstacle")
        {
            Debug.Log("obstacle");
        }
        else if (hit.gameObject.tag == "wall")
        {
            Debug.Log("wall");
        }
    }
}
