using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenScript : MonoBehaviour
{
    [SerializeField] GameObject m_doorMessageObject;
    DoorMessageScript m_doorMessageScript;
    [SerializeField] GameObject m_FinaldoorObject;
    [SerializeField] private AudioClip m_DoorSE;

    private AudioSource m_audioSource;
    //カメラ用変数
    MainCamera m_cameraScript;
    //　ドアエリアに入っているかどうか
    private bool isNear;
    //　ドアのアニメーター
    private Animator m_animator;

    void Start()
    {
        m_cameraScript = Camera.main.GetComponent<MainCamera>();
        m_audioSource = GetComponent<AudioSource>();
        isNear = false;
        m_animator = GetComponent<Animator>();
        m_doorMessageScript = m_doorMessageObject.GetComponent<DoorMessageScript>();
    }

    //プレイヤーがドアの近くに来たら
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {          
            isNear = true;
        }
    }

    //プレイヤーがドアから離れたら
    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            isNear = false;
            m_doorMessageScript.m_messageStatus = MessageStatus.NoMessage;
        }
    }

    void Update()
    {
        if (isNear == false || m_cameraScript.cameraStatus == CameraStatus.First_Parson)
        {
            return;
        }

        //ドアを開ける
        if (Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("Action"))
        {
            m_audioSource.PlayOneShot(m_DoorSE);
            m_animator.SetBool("Open", !m_animator.GetBool("Open"));
        }

        //ドアの開閉メッセージ
        if (m_animator.GetBool("Open") == false)
        {
            m_doorMessageScript.m_messageStatus = MessageStatus.DoorOpen;
        }
        else
        {
            m_doorMessageScript.m_messageStatus = MessageStatus.DoorClose;
        }
    }
}
