using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScript : MonoBehaviour
{
    //　回転スピード
    [SerializeField] private float m_rotateSpeed = 0.5f;
    //　スカイボックスのマテリアル
    private Material m_skyboxMaterial;
    // Start is called before the first frame update
    void Start()
    {
        //LightingSettingsで指定したスカイボックスのマテリアルを取得
        m_skyboxMaterial = RenderSettings.skybox;
    }

    async public UniTask LoadScene(string sceneName)
    {
        await SceneManager.LoadSceneAsync(sceneName).ToUniTask();
    }

    // Update is called once per frame
    void Update()
    {
        //　スカイボックスマテリアルのRotationを操作して角度を変化させる
        m_skyboxMaterial.SetFloat("_Rotation", Mathf.Repeat(m_skyboxMaterial.GetFloat("_Rotation") + m_rotateSpeed * Time.deltaTime, 360f));
    }
}
