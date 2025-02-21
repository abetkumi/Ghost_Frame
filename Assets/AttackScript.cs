using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

//UI用のスクリプトクラス
public class AttackScript : MonoBehaviour
{
    [SerializeField] GameObject m_enemyObject;
    [SerializeField] GameObject m_subCameraObject;
    [SerializeField] AudioClip m_shoterSE;
    [SerializeField] AudioClip m_cameraChargeSE;
    [SerializeField] AudioClip m_cameraChargeFinishSE;
    EnemyScript m_enemyScript;
    Camera m_cam;
    // MainCamera cameraScript;
    [SerializeField] private AudioSource m_audioSource;
    [SerializeField] private AudioSource m_audioSource_ChargeSE;
    [SerializeField] private AudioSource m_audioSource_ChargeFinishSE;
    public bool isShot;
    public Image m_attack_Gauge;
    public float m_attack_max = 100.0f;
    public float m_attack_current = 0.0f;
    Ray ray;

    // Start is called before the first frame update
    void Start()
    {
        m_cam = m_subCameraObject.GetComponent<Camera>();
        m_attack_Gauge = GetComponent<Image>();
        m_enemyScript = m_enemyObject.GetComponent<EnemyScript>();
        isShot = true;
    }

    //攻撃判定の処理
    public void doAttack_Gauge(float amount,CameraStatus cameraStatus)
    {
        //一人称視点の時
        if (cameraStatus == CameraStatus.First_Parson)
        {
            //画面中央からレイを飛ばす
            ray = m_cam.ScreenPointToRay(new Vector3(Screen.width / 2.0f, Screen.height / 2.0f, 0.0f));
            
            //レイが敵の頭に当たった時
            if (Physics.Raycast(ray, out RaycastHit hit, 30.0f, LayerMask.GetMask("EnemyHead")))
            {
                //攻撃可能なら
                if (isShot == true)
                {
                    m_attack_current += amount * 0.7f * Time.deltaTime;
                    if (!m_audioSource_ChargeSE.isPlaying)
                    {
                        m_audioSource_ChargeSE.PlayOneShot(m_cameraChargeSE);
                    }   
                }

                //Eボタンで攻撃する
                if (Input.GetKeyDown(KeyCode.E))
                {
                    doAttack(hit.transform);
                    isShot = false;
                    m_audioSource_ChargeSE.Stop();

                    //攻撃SEを鳴らす
                    if (!m_audioSource.isPlaying)
                    {
                        Debug.Log("SE2");
                        m_audioSource.PlayOneShot(m_shoterSE);
                    }
                }
            }

            //レイが勾玉に当たった時
            else if (Physics.Raycast(ray, out hit, 30.0f, LayerMask.GetMask("Magatama")))
            {
                //攻撃可能なら
                if (isShot == true)
                {
                    m_attack_current += amount * 0.7f * Time.deltaTime;
                    if (!m_audioSource_ChargeSE.isPlaying)
                    {
                        m_audioSource_ChargeSE.PlayOneShot(m_cameraChargeSE);
                    }
                }

                //Eボタンで攻撃する
                if (Input.GetKeyDown(KeyCode.E))
                {
                    doAttack_Magatama(hit.transform);
                    isShot = false;
                    m_audioSource_ChargeSE.Stop();

                    //攻撃SEを鳴らす
                    if (!m_audioSource.isPlaying)
                    {
                        Debug.Log("SE2");
                        m_audioSource.PlayOneShot(m_shoterSE);
                    }
                }
            }

            //レイが攻撃可能な物に当たっていないなら
            else
            {
                m_audioSource_ChargeSE.Stop();
                m_attack_current -= amount * 0.7f * Time.deltaTime;
            }

            //アタックゲージがマックスの時
            if (m_attack_current > m_attack_max)
            {
                m_attack_current = m_attack_max;
                m_audioSource_ChargeSE.Stop();
                
                //チャージマックスSEを鳴らす
                if (!m_audioSource_ChargeFinishSE.isPlaying)
                {
                    m_audioSource_ChargeFinishSE.PlayOneShot(m_cameraChargeFinishSE);
                }
            }

            //アタックゲージが0以下の時
            else if(m_attack_current < 0)
            {
                m_attack_current = 0;
            }
            m_attack_current = Mathf.Clamp(m_attack_current, 0, m_attack_max);
            
            //アタックゲージの画像を変更する
            doUpdateGauge();    
        }

        //3人称視点の時
        else
        {
            m_attack_current = 0;
        }
    }

    //攻撃処理
    void doAttack(Transform target)
    {
        // Debug.DrawRay(ray.origin, ray.direction * 10, Color.red);
        // 親に向けてメッセージを送る
        if (m_attack_current != m_attack_max)
        {
            target.GetComponent<EnemyHeadScript>().TakeDamage((int)m_attack_current / 4);
        }
        
        m_attack_current = 0;
    }

    //勾玉を攻撃した時
    void doAttack_Magatama(Transform target)
    {
        if(m_attack_current >= m_attack_max -1)
        {
          //勾玉を壊す
          target.GetComponent<MagatamaScript>().doBreak();
        }

        m_attack_current = 0;
    }

    //アタックゲージの増減処理
    public void doUpdateGauge()
    {
        m_attack_Gauge.fillAmount = (m_attack_current / m_attack_max);
    }

    //攻撃後の攻撃不可時間
    async void doWait()
    {
        await UniTask.Delay(1000);
        isShot = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isShot == false)
        {
            doWait();
        }
    }
}

