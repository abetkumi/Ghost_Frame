using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideEnemyScript : MonoBehaviour
{
    [SerializeField] GameObject m_hideEnemyObject;
    // Start is called before the first frame update

    private void OnTriggerEnter(Collider col)
    {
        if(col.tag == ("Player"))
        {
            m_hideEnemyObject.gameObject.SetActive(true);
        }
   
    }
}
