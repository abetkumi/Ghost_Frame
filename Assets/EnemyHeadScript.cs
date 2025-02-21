using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHeadScript : MonoBehaviour
{
    [SerializeField] EnemyScript enemyScript;
    [SerializeField] private SphereCollider sphereCollider;

    public void TakeDamage(int damage)
    {
        enemyScript.TakeDamage(damage);
    }

    //Ž€‚ñ‚¾Œã“ª‚Ì“–‚½‚è”»’è‚ð‚È‚­‚·
    public void doDeadHead()
    {
        sphereCollider.enabled = false;
    }
}
