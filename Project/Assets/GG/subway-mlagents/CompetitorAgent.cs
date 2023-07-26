using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Photon.Pun;
using System;

public class CompetitorAgent : Agent
{
    /// <summary>
    /// Variable : Status / Move / Avoid / Hide / Item / Environment 총 6부문별로 구분
    /// </summary>

    //1. Status : Agent 설정
    //public CompetitorAgent agent;

    // agent 시점 카메라
    public Camera agentCamera;

    /// HP <see cref="CharacterStatus.m_fMaxHP"/>
    public const float maxHP = 100f;
    private float hp;

    /// 스태미나 <see cref="CharacterStatus.m_fMaxStamina"/>
    public const float maxStamina = 100f;
    private float stamina;
    private bool staminaIsUsable = true;

    /// 스태미나 소모 속도 (달리기) <see cref="Player.Run"/>
    public const float runStamina = 25f;

    /// 스태미나 회복속도 <see cref="CharacterStatus.m_fSPRecover"/>
    public const float staminaRecover = 10f;


  //2. Move : Agent의 action에 관한 변수
    // Rigidbody
    private Rigidbody agentRb;
    private Animator agentAnimator;

    /// 이동속도 <see cref="Player.m_fSpeed"/>
    public const float moveForce = 50f; //Player.m_fspeed

    /// 회전속도 <see cref="Player.m_fRotateSpeed"/>
    public const float rotationSpeed = 250f;
    private float smoothRotationChange = 0f; // 자연스러운 회전을 위한 계수

    /// 달리기 중 여부 <see cref="Player.m_bIsRun"/>
    private bool isRun;


  //3. Avoid
    // 근접하다고 인식할 반경
    public const float closeRadius = 1f;

    // 충돌했다고 인식할 반경
    public const float collideRadius = 0.5f;

    // 반경 내 Human (AI & Players)
    public CompetitorAgent[] closeAIs;
    public Player[] closePlayers;

    // 반경 내 장애물 (Fall & Obstacle)
    //public GameObject closeFall;
    //public GameObject closeObstacle;


  //4. Hide
    // 지진 이벤트 발생 여부
    public bool eventOccured;

    // 가장 가까이 있는 bunker
    private Bunker nearestBunker;

    // bunker에 숨는 중인지 여부
    private bool isHide;

  //5. Environment
    //Training Mode 여부
    public bool trainingMode = true;

    //Agent가 의도적으로 움직임을 멈춘 상태인지 여부
    private bool frozen = false;

    //Agent가 속해 있는 맵 영역
    //private SubwayArea subwayArea;

    //6. Item



    ///<summary>
    ///<see cref="CharacterStatus"/>
    ///     Status private 변수 접근자 & HP 및 스태미나 관련 함수
    ///</summary>

    public float Get_HP()
    {
        return hp;
    }
    public float Get_Stamina()
    {
        return stamina;
    }

    public void Set_Damage(float fDamage)
    {
        m_PV.RPC("Damaging", RpcTarget.All, fDamage);
    }
    public bool Is_Usable()
    {
        return staminaIsUsable;
    }
    public void Use_Stamina(float fStamina)
    {
        stamina -= runStamina * Time.deltaTime;
        if (0f > stamina)
        {
            stamina = 0;
            staminaIsUsable = false;
        }
    }

    private void Recover_Stamina()
    {
        stamina += staminaRecover * Time.deltaTime;
        if (stamina > 10f)
            staminaIsUsable = true;

        if (stamina > maxStamina)
            stamina = maxStamina;
    }

    [PunRPC]

    private void Damaging(float fDamage)
    {
        hp -= fDamage;
        if (0f >= hp)
        {
            hp = 0;
            this.Set_Dead();
        }
    }

    public void Set_Dead()
    {
        agentAnimator.SetTrigger("Death");
        this.gameObject.SetActive(true);
    }


    private void Update()
    {
        Recover_Stamina();
    }



    ///<summary>
    /// ML agent 상속 함수
    ///</summary>

    /// <summary> Initialize the agent </summary>
    public override void Initialize()
    {
        agentRb = GetComponent<Rigidbody>();
        // subwayArea = GetComponentInParent<SubwayArea>();

        //If not training mode, no max step and keep playing
        if (!trainingMode) MaxStep = 0;
    }

    /// <summary> Reset the agent when an episode begins </summary>
    public override void OnEpisodeBegin()
    {
        if (trainingMode)
        {
            //agent 스폰 위치 설정
            MoveToSafeRandomPosition();

            //낙하물, 아이템 위치 등 리셋 (Hide, Item 개발시)
            //subwayArea.ResetMap();
        }

        //Status 초기화
        hp = maxHP;
        stamina = maxStamina;
        this.gameObject.SetActive(true);

        //Move 초기화
        agentRb.velocity = Vector3.zero;
        agentRb.angularVelocity = Vector3.zero;

    }

    /// <summary> Move agent to a position where agent does not collide with anything </summary>
    private void MoveToSafeRandomPosition()
    {
        bool safePositionFound = false;
        int attemptRemaining = 100;
        Vector3 potentialPosition = Vector3.zero;
        Quaternion potentialRotation = new Quaternion();

        // 시도 횟수 남아있을 동안만 safe position 탐색
        while (!safePositionFound && attemptRemaining > 0)
        {
            attemptRemaining--;

            // 주변 오브젝트와 collider 겹치는지 (즉 충돌하는지) 확인
            Collider[] colliders = Physics.OverlapSphere(potentialPosition, 0.5f);

            // 겹치는 collider 없으면 스폰위치 설정 성공
            safePositionFound = colliders.Length == 0;
        }
    }

    //RaycastSensors로부터 수집되지 않는 정보를 수집함
    public override void CollectObservations(VectorSensor sensor)
    {
        
    }
    /// <summary>
    /// action 명령이 player input이나 NN으로부터 들어오면 호출됨
    /// * 일단 y 벡터 (점프) 제외
    /// actions[0] : move vector x (+1 = right, -1 = left)
    /// actions[1] : move vector z (+1 = forward, -1 = backward)
    /// actions[2] : yaw angle (+1 = turn right, -1 = turn left)
    /// </summary>
    /// <param name="actions"></param>
    public override void OnActionReceived(ActionBuffers actions)
    {
        //Don't take actions if frozen
        if (frozen) return;

        //Calculate movement vector
        Vector3 move = new Vector3(actions.ContinuousActions[0], 0, actions.ContinuousActions[1]);

        //Add force in the direction of the move vector
        agentRb.AddForce(Physics.gravity);
        agentRb.AddForce(move * moveForce);

        //Get the current rotation
        Vector3 rotationVector = transform.rotation.eulerAngles;

        //Caculate yaw rotation
        float rotationChange = actions.ContinuousActions[2];

        //Calculate smooth rotation changes
        smoothRotationChange = Mathf.MoveTowards(smoothRotationChange, rotationChange, 2f * Time.fixedDeltaTime);

        //Calculate new yaw based on smoothed values
        float rotation = rotationVector.y + smoothRotationChange * Time.fixedDeltaTime * rotationSpeed;

        //Apply the new rotation
        transform.rotation = Quaternion.Euler(0, rotation, 0f);
            
    }

    /// <summary>
    /// <see cref="OnActionReceived(ActionBuffers)"에 NN model 대신 키보드 인풋 값을 넘겨줌/>
    /// </summary>
    /// <param name="actionsOut">Output action array</param>
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var action = actionsOut.ContinuousActions;

        action[0] = Input.GetAxis("Horizontal"); //Steering
        action[1] = Input.GetKey(KeyCode.W) ? 1f : 0f; //Acceleration
    }
}
