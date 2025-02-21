using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;


//プレイヤーステート用
enum PlayerStatus
{
    Idle,
    Walk,
    Run,
    Trun_Right,
    Trun_Left,
    Attack,
    Damage,
    Death,
    Camera_Shot,
    Clear,
}

public class PlayerScript : MonoBehaviour
{
    [SerializeField] GameObject m_cameraObject;
    [SerializeField] GameObject m_subcameraObject;
    [SerializeField] GameObject m_enemyObject;
    [SerializeField] GameObject m_handLight;
    [SerializeField] GameObject m_gameClearObject;
    [SerializeField] AudioClip m_asiotoSE;
    [SerializeField] AudioClip m_bressSE;

    //カメラスクリプト用変数
    MainCamera m_cameraScript;
    //ゲームクリア用変数
    GameClearScript m_gameClearScript;
    //アニメーション用変数
    public Animator m_animator;        // アニメーター
    //プレイヤーの移動用変数
    private float m_speed = 3.0f;       // Walking speed
    private float m_runSpeed = 6.0f;    // Run speed
    float m_clearSpeed = 8.0f;          // ClearRun speed
    private float m_gravity = 9.81f;    //gravity
    private CharacterController m_controller;
    private Vector3 m_moveDirection = Vector3.up;
    //初期配置用変数
    Vector3 m_initPos = new Vector3(21.0f,12.0f,5.0f);
    //ステータス用変数
    PlayerStatus m_playerAnimStatus = PlayerStatus.Idle;//アニメーションステート
    //ゲームクリア用変数
    public Vector3 m_dest;
    //オーディオソース用変数
    public AudioSource m_audioSource;

    // Start is called before the first frame update
    void Start()
    {
        m_gameClearScript = m_gameClearObject.GetComponent<GameClearScript>();
        m_animator = GetComponent<Animator>();
        m_controller = GetComponent<CharacterController>();
        m_cameraScript = Camera.main.GetComponent<MainCamera>();
        m_audioSource = GetComponent<AudioSource>();
    }

    //初期設定用メソッド
    public void doInit()
    {
        //初期配置
        transform.position = m_initPos;
    }

    //プレイヤーの移動処理メソッド
    public void doMove()
    {
        if(m_playerAnimStatus == PlayerStatus.Death)
        {
            return;
        }
        //入力    
        float vert = Input.GetAxis("Vertical");
        float horiz = Input.GetAxis("Horizontal");
        m_moveDirection = new Vector3(horiz,0.0f,vert);
        //プレイヤーの移動処理
        if (m_controller.isGrounded)
        {
            if (m_moveDirection.magnitude > 0.1f)
            {
                m_moveDirection = transform.TransformDirection(m_moveDirection);
                if(m_cameraScript.cameraStatus == CameraStatus.Third_Parson)
                {
                    //3人称視点の移動
                    //ダッシュ
                    if (Input.GetKey("left shift"))
                    {
                        m_playerAnimStatus = PlayerStatus.Run;
                        m_moveDirection *= m_runSpeed;
                    }
                    //歩き
                    else
                    {
                        m_playerAnimStatus = PlayerStatus.Walk;
                        m_moveDirection *= m_speed;
                    }

                }
                else
                {
                    //1人称視点の移動
                    if (Input.GetKey("a") || Input.GetKey("d"))
                    {
                        transform.RotateAround(transform.position, Vector3.up, horiz * Time.deltaTime);
                    }
                    //歩き
                    else
                    {
                        m_playerAnimStatus = PlayerStatus.Walk;
                        m_moveDirection *= m_speed;                    
                    }
                    //方向転換
                    if (Input.GetKey("a") || Input.GetKey("d"))
                    {
                        transform.RotateAround(transform.position, Vector3.up, horiz * Time.deltaTime);
                    }
                }
            }
            else
            {
                m_playerAnimStatus = PlayerStatus.Idle;
            }
        }

        //重力処理
        m_moveDirection.y -= m_gravity;

        //最終処理
        m_controller.Move(m_moveDirection * Time.deltaTime);
    }

    //プレイヤーのHPが0以下になった処理メソッド
    public void doDead()
    {
        m_playerAnimStatus = PlayerStatus.Death;
        m_handLight.SetActive(false);
        m_cameraScript.doGameOverCamera();
        //敵のデリート処理
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in objects)
        {
            Destroy(enemy);
        }
    }
    //アニメーション
    public void doAnimStatus()
    {
        switch (m_playerAnimStatus)
        {
            case PlayerStatus.Idle:
                //アニメーション
                m_animator.SetBool("runFlag", false);
                m_animator.SetBool("walkFlag", false);
                m_animator.SetBool("idleFlag", true);
                m_animator.SetBool("turn_rightFlag", false);
                m_animator.SetBool("turn_leftFlag", false);
                m_animator.SetBool("damageFlag", false);
                m_animator.SetBool("deathFlag", false);
                m_animator.SetBool("camera_shotFlag", false);
                if (m_audioSource.isPlaying)
                {
                    m_audioSource.Stop();
                }
                break;
            case PlayerStatus.Walk:
                //アニメーション
                m_animator.SetBool("runFlag", false);
                m_animator.SetBool("walkFlag", true);
                m_animator.SetBool("idleFlag", false);
                m_animator.SetBool("turn_rightFlag", false);
                m_animator.SetBool("turn_leftFlag", false);
                m_animator.SetBool("damageFlag", false);
                m_animator.SetBool("deathFlag", false);
                m_animator.SetBool("camera_shotFlag", false);
                //SE
                m_audioSource.pitch = 1.0f;
                if (!m_audioSource.isPlaying)
                {
                    m_audioSource.Play();
                }
                break;
            case PlayerStatus.Run:
                //アニメーション
                m_animator.SetBool("runFlag", true);
                m_animator.SetBool("walkFlag", false);
                m_animator.SetBool("idleFlag", false);
                m_animator.SetBool("turn_rightFlag", false);
                m_animator.SetBool("turn_leftFlag", false);
                m_animator.SetBool("damageFlag", false);
                m_animator.SetBool("deathFlag", false);
                m_animator.SetBool("camera_shotFlag", false);
                //SE
                m_audioSource.pitch = 1.8f;
                if (!m_audioSource.isPlaying)
                {
                    m_audioSource.Play();
                }
                break;
            case PlayerStatus.Trun_Right:
                //アニメーション
                m_animator.SetBool("runFlag", false);
                m_animator.SetBool("walkFlag", false);
                m_animator.SetBool("idleFlag", false);
                m_animator.SetBool("turn_rightFlag", true);
                m_animator.SetBool("turn_leftFlag", false);
                m_animator.SetBool("damageFlag", false);
                m_animator.SetBool("deathFlag", false);
                m_animator.SetBool("camera_shotFlag", false);
                break;
            case PlayerStatus.Trun_Left:
                //アニメーション
                m_animator.SetBool("runFlag", false);
                m_animator.SetBool("walkFlag", false);
                m_animator.SetBool("idleFlag", false);
                m_animator.SetBool("turn_rightFlag", false);
                m_animator.SetBool("turn_leftFlag", true);
                m_animator.SetBool("damageFlag", false);
                m_animator.SetBool("deathFlag", false);
                m_animator.SetBool("camera_shotFlag", false);
                break;
            case PlayerStatus.Damage:
                //アニメーション
                m_animator.SetBool("runFlag", false);
                m_animator.SetBool("walkFlag", false);
                m_animator.SetBool("idleFlag", false);
                m_animator.SetBool("turn_rightFlag", false);
                m_animator.SetBool("turn_leftFlag", false);
                m_animator.SetBool("damageFlag", true);
                m_animator.SetBool("deathFlag", false);
                m_animator.SetBool("camera_shotFlag", false);
                if (m_audioSource.isPlaying)
                {
                    m_audioSource.Stop();
                }
                break;
            case PlayerStatus.Death:
                //アニメーション
                m_animator.SetBool("runFlag", false);
                m_animator.SetBool("walkFlag", false);
                m_animator.SetBool("idleFlag", false);
                m_animator.SetBool("turn_rightFlag", false);
                m_animator.SetBool("turn_leftFlag", false);
                m_animator.SetBool("damageFlag", false);
                m_animator.SetBool("deathFlag", true);
                m_animator.SetBool("camera_shotFlag", false);
                if (m_audioSource.isPlaying)
                {
                    m_audioSource.Stop();
                }
                break;
            case PlayerStatus.Camera_Shot:
                //アニメーション
                m_animator.SetBool("runFlag", false);
                m_animator.SetBool("walkFlag", false);
                m_animator.SetBool("idleFlag", false);
                m_animator.SetBool("turn_rightFlag", false);
                m_animator.SetBool("turn_leftFlag", false);
                m_animator.SetBool("damageFlag", false);
                m_animator.SetBool("deathFlag", false);
                m_animator.SetBool("camera_shotFlag", true);
                if (m_audioSource.isPlaying)
                {
                    m_audioSource.Stop();
                }
                break;
            case PlayerStatus.Clear:
                m_animator.SetBool("runFlag", false);
                m_animator.SetBool("walkFlag", false);
                m_animator.SetBool("idleFlag", false);
                m_animator.SetBool("turn_rightFlag", false);
                m_animator.SetBool("turn_leftFlag", false);
                m_animator.SetBool("damageFlag", false);
                m_animator.SetBool("deathFlag", false);
                m_animator.SetBool("camera_shotFlag", false);
                m_animator.SetTrigger("GameClear");
                m_audioSource.pitch = 1.8f;

                break;
            default:
                break;
        }
    }

    // クリア時の処理
    public void doClearAnim(Vector3 pos)
    {
        m_dest = pos;
        m_dest.y = 0.0f;
        // 目的地の方向を向く
        transform.LookAt(m_dest);

        // 目的地の方向に移動させる
        Vector3 dir = (m_dest - transform.position).normalized;
        transform.position += dir * m_clearSpeed * Time.deltaTime;
        m_animator.SetTrigger("GameClear");
        m_animator.SetBool("walkFlag", false);
        m_animator.SetBool("runFlag", false);
        m_audioSource.pitch = 0.8f;
        if (!m_audioSource.isPlaying)
        {
            m_audioSource.PlayOneShot(m_bressSE);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
