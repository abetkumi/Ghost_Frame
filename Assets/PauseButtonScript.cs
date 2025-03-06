using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public class PauseButtonScript : MonoBehaviour
{
    [SerializeField] public Button m_focusButton_Title;
    [SerializeField] Button m_focusButton_GameBack;
    bool m_startLoading = false;

    // Start is called before the first frame update
    void Start()
    {
        m_startLoading = false;
        m_focusButton_Title = m_focusButton_Title.GetComponent<Button>();
        m_focusButton_GameBack = m_focusButton_GameBack.GetComponent<Button>();
        m_focusButton_Title.Select();
    }

    public async void doGamePauseButton()
    {
        if (!m_startLoading)
        {
            EventSystem.current.SetSelectedGameObject(null);
            m_focusButton_Title.Select();
            m_startLoading = true;
            Time.timeScale = 1.0f;
            await UniTask.Delay(1000);
            //メインゲームシーンに移動する
            SceneManager.LoadScene("Title");
            Debug.Log("タイトルに戻る");
        }
    }
}
