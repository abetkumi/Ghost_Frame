using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//霧の壁用スクリプト
public class MistWallScript : MonoBehaviour
{
    [SerializeField] GameObject m_messageObject;
    DoorMessageScript m_messageScript;
    [SerializeField] private AudioClip m_MistBreakSE;
    private AudioSource m_audioSource;

    private void Start()
    {
        m_messageScript = m_messageObject.GetComponent<DoorMessageScript>();
        m_audioSource = GetComponent<AudioSource>();
    }

    //プレイヤーが近づいたら
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            m_messageScript.m_messageStatus = MessageStatus.MistWallClose;
        }
    }

    //プレイヤーがドアから離れたら
    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            m_messageScript.m_messageStatus = MessageStatus.NoMessage;
        }
    }

    async public void doBreak()
    {
        m_messageScript.m_messageStatus = MessageStatus.MistWallBreak;
        m_audioSource.PlayOneShot(m_MistBreakSE);
        //3秒待つ
        await UniTask.Delay(3000);
        Destroy(gameObject);
    }
}
