using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemySearchScript : MonoBehaviour
{
    [SerializeField] GameObject m_enemyObject;
    private EnemyScript m_enemyScript;
    [SerializeField]
    private SphereCollider m_searchArea;
    float myTime;

    // Start is called before the first frame update
    void Start()
    {
        m_enemyScript = m_enemyObject.GetComponentInParent<EnemyScript>();
    }

    //プレイヤーがチェイス範囲に入ったらチェイスステートに移行する
    void OnTriggerStay(Collider col)
    {       
        if (col.tag == "Player")
        {
            m_enemyScript.ChangeEnemyStatusIfPossible( EnemyStatus.Chase );
        }
    }

    //プレイヤーがチェイス範囲から出たらサーチステートに移行する
    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
             m_enemyScript.ChangeEnemyStatusIfPossible( EnemyStatus.Search ) ;
             m_enemyScript.m_agent.SetDestination(m_enemyScript.m_movePoint_N.position);
             Debug.Log("見失った");
        }
    }
}
