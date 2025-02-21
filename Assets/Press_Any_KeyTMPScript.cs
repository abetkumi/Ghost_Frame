using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Press_Any_KeyTMPScript : MonoBehaviour
{
    TextMeshProUGUI myTMP;
    // Start is called before the first frame update
    void Start()
    {
        myTMP = GetComponent<TextMeshProUGUI>();
    }

    public void doColor(float t)
    {
        myTMP.color = new Color(1.0f, 1.0f, 1.0f, t);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
