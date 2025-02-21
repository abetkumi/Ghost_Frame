using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitKeyScript : MonoBehaviour
{
    //　ドアエリアに入っているかどうか
    private bool isNear;
    //　ドアのアニメーター
    private Animator m_animator;

    void Start()
    {
        isNear = false;
        m_animator = GetComponent<Animator>();
    }


    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            isNear = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            isNear = false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown("f") && isNear)
        {
            m_animator.SetBool("Open", !m_animator.GetBool("Open"));
        }
    }
}
