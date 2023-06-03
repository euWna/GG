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

    public float earthquakeThreshold = 0.5f; //지진이 일어났을 때 판단 기준 : agent의 y값이 0.5이상 변동
    public float searchTime = 2f; //bunker 탐색 제한시간 2초

    private bool earthquakeOccurred;
    private float searchTimer;
    private GameObject nearestBunker;

    public GameObject bunker;

    public bool isHide;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        m_rayPerceptionSensorComponent3D = GetComponentInChildren<RayPerceptionSensorComponent3D>();
        isHide = false;
    }


    private void RayCastInfo(RayPerceptionSensorComponent3D rayComponent)
    // 센서에 감지된 낙하물들과의 거리만 계산하기 위해 만든 함수
    {
        //센서가 감지한 정보들을 RayOutPuts에 받아옴
        var rayOutputs = RayPerceptionSensor
                .Perceive(rayComponent.GetRayPerceptionInput())
                .RayOutputs;
        //감지한 정보(물체)가 있다면
        if (rayOutputs != null)
        {   //센서에 달린 Ray가 여러 개 있으니 Ray는 배열
            var outputLegnth = RayPerceptionSensor
                    .Perceive(rayComponent.GetRayPerceptionInput())
                    .RayOutputs
                    .Length;

            for (int i = 0; i < outputLegnth; i++)
            {
                //충돌(감지)한 오브젝트를 받아옴
                GameObject goHit = rayOutputs[i].HitGameObject;
                if (goHit != null)
                {
                    //충돌한 오브젝트까지의 거리 계산
                    var rayDirection = rayOutputs[i].EndPositionWorld - rayOutputs[i].StartPositionWorld;
                    var scaledRayLength = rayDirection.magnitude;
                    float rayHitDistance = rayOutputs[i].HitFraction * scaledRayLength;

                    //중돌한 오브젝트가 낙하물 && 일정 거리 이내로 들어온 경우(충돌했다고 볼만큼)
                    if(goHit.tag == "Obstacle" && rayHitDistance < 2.4f && isHide == false)
                    {
                        Debug.Log("boom!!!");
                        //리워드 -1
                        AddReward(-1.0f);
                        //에피소드 종료
                        EndEpisode();
                    }
                }
            }
        }
    }

    public void HideNew()
    {
        //진도가 4 이상일 경우에만 숨기
        if(GroundShaker.magnitude >= 4)
        {
            SearchForBunker();
            //벙커를 찾은 경우
            if(nearestBunker != null)
            {
                MoveToBunker();
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
        HideNew();
    }

    public override void OnEpisodeBegin()
    {
        this.transform.localPosition = new Vector3(Random.Range(-19, 7), 0, Random.Range(-19, 7));
        this.transform.localEulerAngles = new Vector3(0, 0, 0);
        bunker.gameObject.transform.localPosition = new Vector3(Random.Range(-16, 2), 0, Random.Range(-16, 2));
        GroundShaker.magnitude = Random.Range(1, 9);
        isHide = false;
        rb.velocity = Vector3.zero;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Hit Wall");
            //벽에 충돌하면 리워드 -0.5
            AddReward(-0.5f);
        }
        else if (collision.gameObject.CompareTag("Goal"))
        {
            Debug.Log("Hit Goal");
            //피니시라인 도달하면 리워드 +1
            AddReward(1.0f);
            //에피소드 종료
            EndEpisode();
        }
        else if (collision.gameObject.CompareTag("Bunker"))
        {
            //숨어있는 상태에서 벙커에 충돌한 경우
            //(단순히 이동중 벙커에 부딫힌 경우와 구분하기 위한 조건임)
            if(isHide == true)
            {
                Debug.Log("Hide");
                //벙커에 충돌(숨으면) 리워드 +1
                AddReward(1.0f);
                //에피소드 종료
                EndEpisode();
            }
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(this.transform.localPosition); // Agent 자신의 위치
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

        // 걸음 수가 많을수록(탈출, 은신이 지연될수록) 리워드 감소
        AddReward(-1f / MaxStep);
    }

    private void MoveToBunker()
    {
        //이동할 벙커와 Agent의 거리 계산
        Vector3 moveDirection = nearestBunker.transform.position - transform.position;
        float dist = Vector3.Distance(this.gameObject.transform.localPosition, nearestBunker.gameObject.transform.localPosition);
        //벙커가 있는 방향으로 이동
        transform.Translate(moveDirection.normalized * Time.deltaTime * 3);
        if (dist < 2f)
        {
            //거리가 일정 범위 안으로 들어왔을 때(도달했다고 볼 만큼 가까울 때)
            //isHide를 true로해 숨어있는 상태 나타냄
            isHide = true;
        }
        
    }

    private void SearchForBunker()
    {
        //플레이어 주변 벙커(콜라이더) 탐색
        Collider[] colliders = Physics.OverlapSphere(transform.position, 5.5f); 

        foreach (Collider collider in colliders)
        {
            //감지된 콜라이더가 벙커일 경우
            if(collider.CompareTag("Bunker"))
            {
                //벙커 발견 완료
                nearestBunker = collider.gameObject;
                Debug.Log("BunkerFInd");
            }

        }
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
