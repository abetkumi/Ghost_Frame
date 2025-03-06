using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//カメラ用ステータス
public enum CameraStatus
{
    Third_Parson,
    First_Parson,
}

public class MainCamera : MonoBehaviour
{
    [SerializeField] GameObject m_playerObject;
    [SerializeField] GameObject m_cameraObject;
    [SerializeField] GameObject m_subcameraObject;
    [SerializeField] GameObject m_gameOverCameraPosObject;

    //初期設定用変数
    Vector3 initPos = new Vector3(18.0f, 14.0f, 5.0f);
    Vector3 angle;
    Vector3 primary_angle;
    Quaternion initRot = Quaternion.Euler(15.0f, 100.0f, 0.0f);
    float initFOV = 25.0f;
    Transform cameraFirstPos;
    public CameraStatus cameraStatus = CameraStatus.Third_Parson;
    // Start is called before the first frame update
    void Start()
    {
        //カメラ初期値用変数
        cameraFirstPos = gameObject.transform;
        angle = m_subcameraObject.transform.localEulerAngles;
        primary_angle = m_subcameraObject.transform.localEulerAngles;
    }

    //カメラの初期設定
    public void doInit()
    {
        transform.position = initPos;       //位置
        transform.rotation = initRot;       //向き
        Camera.main.fieldOfView = initFOV;  //画角
    }

    //カメラ移動メソッド
    public void doMove()
    {
        float vert = Input.GetAxis("C_Vertical");
        float horiz = Input.GetAxis("C_Horizontal");
        Vector3 cameraPos = m_cameraObject.transform.position - m_playerObject.transform.position;

        //3人称視点
        if (cameraStatus == CameraStatus.Third_Parson)
        {
            //カメラの制限
            if (cameraPos.y < 0.0f && vert < 0.0f)
            {
                vert = 0.0f;
            }
            else if (cameraPos.y > 3.0f && vert > 0.0f)
            {
                vert = 0.0f;
            }
            //カメラの回転
            m_playerObject.transform.RotateAround(m_playerObject.transform.position, Vector3.up, horiz);
            m_cameraObject.transform.RotateAround(m_playerObject.transform.position, m_cameraObject.transform.right, vert);
        }

        //1人称視点
        else if (cameraStatus == CameraStatus.First_Parson)
        {
            //カメラの回転
            m_subcameraObject.transform.Rotate(Vector3.right * vert * 60.0f * Time.deltaTime);
            m_playerObject.transform.RotateAround(m_subcameraObject.transform.position, Vector3.up, horiz * 0.5f);
        }

        //1人称視点、3人称視点の切り替え
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetButtonDown("Camera"))
        {
            if (m_cameraObject.activeSelf)
            {     
                cameraStatus = CameraStatus.First_Parson;
                Shader.DisableKeyword("_DITHERING_ON");
            }
            else
            {
                cameraStatus = CameraStatus.Third_Parson;
                Shader.EnableKeyword("_DITHERING_ON");
            }
        }
    }

    //ゲームオーバー時のカメラ
    public void doGameOverCamera()
    {
        cameraStatus = CameraStatus.Third_Parson;
        transform.position = m_gameOverCameraPosObject.transform.position;
        transform.LookAt(m_playerObject.transform.position);
    }

    //クリア時のカメラの処理
    public void doClearCamera(Vector3 pos)
    {
        cameraStatus = CameraStatus.Third_Parson;
        transform.position = pos + new Vector3(0.0f, 1.0f, 0.0f);
        transform.LookAt(m_playerObject.transform.position);
    }

    //カメラのステータス状態
    public void doCameraStatus()
    {
        switch (cameraStatus)
        {
            case CameraStatus.Third_Parson:
                m_cameraObject.gameObject.SetActive(true);
                m_subcameraObject.gameObject.SetActive(false);
                break;
            case CameraStatus.First_Parson:
                m_cameraObject.gameObject.SetActive(false);
                m_subcameraObject.gameObject.SetActive(true);
                break;
        }
    }
    
    void Update()
    {

    }
}
