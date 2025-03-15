using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;


public class PlayerHPScript : MonoBehaviour
{
    [SerializeField] GameObject m_playerHP;
    [SerializeField] GameObject m_playerObject;
    [SerializeField] private float m_nextCountTime = 0.0f;
    [SerializeField] private int m_amountOfDamageAtOneTime = 100;
    [SerializeField] AudioClip m_shoutSE;
    AudioSource m_audioSource;
    private PlayerScript m_playerScript;
    //プレイヤーの体力
    public float m_player_HP;
    private float m_player_MAX_HP = 100.0f;
    public float m_player_Fainal_HP;
    private float m_countTime = 0.0f;
    private int m_damage = 0;
    private bool isReducing;
    public bool isHPView;
    //　HP表示用スライダー
    private Slider m_hpSlider;
    //　一括HP表示用スライダー
    private Slider m_bulkHPSlider;

    // Start is called before the first frame update
    void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
        m_playerScript = m_playerObject.GetComponent<PlayerScript>();
        m_hpSlider = transform.Find("HPSlider").GetComponent<Slider>();
        m_bulkHPSlider = transform.Find("BulkHPSlider").GetComponent<Slider>();
        m_player_HP = m_player_MAX_HP;
        m_player_Fainal_HP = m_player_MAX_HP;
        m_hpSlider.value = 1;
        m_bulkHPSlider.value = 1;
        isReducing = false;
        isHPView = false;
        m_playerHP.SetActive(false);
    }

    //プレイヤーのHPを減らす処理
    public void doTakeDamage(int damage)
    {
        m_playerHP.SetActive(true);
        //　ダメージを受けた時に一括HP用のバーの値を変更する
        var tempHP = Mathf.Max(m_player_Fainal_HP -= damage, 0);
        m_bulkHPSlider.value = (float)tempHP / m_player_MAX_HP;
        this.m_damage += damage;
        m_countTime = 0.0f;
        //　一定時間後にHPバーを減らすフラグを設定
        Invoke("StartReduceHP", 1.0f);
        Debug.Log("当たった！");
        if (m_player_Fainal_HP <= 0.0f)
        {
            Debug.Log("Dead");
            m_playerScript.doDead();
            m_audioSource.PlayOneShot(m_shoutSE);
        }
        else
        {
            m_playerScript.m_playerAnimStatus = PlayerStatus.Damage;
        }
    }

    //プレイヤーのHPゲージの見た目を少しずつ減らす処理
    public void doHit()
    {
        //　ダメージなければ何もしない
        if (!isReducing)
        {
            return;
        }
        //　次に減らす時間がきたら
        if (m_countTime >= m_nextCountTime)
        {
            int tempDamage;
            //　決められた量よりも残りダメージ量が小さければ小さい方を1回のダメージに設定
            tempDamage = Mathf.Min(m_amountOfDamageAtOneTime, m_damage);
            m_player_HP -= tempDamage;
            //　全体の比率を求める
            m_hpSlider.value = m_player_HP / m_player_MAX_HP;
            //　全ダメージ量から1回で減らしたダメージ量を減らす
            m_damage -= tempDamage;
            //　全ダメージ量が0より下になったら0を設定
            m_damage = Mathf.Max(m_damage, 0);

            m_countTime = 0.0f;

            //　ダメージがなくなったらHPバーの変更処理をしないようにする
            if (m_damage <= 0)
            {
                isReducing = false;

                isHPView = true;
            }
        }
        m_countTime += Time.deltaTime;
    }

    //　徐々にHPバーを減らすのをスタート
    public void StartReduceHP()
    {
        isReducing = true;
    }

    //プレイヤーHPバーが見える時間
    async public void doHPView()
    {
        await UniTask.Delay(2000);
        isHPView = false;
        m_playerHP.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
