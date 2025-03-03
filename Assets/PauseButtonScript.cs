using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public class PauseButtonScript : MonoBehaviour
{
    [SerializeField] Button m_focusButton;
    bool m_startLoading = false;

    // Start is called before the first frame update
    void Start()
    {
        m_startLoading = false;
        m_focusButton = m_focusButton.GetComponent<Button>();
    }

    public void doGamePauseButton()
    {
        if (!m_startLoading)
        {
            m_startLoading = true;
            Time.timeScale = 1.0f;
            //メインゲームシーンに移動する
            SceneManager.LoadScene("Title");
            Debug.Log("タイトルに戻る");
        }
    }
}
