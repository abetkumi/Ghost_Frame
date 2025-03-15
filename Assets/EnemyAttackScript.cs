using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class EnemyAttackScript : MonoBehaviour
{
    [SerializeField] GameObject m_playerObject;
    [SerializeField] GameObject m_playerHPObject;
    [SerializeField] GameObject m_DamageScreenObject;
    [SerializeField] GameObject m_DamageScreenObject_F;
    PlayerScript m_playerScript;
    PlayerHPScript m_playerHPScript;
    DamageScreenScript m_damageScreenScript;
    DamageScreenScript m_damageScreenScript_F;
    MainCamera m_mainCameraScript;

    private bool isWaitTime = false;

    // Start is called before the first frame update
    void Start()
    {
        m_playerScript = m_playerObject.GetComponent<PlayerScript>();
        m_playerHPScript = m_playerHPObject.GetComponent<PlayerHPScript>();
        m_damageScreenScript = m_DamageScreenObject.GetComponent<DamageScreenScript>();
        m_damageScreenScript_F = m_DamageScreenObject_F.GetComponent<DamageScreenScript>();
        m_mainCameraScript = Camera.main.GetComponent<MainCamera>();
    }

    //攻撃がプレイヤーに当たった時
    private void OnTriggerEnter(Collider col)
    {
        if (isWaitTime)
        {
            return;
        }

        //プレイヤーのHPにダメージを与える
        if (col.tag == "Player")
        {
            Debug.Log("Hit");
            m_playerHPScript.doTakeDamage(20);
            m_damageScreenScript.Damaged();
            m_damageScreenScript_F.Damaged();
            m_playerScript.m_audioSource.PlayOneShot(m_playerScript.m_damageSE);
            m_mainCameraScript.cameraStatus = CameraStatus.Third_Parson;

            isWaitTime = true;
        }
    }

    //攻撃後の攻撃不可時間
    async void doWait()
    {
        await UniTask.Delay(1000);
        isWaitTime = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isWaitTime == true)
        {
            doWait();
        }        
    }
}
