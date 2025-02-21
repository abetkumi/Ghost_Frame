using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TitleButtonScript : MonoBehaviour
{
    [SerializeField] Button m_focusButton;

    // Start is called before the first frame update
    void Start()
    {
        m_focusButton = m_focusButton.GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
