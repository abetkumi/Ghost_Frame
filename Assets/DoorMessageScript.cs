using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cysharp.Threading.Tasks;

public enum MessageStatus
{
    NoMessage,
    DoorOpen,
    DoorClose,
    MistWallClose,
    MistWallBreak,
}
public class DoorMessageScript : MonoBehaviour
{
    [SerializeField] GameObject myTMPObject;
    [SerializeField] TextMeshProUGUI myTMP;
    [SerializeField] GameObject myImage;
    Image m_messageWindow;
    public MessageStatus m_messageStatus;
    float t = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        m_messageWindow = myImage.GetComponent<Image>();
        m_messageWindow.color = new Color(1.0f, 1.0f, 1.0f, t);
        myTMP = myTMPObject.GetComponent<TextMeshProUGUI>();
        myTMP.color = new Color(1.0f, 1.0f, 1.0f, t);
        t = 0.0f;
    }

    void doDoorOpenMessage()
    {
        t += Time.deltaTime * 2.0f;
        if (t > 1.0f)
        {
            t = 1.0f;
        }
        myTMP.color = new Color(1.0f, 1.0f, 1.0f, t * t);
        m_messageWindow.color = new Color(1.0f, 1.0f, 1.0f, t * t);
        myTMP.text = "LBでドアを開く";
    }

    void doDoorCloseMessage()
    {
        t += Time.deltaTime * 2.0f;
        if (t > 1.0f)
        {
            t = 1.0f;
        }
        myTMP.color = new Color(1.0f, 1.0f, 1.0f, t * t);
        m_messageWindow.color = new Color(1.0f, 1.0f, 1.0f, t * t);
        myTMP.text = "LBでドアを閉じる";
    }

    void doMistWallCloseMessage()
    {
        t += Time.deltaTime * 2.0f;
        if (t > 1.0f)
        {
            t = 1.0f;
        }
        myTMP.color = new Color(1.0f, 1.0f, 1.0f, t * t);
        m_messageWindow.color = new Color(1.0f, 1.0f, 1.0f, t * t);
        myTMP.text = "先に進めないようだ…";
    }

    async void doMistWallBreakMessage()
    {
        t += Time.deltaTime * 2.0f;
        if (t > 1.0f)
        {
            t = 1.0f;
        }
        myTMP.color = new Color(1.0f, 1.0f, 1.0f, t * t);
        m_messageWindow.color = new Color(1.0f, 1.0f, 1.0f, t * t);
        myTMP.text = "フロアの空気が変わったようだ…";

        await UniTask.Delay(3000);
        m_messageStatus = MessageStatus.NoMessage;
    }

    async void doMessageOff()
    {
        await UniTask.Delay(1000);
        t -= Time.deltaTime;
        if (t < 0.0f)
        {
            t = 0.0f;
        }
        myTMP.color = new Color(1.0f, 1.0f, 1.0f, t * t);
        m_messageWindow.color = new Color(1.0f, 1.0f, 1.0f, t * t);
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_messageStatus)
        {
            case MessageStatus.NoMessage:
                doMessageOff();
                break;
            case MessageStatus.DoorOpen:
                doDoorOpenMessage();
                break;
            case MessageStatus.DoorClose:
                doDoorCloseMessage();
                break;
            case MessageStatus.MistWallClose:
                doMistWallCloseMessage();
                break;
            case MessageStatus.MistWallBreak:
                doMistWallBreakMessage();
                break;
        }
    }
}
