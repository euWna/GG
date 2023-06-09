using UnityEngine;
using Random = UnityEngine.Random;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;



public class HideAgent : Agent {
    public float earthquakeThreshold = 0.5f; //지진이 일어났을 때 판단 기준 : agent의 y값이 0.5이상 변동
    public float searchTime = 2f; //bunker 탐색 제한시간 2초

    Rigidbody agentRb;
    private bool earthquakeOccurred;
    private float searchTimer;
    private GameObject nearestBunker;

    public bool useVectorObs;

    public override void Initialize()
    {
        agentRb = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        //transform.position = new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f));
        transform.position = new Vector3(46, 1, -20);
        agentRb.velocity = Vector3.zero;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        if (useVectorObs)
        {
            sensor.AddObservation(transform.InverseTransformDirection(agentRb.velocity));
        }
    }

    public void MoveAgent(ActionSegment<int> act)
    {
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        var action = act[0];
        switch (action)
        {
            case 1:
                dirToGo = transform.forward * 1f;
                break;
            case 2:
                dirToGo = transform.forward * -1f;
                break;
            case 3:
                rotateDir = transform.up * 1f;
                break;
            case 4:
                rotateDir = transform.up * -1f;
                break;
        }
        transform.Rotate(rotateDir, Time.deltaTime * 200f);
        agentRb.AddForce(dirToGo * 2f, ForceMode.VelocityChange);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // 자신의 y position 변화가 0.5 이상인지 감지하여 지진 여부 확인
        if (!earthquakeOccurred && Mathf.Abs(transform.position.y - 0.5f) > earthquakeThreshold)
        {
            earthquakeOccurred = true;
        }

        if (earthquakeOccurred)
        {
            // Bunker를 탐색하는 동안의 타이머 업데이트
            searchTimer += Time.deltaTime;

            if (searchTimer <= searchTime) //제한 시간 2초 동안
            {
                // bunker 탐색
                SearchForBunker();

                //bunker 못찾으면 뒤돌아서 다시 탐색
                if (nearestBunker == null) {
                    transform.Rotate(0f, 180f, 0f);
                    SearchForBunker();
                }
            }
            else
            {
                if (nearestBunker != null)
                {
                    // 가장 가까운 bunker로 이동
                    MoveToBunker();
                }
                else
                {
                    // Bunker를 찾지 못한 경우, Episode 종료
                    SetReward(-1f);
                    EndEpisode();
                }
            }
        }
    }

    private void MoveToBunker()
    {
        // 가장 가까운 bunker로 이동
        Vector3 moveDirection = nearestBunker.transform.position - transform.position;
        transform.Translate(moveDirection.normalized * Time.deltaTime);

        // Agent의 collider 부피가 Bunker의 collider 내에 80% 이상 포함될 때까지 이동
        Collider agentCollider = GetComponent<Collider>();
        Collider bunkerCollider = nearestBunker.GetComponent<Collider>();

        if (bunkerCollider.bounds.Contains(agentCollider.bounds.center) &&
            agentCollider.bounds.size.magnitude >= bunkerCollider.bounds.size.magnitude * 0.8f)
        {
            // 목표 달성 시 보상 지급 후 Episode 종료
            SetReward(1f);
            EndEpisode();
        }
    }


    private void SearchForBunker()
    {
        // 자신을 중심으로 주변 180도를 탐색하여 가장 가까운 bunker 탐색
        Collider[] colliders = Physics.OverlapSphere(transform.position, LayerMask.GetMask("bunker"));

        float minDistance = Mathf.Infinity;
        foreach (Collider collider in colliders)
        {
            float distance = Vector3.Distance(transform.position, collider.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestBunker = collider.gameObject;
            }
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        if (Input.GetKey(KeyCode.D))
        {
            discreteActionsOut[0] = 3;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            discreteActionsOut[0] = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            discreteActionsOut[0] = 4;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            discreteActionsOut[0] = 2;
        }
    }


}
