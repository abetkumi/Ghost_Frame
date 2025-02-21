using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatalAttackScript : MonoBehaviour
{
    [SerializeField] GameObject m_attackObject;
    AttackScript m_attackScript;
    // Start is called before the first frame update
    void Start()
    {
        m_attackScript = m_attackObject.GetComponent<AttackScript>();
    }

    //フェイタル攻撃ができる時
    private void FatalStart()
    {
        m_attackScript.isShot = true;
        m_attackScript.m_attack_current = m_attackScript.m_attack_max;
    }

    //フェイタル攻撃ができなくなった時
    private void FatalEnd()
    {
        m_attackScript.isShot = false;
        m_attackScript.m_attack_current = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
