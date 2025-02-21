using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackSearchScript : MonoBehaviour
{
    [SerializeField] private SphereCollider m_searchArea;
    [SerializeField] GameObject m_enemyObject;
    EnemyScript m_enemyScript;
    // Start is called before the first frame update
    void Start()
    {
        m_enemyScript = m_enemyObject.GetComponent<EnemyScript>();
    }

    //プレイヤーが攻撃できる範囲に入ったら
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            m_enemyScript.ChangeEnemyStatusIfPossible(EnemyStatus.Attack);
            Debug.Log("アタックステート");
        }
    }
    //void OnTriggerExit(Collider col)
    //{
    //    if (col.tag == "Player")
    //    {
    //        m_enemyScript.ChangeEnemyStatusIfPossible(EnemyStatus.Chase);
    //        Debug.Log("アタックステート");
    //    }
    //}
}
