using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemySearchScript : MonoBehaviour
{
    [SerializeField] GameObject m_enemyObject;
    private EnemyScript m_enemyScript;
    [SerializeField] private BoxCollider m_searchArea;
    //BGM管理スクリプト用変数
    [SerializeField] GameObject m_BGMObject;
    BGMScript m_BGMScript;
    float myTime;

    // Start is called before the first frame update
    void Start()
    {
        m_enemyScript = m_enemyObject.GetComponentInParent<EnemyScript>();
        //BGM管理オブジェクトからBGM変更スクリプトを取得
        m_BGMScript = m_BGMObject.GetComponent<BGMScript>();
    }

    //プレイヤーがチェイス範囲に入ったらチェイスステートに移行する
    void OnTriggerStay(Collider col)
    {       
        if (col.tag == "Player")
        {
            m_BGMScript.m_BGMstatus = BGMStatus.ChaseBGM;
            m_enemyScript.ChangeEnemyStatusIfPossible( EnemyStatus.Chase );
        }
    }

    //プレイヤーがチェイス範囲から出たらサーチステートに移行する
    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            m_BGMScript.m_BGMstatus = BGMStatus.NormalBGM;
             m_enemyScript.ChangeEnemyStatusIfPossible( EnemyStatus.Search ) ;
             m_enemyScript.m_agent.SetDestination(m_enemyScript.m_movePoint_N.position);
             Debug.Log("見失った");
        }
    }
}
