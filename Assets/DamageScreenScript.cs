using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DamageScreenScript : MonoBehaviour
{
    [SerializeField] Image DamageImg;

    // Start is called before the first frame update
    void Start()
    {
        DamageImg.color = Color.clear;
    }

    //“G‚©‚çUŒ‚‚ğó‚¯‚½
    public void Damaged()
    {
        //‰æ–Ê‚ğÔ‚­‚·‚é
        DamageImg.color = new Color(0.7f, 0, 0, 0.7f);
    }

    // Update is called once per frame
    void Update()
    {
        DamageImg.color = Color.Lerp(DamageImg.color, Color.clear, Time.deltaTime);
    }
}

