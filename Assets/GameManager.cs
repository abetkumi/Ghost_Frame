using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using TMPro;

//ゲーム進行用ステータスの列挙型の定義
enum GameStatus
{
    Init,
    GamePlay,
    GameOver,
    GameClear,
    GameStop,
    GamePause,
}

//ゲームマネージャークラスのスクリプト
public class GameManager : MonoBehaviour
{
    //ゲームオーバー用変数
    [SerializeField] GameObject myGameOverImage;
    Image m_gameOverImage;
    //ゲームクリア変数
    [SerializeField] GameObject myGameClearImage;
    Image m_gameClearImage;
    [SerializeField] GameObject TMPObject;
    Press_Any_KeyTMPScript myTMP;
    //BGM管理スクリプト用変数
    [SerializeField] GameObject m_BGMObject;
    BGMScript m_BGMScript;
    [SerializeField] AudioClip m_fallSE;
    AudioSource m_audioSource;
    //画像表示用変数
    float t = 0.0f;
    //プレイヤースクリプト用変数
    [SerializeField] GameObject m_playerObject;
    PlayerScript m_playerScript;
    //カメラスクリプト用変数
    [SerializeField] GameObject m_cameraObject;
    [SerializeField] GameObject m_subcameraObject;
    MainCamera m_cameraScript;
    [SerializeField] GameObject m_cameraQUIObject;
    CameraQ_UIScript m_cameraQUIScript;
    //エネミースクリプト用変数
    [SerializeField] GameObject m_enemyObject;
    EnemyScript m_enemyScript;
    //アタックパネルスクリプト用変数
    [SerializeField] GameObject m_attackObject;
    AttackScript m_attackScript;
    //プレイヤーHPスクリプト用変数
    [SerializeField] GameObject m_playerHPObject;
    PlayerHPScript m_playerHPScript;
    //ゲームクリアスクリプト用変数
    [SerializeField] GameObject m_gameClearObject;
    GameClearScript m_gameClearScript;
    //ポーズメニュー用スクリプト
    [SerializeField] GameObject m_pauseObject;
    //ステータス用変数
    GameStatus m_gameStatus = GameStatus.Init;


    // Start is called before the first frame update
    void Start()
    {
        //テキストメッシュプロオブジェクトからテキストメッシュプロを取得
        myTMP = TMPObject.GetComponent<Press_Any_KeyTMPScript>();
        //プレイヤーオブジェクトからプレイヤースクリプトを取得
        m_playerScript = m_playerObject.GetComponent<PlayerScript>();
        //カメラオブジェクトからカメラスクリプトを取得
        m_cameraScript = Camera.main.GetComponent<MainCamera>();
        //エネミーオブジェクトからエネミースクリプトを取得
        m_enemyScript = m_enemyObject.GetComponent<EnemyScript>();
        //パネルオブジェクトからパネルスクリプトを取得
        m_attackScript = m_attackObject.GetComponent<AttackScript>();
        //ゲームクリア判定からゲームクリアスクリプトを取得
        m_gameClearScript = m_gameClearObject.GetComponent<GameClearScript>();
        //プレイヤーHPオブジェクトからプレイヤーHPスクリプトを取得
        m_playerHPScript = m_playerHPObject.GetComponent<PlayerHPScript>();
        //カメラUIオブジェクトからカメラUIスクリプトを取得
        m_cameraQUIScript = m_cameraQUIObject.GetComponent<CameraQ_UIScript>();
        //ゲームオーバー画像
        m_gameOverImage = myGameOverImage.GetComponent<Image>();
        //ゲームクリア画像
        m_gameClearImage = myGameClearImage.GetComponent<Image>();
        t = 0.0f;
        //BGM管理オブジェクトからBGM変更スクリプトを取得
        m_BGMScript = m_BGMObject.GetComponent<BGMScript>();
        //オーディオソース
        m_audioSource = GetComponent<AudioSource>();

        // ディザリングがオンになる
        Shader.EnableKeyword("_DITHERING_ON");
        //ディザリングがオフになる
        //Shader.DisableKeyword("_DITHERING_ON");
    }

    void doInit()
    {
        //プレイヤーの初期設定
        m_playerScript.doInit();
        //カメラの初期設定
        m_cameraScript.doInit();
        //ステータス更新
        Time.timeScale = 1.0f;
        m_gameStatus = GameStatus.GamePlay;
    }

    //インゲーム用メソッド
    async void doInGame()
    {
        //カメラ操作
        m_cameraScript.doMove();
        //カメラのステータス
        m_cameraScript.doCameraStatus();
        //プレイヤーの移動
        m_playerScript.doMove();
        //プレイヤーのステータス
        m_playerScript.doAnimStatus();
        //プレイヤー攻撃時のメソッド
        m_attackScript.doAttack_Gauge(20.0f, m_cameraScript.cameraStatus);
        //プレイヤーHP変動
        m_playerHPScript.doHit();
        if (m_playerHPScript.isHPView == true)
        {
            m_playerHPScript.doHPView();
        }
        //プレイヤーのHPが0以下の時
        if (m_playerHPScript.m_player_Fainal_HP <= 0)
        {
            await UniTask.Delay(1000);
            m_gameStatus = GameStatus.GameOver;
        }
        //ポーズ画面移行
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            m_gameStatus = GameStatus.GamePause;
            m_pauseObject.SetActive(true);
            Time.timeScale = 0.0f;
        }
        //BGM変更
        m_BGMScript.BGMChange();
    }

    //プレイヤーがゲームクリア判定に入ったら
    void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player")
        {
            if (m_playerScript.m_audioSource.isPlaying)
            {
                m_playerScript.m_audioSource.Stop();
            }
            m_gameStatus = GameStatus.GameClear;
            m_BGMObject.SetActive(false);
        }
    }

    //ゲームオーバー用メソッド
    void doGameOver()
    {
        t += Time.deltaTime;
        if (t > 1.0f)
        {
            t = 1.0f;
        }
        Debug.Log("ゲームオーバー");
        m_gameOverImage.color = new Color(1.0f, 1.0f, 1.0f, t * t);
        m_cameraQUIScript.doClearUI();
        myTMP.doColor(t);
        if (Input.anyKeyDown)
        {
            Debug.Log("タイトルに戻る");
            m_gameStatus = GameStatus.Init;
            //メインゲームシーンに移動する
            SceneManager.LoadScene("Title");
        }
    }

    //ゲームクリア用メソッド
    void doGameClear()
    {
        m_gameClearScript.doGameClear();
        m_cameraQUIScript.doClearUI();
        if ((m_playerScript.m_dest - m_playerScript.transform.position).magnitude < 0.5f)
        {
            m_gameStatus = GameStatus.GameStop;
            m_playerScript.m_animator.SetTrigger("GameClear_Last");
            m_audioSource.PlayOneShot(m_fallSE);
        }
    }

    void doGamePause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            m_gameStatus = GameStatus.GamePlay;
            m_pauseObject.SetActive(false);
            Time.timeScale = 1.0f;
        }
    }

    //アウトゲーム用メソッド
    void doOutGame()
    {
        switch (m_gameStatus)
        {
            case GameStatus.Init:
                doInit();
                break;
            case GameStatus.GamePlay:
                doInGame();
                break;
            case GameStatus.GameOver:
                doGameOver();
                break;
            case GameStatus.GameClear:
                doGameClear();
                break;
            case GameStatus.GameStop:
                t += Time.deltaTime * 0.2f;
                if (t > 1.0f)
                {
                    t = 1.0f;
                }
                m_gameClearImage.color = new Color(1.0f, 1.0f, 1.0f, t);
                myTMP.doColor(t);
                if (Input.anyKeyDown)
                {
                    m_gameStatus = GameStatus.Init;
                    Debug.Log("タイトルに戻る");
                    //メインゲームシーンに移動する
                    SceneManager.LoadScene("Title");
                }
                break;
            case GameStatus.GamePause:
                doGamePause();
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        doOutGame();
    }
}
