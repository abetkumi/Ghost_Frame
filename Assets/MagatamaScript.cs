using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Linq;

public class MagatamaScript : MonoBehaviour
{
    [SerializeField] GameObject m_mistWallObject;
    MistWallScript m_mistWallScript;
    [SerializeField] private AudioClip m_magatamaBreakSE;
    private AudioSource m_audioSource;

    // Start is called before the first frame update
    void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
        m_mistWallScript = m_mistWallObject.GetComponent<MistWallScript>();
    }

    //勾玉の破壊処理
    public async void doBreak()
    {
        //勾玉が壊れるSE
        m_audioSource.PlayOneShot(m_magatamaBreakSE);
        //壊れた時に飛んでいく処理
        var random = new System.Random();
        var min = -3;
        var max = 3;
        gameObject.GetComponentsInChildren<Rigidbody>().ToList().ForEach(r => {
            r.isKinematic = false;
            r.transform.SetParent(null);
            var vect = new Vector3(random.Next(min, max), random.Next(0, max), random.Next(min, max));
            r.AddForce(vect, ForceMode.Impulse);
            r.AddTorque(vect, ForceMode.Impulse);
        });
        //1秒待つ
        await UniTask.Delay(1000);
        //オブジェクト破棄
        Destroy(gameObject);
        //霧のオブジェクト破棄
        m_mistWallScript.doBreak();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * 180.0f * Time.deltaTime);
    }
}
