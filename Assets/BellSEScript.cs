using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellSEScript : MonoBehaviour
{
    [SerializeField] GameObject m_bellSEObject;
    [SerializeField] AudioClip m_bellSE;
    AudioSource m_AudioSource;
    bool m_IsPlaying = false;
    // Start is called before the first frame update
    void Start()
    {
       m_AudioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player"&&m_IsPlaying == false)
        {
            m_AudioSource.PlayOneShot(m_bellSE);
            m_IsPlaying = true;
        }
    }
}
