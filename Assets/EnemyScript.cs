using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum EnemyStatus
{
    Idle,
    Search,
    Chase,
    Attack,
    Wait,
    Damage,
    DamageEnd,
    Dead,
}

public class EnemyScript : MonoBehaviour
{
    [SerializeField] GameObject m_enemyObject;
    [SerializeField] GameObject m_enemyHeadObject;
    [SerializeField] GameObject m_playerObject;
    [SerializeField] GameObject m_playerHPObject;
    [SerializeField] GameObject m_attackObject;
    [SerializeField] public Transform m_movePoint_N;
    [SerializeField] private Transform m_movePoint_S;
    [SerializeField] private Transform m_target;
    [SerializeField] private Transform m_enemyTrans;
    [SerializeField] private BoxCollider m_collider;
    [SerializeField] private SphereCollider m_attackCollider;
    [SerializeField] private float m_nextCountTime = 0.0f;
    [SerializeField] private int m_amountOfDamageAtOneTime = 100;
    [SerializeField] private AudioClip m_enemyDeadVoice;
    [SerializeField] private AudioClip m_enemyDamageVoice;
    //BGM管理スクリプト用変数
    [SerializeField] GameObject m_BGMObject;
    BGMScript m_BGMScript;
    //プレイヤー用変数
    PlayerScript m_playerScript;
    PlayerHPScript m_playerHPScript;
    AttackScript m_attackScript;
    EnemyHeadScript m_enemyHeadScript;
    private NavMeshAgent m_navMeshAgent;
    public NavMeshAgent m_agent;
    private float m_countTime = 0.0f;
    public float m_Enemy_HP = 100.0f;
    private float m_Enemy_MAX_HP = 100.0f;
    private float m_Enemy_Fainal_HP = 100.0f;
    private Transform m_playerTransform;
    private Animator m_animator;
    private Slider m_hpSlider;
    private Slider m_bulkHPSlider;
    private AudioSource m_audioSource;
    private EnemyStatus m_enemyStatus;

    public EnemyStatus EnemyStatus
    {
        get;
    }
    /// <summary>
    /// 敵の状態を変更できれば変更する
    /// </summary>
    /// <param name="nextStatus"></param>
    public void ChangeEnemyStatusIfPossible(EnemyStatus nextStatus)
    {
        if (m_enemyStatus == EnemyStatus.Damage)
        {
            if(nextStatus == EnemyStatus.DamageEnd
                || nextStatus == EnemyStatus.Dead 
            )
            {
                // ダメージ中は終了状態への遷移しか許可しない
                m_enemyStatus = nextStatus;
            }
        }
        else{

            m_enemyStatus = nextStatus;
        }
    }
    public int m_damage = 0;
    private bool m_isReducing;
    public bool m_isDead;

    // アニメーターのパラメーターのIDを取得（高速化のため）
    readonly int DeadHash = Animator.StringToHash("Dead");

    // Start is called before the first frame update
    void Start()
    {
        m_enemyHeadScript =m_enemyHeadObject.GetComponent<EnemyHeadScript>();
        m_playerScript = m_playerObject.GetComponent<PlayerScript>();
        m_playerHPScript = m_playerHPObject.GetComponent<PlayerHPScript>();
        m_attackScript = m_attackObject.GetComponent<AttackScript>();
        m_navMeshAgent = GetComponent<NavMeshAgent>();
        m_agent = GetComponent<NavMeshAgent>();
        m_animator = GetComponent<Animator>();
        m_hpSlider = transform.Find("EnemyHP_Bar/HPSlider").GetComponent<Slider>();
        m_bulkHPSlider = transform.Find("EnemyHP_Bar/BulkHPSlider").GetComponent<Slider>();
        m_audioSource = GetComponent<AudioSource>();
        //BGM管理オブジェクトからBGM変更スクリプトを取得
        m_BGMScript = m_BGMObject.GetComponent<BGMScript>();
        m_hpSlider.value = 1.0f;
        m_bulkHPSlider.value = 1.0f;
        ChangeEnemyStatusIfPossible( EnemyStatus.Idle );
        doInit();
    }

    public void doInit()
    {
        m_Enemy_HP = m_Enemy_MAX_HP;
        m_Enemy_Fainal_HP = m_Enemy_MAX_HP;
        m_hpSlider.value = 1.0f;
        m_bulkHPSlider.value = 1.0f;
        m_enemyStatus = EnemyStatus.Idle;
        m_agent.SetDestination(m_movePoint_N.position);
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

    //徘徊
    private void doSearchMove()
    {
        if (Vector3.Distance(transform.position, m_movePoint_S.position) < 1.0f)
        {
            m_agent.SetDestination(m_movePoint_N.position);
        }
        else if (Vector3.Distance(transform.position, m_movePoint_N.position) < 1.0f)
        {
            m_agent.SetDestination(m_movePoint_S.position);
        }
    }

    //プレイヤー発見後
    private void doChaseMove()
    {
        m_agent.SetDestination(m_target.position);
        if (Vector3.Distance(transform.position, m_playerObject.transform.position) < 3.0f)
        {
            Debug.Log("アタックステート");
            ChangeEnemyStatusIfPossible( EnemyStatus.Attack );          
        }
    }


    public void doDamage()
    {
        if (!m_isReducing)
        {
            return;
        }
        
        //次に減らす時間が来たら
        if (m_countTime >= m_nextCountTime)
        {
            int m_tempDamage;
            //決められた量より残りダメージ量が小さければ小さい方を1回の「ダメージに設定
            m_tempDamage = Mathf.Min(m_amountOfDamageAtOneTime, m_damage);
            m_Enemy_HP -= m_tempDamage;
            //全体の比率を求める
            m_hpSlider.value = m_Enemy_HP / m_Enemy_MAX_HP;
            //全ダメージ量から１回で減らしたダメージ量を減らす
            m_damage -= m_tempDamage;
            //全ダメージが0より下になったら0を設定
            m_damage = Mathf.Max(m_damage, 0);

            m_countTime = 0;
            //ダメージがなくなったらHPバーの変更処理をしないようにする
            if (m_damage <= 0)
            {
                m_isReducing = false;
            }
            Debug.Log("ダメージ");
        }
        m_countTime += Time.deltaTime;
    }
    //　ダメージ値を追加するメソッド
    public void TakeDamage(int damage)
    {
        if (m_attackScript.m_attack_current >= m_attackScript.m_attack_max - 3)
        {
            ChangeEnemyStatusIfPossible(EnemyStatus.Damage);
        }
        //　ダメージを受けた時に一括HP用のバーの値を変更する
        var tempHP = Mathf.Max(m_Enemy_Fainal_HP -= damage, 0);

        m_bulkHPSlider.value = (float)tempHP / m_Enemy_MAX_HP;
        this.m_damage += damage;
        m_countTime = 0.0f;
        //HPが0になったら
        if (m_Enemy_HP <= 0)
        {
            m_Enemy_HP = 0;
            m_audioSource.Stop();

            if (!m_audioSource.isPlaying)
            {
                m_audioSource.PlayOneShot(m_enemyDeadVoice);
            }  
            ChangeEnemyStatusIfPossible(EnemyStatus.Dead);
            
            doDead();
        }
        else
        {
            if (!m_audioSource.isPlaying)
            {
                m_audioSource.PlayOneShot(m_enemyDamageVoice);
            }
        }
        //　一定時間後にHPバーを減らすフラグを設定
        Invoke("StartReduceHP", 1.0f);
    }

    //　徐々にHPバーを減らすのをスタート
    public void StartReduceHP()
    {
        m_isReducing = true;
    }


    //4んだ時の処理
    void doDead()
    {
        m_isDead = true;
        m_collider.enabled = false;
        m_animator.SetBool(DeadHash, true);
        m_agent.speed = 0.0f;
        m_BGMScript.m_BGMstatus = BGMStatus.NormalBGM;
        m_enemyHeadScript.doDeadHead();
        StartCoroutine(nameof(DeadTimer));
        Debug.Log("4んだ");
    }

    IEnumerator DeadTimer()
    {
        yield return new WaitForSeconds(7.0f);

        Destroy(gameObject);
    }

    void doEnemyStatus()
    {
        switch (m_enemyStatus)
        {
            case EnemyStatus.Idle:
                ChangeEnemyStatusIfPossible( EnemyStatus.Search );
                break;
            case EnemyStatus.Search:
                doSearchMove();
                break;
            case EnemyStatus.Chase:
                doChaseMove();
                break;
            case EnemyStatus.Attack:
                //doAttack();
         
                break;
            case EnemyStatus.Wait:
                break;
            case EnemyStatus.Damage:
                var hashDamage = Animator.StringToHash("Base Layer.Big Hit");
                var animatorState = m_animator.GetCurrentAnimatorStateInfo(0);
                if (animatorState.nameHash == hashDamage 
                    && animatorState.normalizedTime >= 1.0f)
                {
                    // HPバーを減らす演出が終わった　かつ　ダメージモーションの再生も終わった
                    ChangeEnemyStatusIfPossible(EnemyStatus.DamageEnd);
                }
                break;
            case EnemyStatus.Dead:
                //doDead();
                break;
            default:
                break;
        }
    }


    // Update is called once per frame
    void Update()
    {
        doAnimation();
        doEnemyStatus();
        doDamage();
    }
}
