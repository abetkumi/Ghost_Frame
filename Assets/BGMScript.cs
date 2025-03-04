using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BGMStatus
{
    NormalBGM,
    ChaseBGM,
    //GameOverBGM,
    //GameClearBGM,
}

public class BGMScript : MonoBehaviour
{
    [SerializeField] GameObject m_enemySearchObject;
    EnemySearchScript m_enemySearchScript;
    [SerializeField] AudioClip m_normalBGM;
    [SerializeField] AudioClip m_chaseBGM;
    AudioSource m_audioSource;
    public BGMStatus m_BGMstatus = BGMStatus.NormalBGM;

    // Start is called before the first frame update
    void Start()
    {
        m_enemySearchScript = m_enemySearchObject.GetComponent<EnemySearchScript>();
        m_audioSource = GetComponent<AudioSource>();
       // m_audioSource.Play();
    }

    public void BGMChange()
    {
        switch (m_BGMstatus)
        {
            case BGMStatus.NormalBGM:
                m_audioSource.clip = m_normalBGM;
                if (!m_audioSource.isPlaying)
                {
                    m_audioSource.Play();
                }
                break;
            case BGMStatus.ChaseBGM:
                m_audioSource.clip = m_chaseBGM;
                if (!m_audioSource.isPlaying)
                {
                    m_audioSource.Play();
                }              
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
