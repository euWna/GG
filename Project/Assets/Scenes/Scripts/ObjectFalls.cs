using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectFalls : MonoBehaviour
{
    public GameObject obstacle;
    public  float obstacle_pos_x;
    public  float obstacle_pos_y;
    public  float obstacle_pos_z;

    void Start()
    {
         InvokeRepeating("CreateObs", 0, 0.6f);
    }
   
    public void CreateObs()
    {
        obstacle_pos_x = Random.Range(-18.0f, 6.0f);
        obstacle_pos_z = Random.Range(-18.0f, 6.0f);
        Instantiate(obstacle, new Vector3(obstacle_pos_x, 15, obstacle_pos_z), Quaternion.identity);

    }
}
