using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleCameraScript : MonoBehaviour
{
    //Camera m_camera;
    Vector3 m_initPos = new Vector3(90.0f, 4.7f, 63.0f);
    Vector3 m_cameraPos = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        //m_camera = Camera.main.GetComponent<Camera>();
        m_cameraPos = m_initPos;
        transform.position = m_cameraPos;
    }

    // Update is called once per frame
    void Update()
    {
        m_cameraPos.x -= Time.deltaTime * 2.0f;
        transform.position = m_cameraPos;
        if(m_cameraPos.x < 0 )
        {
            m_cameraPos.x = m_initPos.x;
        }
    }
}
