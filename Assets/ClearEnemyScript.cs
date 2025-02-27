using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearEnemyScript : MonoBehaviour
{
    [SerializeField] AudioClip m_shoutSE;
    AudioSource m_audioSource;
    [SerializeField] GameObject m_playerObject;
    private Animator m_animator;
    private EnemyStatus m_enemyStatus;
    Vector3 m_dest;
    bool isNear = false;
    // Start is called before the first frame update
    void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
        m_animator = GetComponent<Animator>();
        isNear = false;
    }

    //アニメーション
    void doAnimation()
    {
        if (m_enemyStatus == EnemyStatus.Idle)
        {
            m_animator.SetBool("isIdle", true);
            m_animator.SetBool("isWalking", false);
            m_animator.SetBool("isChase", false);
            m_animator.SetBool("isAttack", false);
            m_animator.SetBool("isWait", false);
            m_animator.SetBool("isDamage", false);
            m_animator.SetBool("isDead", false);
            Debug.Log("アイドル");
        }
        else if (m_enemyStatus == EnemyStatus.Search)
        {
            m_animator.SetBool("isIdle", false);
            m_animator.SetBool("isWalking", true);
            m_animator.SetBool("isChase", false);
            m_animator.SetBool("isAttack", false);
            m_animator.SetBool("isWait", false);
            m_animator.SetBool("isDamage", false);
            m_animator.SetBool("isDead", false);
            Debug.Log("歩くよ！");
        }
        else if (m_enemyStatus == EnemyStatus.Chase)
        {
            m_animator.SetBool("isIdle", false);
            m_animator.SetBool("isWalking", false);
            m_animator.SetBool("isChase", true);
            m_animator.SetBool("isAttack", false);
            m_animator.SetBool("isWait", false);
            m_animator.SetBool("isDamage", false);
            m_animator.SetBool("isDead", false);
            Debug.Log("追いかける");
        }
        else if (m_enemyStatus == EnemyStatus.Attack)
        {
            m_animator.SetBool("isIdle", false);
            m_animator.SetBool("isWalking", false);
            m_animator.SetBool("isChase", false);
            m_animator.SetBool("isAttack", true);
            m_animator.SetBool("isWait", false);
            m_animator.SetBool("isDamage", false);
            m_animator.SetBool("isDead", false);
            Debug.Log("攻撃");
        }
        else if (m_enemyStatus == EnemyStatus.Wait)
        {
            m_animator.SetBool("isIdle", false);
            m_animator.SetBool("isWalking", false);
            m_animator.SetBool("isChase", false);
            m_animator.SetBool("isAttack", false);
            m_animator.SetBool("isWait", true);
            m_animator.SetBool("isDamage", false);
            m_animator.SetBool("isDead", false);
            Debug.Log("一時停止");
        }
        else if (m_enemyStatus == EnemyStatus.Damage)
        {
            m_animator.SetBool("isIdle", false);
            m_animator.SetBool("isWalking", false);
            m_animator.SetBool("isChase", false);
            m_animator.SetBool("isAttack", false);
            m_animator.SetBool("isWait", false);
            m_animator.SetBool("isDamage", true);
            m_animator.SetBool("isDead", false);
            Debug.Log("被弾");
        }
        else if (m_enemyStatus == EnemyStatus.Dead)
        {
            m_animator.SetBool("isIdle", false);
            m_animator.SetBool("isWalking", false);
            m_animator.SetBool("isChase", false);
            m_animator.SetBool("isAttack", false);
            m_animator.SetBool("isWait", false);
            m_animator.SetBool("isDamage", false);
            m_animator.SetBool("isDead", true);
            Debug.Log("4");
        }
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            isNear = true;
            Debug.Log("アタックステート");
            m_audioSource.PlayOneShot(m_shoutSE);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isNear == true)
        {
            m_animator.SetBool("isAttack", true);
        }
        else
        {
            m_dest = m_playerObject.transform.position;
            // 目的地の方向を向く
            transform.LookAt(m_dest);

            // 目的地の方向に移動させる
            Vector3 dir = (m_dest - transform.position).normalized;
            transform.position += dir * 10.5f * Time.deltaTime;

        }
    }
}
