using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Cysharp.Threading.Tasks;
using TMPro;

public class TitleButtonScript : MonoBehaviour
{
    [SerializeField] Button m_focusButton;
    [SerializeField] Image m_loadingObject;
    [SerializeField] TextMeshProUGUI m_startButtonTextUI;
    [SerializeField] TextMeshProUGUI m_endButtonTextUI;
    [SerializeField] TextMeshProUGUI m_loadingTextUI;
    [SerializeField] GameObject m_titleManagerObject;
    TitleScript m_titleScript;
    bool m_startLoading = false;
    float t = 0.0f;
    float m_loadtext = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        m_titleScript = m_titleManagerObject.GetComponent<TitleScript>();
        m_focusButton = m_focusButton.GetComponent<Button>();
        m_loadingObject.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        m_loadingTextUI.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    }

    // ボタンが押された場合、今回呼び出される関数
    async public void OnClickStartButton()
    {
        if (!m_startLoading)
        {
            m_startLoading = true;
            await UniTask.Delay(3000);
            //メインゲームシーンに移動する
            m_titleScript.LoadScene("School").Forget();
            Debug.Log("ゲームスタート!");  // ログを出力
        }
  
    }

    public void OnClickEndButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
    }

    // Update is called once per frame
    void Update()
    {
        if (m_startLoading)
        {
            t += Time.deltaTime;
            m_loadtext += Time.deltaTime/2.0f;

            m_startButtonTextUI.color = new Color(1.0f, 1.0f, 1.0f, 1.0f - t);
            m_endButtonTextUI.color = new Color(1.0f, 1.0f, 1.0f, 1.0f - t);
            m_loadingObject.color = new Color(0.0f, 0.0f, 0.0f, t);
            m_loadingTextUI.color = new Color(1.0f, 1.0f, 1.0f, m_loadtext);
            if (t > 1.0f)
            {
                t = 1.0f;
            }
            if (m_loadtext > 1.0f)
            {
                m_loadtext = 0.0f;
            }
        }
    }
}
