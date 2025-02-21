using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public class GameClearScript : MonoBehaviour
{
    [SerializeField] GameObject m_playerObject;
    [SerializeField] GameObject m_lastEnemyObject;
    PlayerScript m_playerScript;
    [SerializeField] GameObject m_clearPoint;
    [SerializeField] GameObject m_cameraClearPositionObject;
    [SerializeField] GameObject m_cameraClearPositionObject2;
    public Vector3 m_position;
    MainCamera m_cameraScript;
    public bool isNear = false;
    // Start is called before the first frame update
    void Start()
    {
        m_position = m_clearPoint.transform.position;
        m_cameraScript = Camera.main.GetComponent<MainCamera>();
        m_playerScript = m_playerObject.GetComponent<PlayerScript>();
    }

    //ゲームクリア処理
    public void doGameClear()
    {
     
        if (isNear == true)
        {
            m_position = m_cameraClearPositionObject2.transform.position;
            m_playerScript.doClearAnim(m_position);
            m_cameraScript.doClearCamera(m_lastEnemyObject.transform.position);
            m_lastEnemyObject.SetActive(true);
        }
        else
        {
            m_playerScript.doClearAnim(m_position);
            m_cameraScript.doClearCamera(m_cameraClearPositionObject.transform.position+Vector3.up);
            // 目的地に十分近づいたら、最終演出
            if ((m_playerScript.m_dest - m_playerScript.transform.position).magnitude < 0.5f)
            {
                isNear = true;
                m_playerScript.transform.position = new Vector3(23.0f, 0.0f, 145.0f);
                m_cameraScript.transform.position = transform.position;
                enabled = false;
            }
        }

        Debug.Log("ゲームクリア");
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
