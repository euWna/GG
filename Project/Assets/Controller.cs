using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class Controller : Agent
{
    public Rigidbody rb;
    public GameObject Goal;
    public RayPerceptionSensorComponent3D m_rayPerceptionSensorComponent3D;
    public GameObject head;
    public GameObject plane;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        m_rayPerceptionSensorComponent3D = GetComponentInChildren<RayPerceptionSensorComponent3D>();
    }

    private void RayCastInfo(RayPerceptionSensorComponent3D rayComponent)
    {
        var rayOutputs = RayPerceptionSensor
                .Perceive(rayComponent.GetRayPerceptionInput())
                .RayOutputs;

        if (rayOutputs != null)
        {
            var outputLegnth = RayPerceptionSensor
                    .Perceive(rayComponent.GetRayPerceptionInput())
                    .RayOutputs
                    .Length;

            for (int i = 0; i < outputLegnth; i++)
            {
                GameObject goHit = rayOutputs[i].HitGameObject;
                if (goHit != null)
                {
                    var rayDirection = rayOutputs[i].EndPositionWorld - rayOutputs[i].StartPositionWorld;
                    var scaledRayLength = rayDirection.magnitude;
                    float rayHitDistance = rayOutputs[i].HitFraction * scaledRayLength;

                    if(goHit.tag == "Obstacle" && rayHitDistance < 2.4f)
                    {
                        Debug.Log("boom!!!");
                        AddReward(-1.0f);
                        EndEpisode();
                    }
                }
            }
        }
    }
    public float distancePlaneToPoint(Vector3 normal, Vector3 planeDot, Vector3 point)
    {
        Plane plane = new Plane(normal, planeDot);
        return plane.GetDistanceToPoint(point);
    }

    public void Update()
    {
        RayCastInfo(m_rayPerceptionSensorComponent3D);

        //float distToPlane = Vector3.Distance(this.gameObject.transform.localPosition, head.gameObject.transform.localPosition);

        float distToPlane;

        Vector3 normal = plane.transform.up;
        Vector3 planeDot = plane.transform.position;
        Vector3 point = head.transform.position;
        distToPlane = distancePlaneToPoint(normal, planeDot, point);

        Debug.Log(distToPlane);

        if (distToPlane < 2.1f)
        {
            this.transform.localEulerAngles = new Vector3(0, 0, 0);
        }

        /*
        if(this.gameObject.transform.localEulerAngles.x > 25 | this.gameObject.transform.localEulerAngles.x < -25 )
        {
            this.transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        else if(this.gameObject.transform.localEulerAngles.z > 25 | this.gameObject.transform.localEulerAngles.z < -25)
        {
            this.transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        */
        //중심 잡기를 위해 rotation 일정 범위 넘어갈 때 reset하는 식으로 하면 이동이 불가능(회전이 안 되니까)
        //머리랑 plane의 거리를 재서 일정 범위 이내 == 바닥에 넘어지려함 --> 이면 바로 서기
        //-> 자연스럽게 만드는 방법은 연구 필요
    }

    public override void OnEpisodeBegin()
    {
        this.transform.localPosition = new Vector3(Random.Range(-19, 7), 0, Random.Range(-19, 7));
        this.transform.localEulerAngles = new Vector3(0, 0, 0);
        //rb.constraints 로테이션 0,0,0 으로 fix
        //Goal.transform.localPosition = new Vector3(Random.Range(-19, 7), 0, Random.Range(-19, 7));

        rb.velocity = Vector3.zero;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Hit Wall");
            AddReward(-0.5f);
        }
        else if (collision.gameObject.CompareTag("Goal"))
        {
            Debug.Log("Hit Goal");
            AddReward(1.0f);
            EndEpisode();
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(this.transform.localPosition); // 3
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var dir = Vector3.zero;
        var rot = Vector3.zero;

        var action = actions.DiscreteActions[0];
        switch(action)
        {
            case 1:
                dir = transform.forward * 1f;
                break;
            case 2:
                dir = transform.forward * -1f;
                break;
            case 3:
                rot = transform.up * 1f;
                break;
            case 4:
                rot = transform.up * -1f;
                break;
        }
        transform.Rotate(rot, Time.deltaTime * 300f);
        rb.AddForce(dir * 0.5f, ForceMode.VelocityChange);
        //transform.position += dir * 10f * Time.deltaTime;

        AddReward(-1f / MaxStep);
    }
    
    
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        RayCastInfo(m_rayPerceptionSensorComponent3D);

        var discreteActionsOut = actionsOut.DiscreteActions;

        if (Input.GetKey(KeyCode.W))
        {
            discreteActionsOut[0] = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            discreteActionsOut[0] = 2;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            discreteActionsOut[0] = 4;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            discreteActionsOut[0] = 3;
        }
        
    }
}
