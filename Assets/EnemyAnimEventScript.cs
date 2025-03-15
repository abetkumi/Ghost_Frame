using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimEventScript : MonoBehaviour
{
    [SerializeField] private GameObject enemyObject;
    [SerializeField] private SphereCollider sphereCollider;
    [SerializeField] private GameObject attackObject;
    EnemyScript m_enemyScript;
    AttackScript m_attackScript;
    // Start is called before the first frame update
    void Start()
    {
        m_enemyScript = enemyObject.GetComponent<EnemyScript>();
        m_attackScript = attackObject.GetComponent<AttackScript>();
    }

    //攻撃アニメーション開始時
    void StatusStart()
    {
        m_enemyScript.m_agent.speed = 0.0f;
    }

    //攻撃判定開始時
    void AttackStart()
    {
        sphereCollider.enabled = true;
        m_enemyScript.m_agent.speed = 3.5f;
        m_attackScript.m_attack_current = m_attackScript.m_attack_max;
        Debug.Log("攻撃判定出現");
    }

    //攻撃判定終了時
    public void AttackEnd()
    {
        sphereCollider.enabled = false;
        m_enemyScript.m_agent.speed = 0.0f;
        Debug.Log("攻撃判定終了");
    }

    //攻撃アニメーション終了時
    async void StatusEnd()
    {
        await UniTask.Delay(10);
        m_enemyScript.ChangeEnemyStatusIfPossible( EnemyStatus.Search );
        Debug.Log("チェイスに戻る");
    }

    //歩きアニメーション開始時
    void WalkStart()
    {
        m_enemyScript.m_agent.speed = 2.0f;
    }

    //ダメージアニメーション開始時
    public void DamageStart()
    {
        m_enemyScript.m_agent.speed = 0.0f;
        sphereCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_enemyScript.m_Enemy_HP <= 0)
        {
            sphereCollider.enabled = false;
        }
    }
}
