using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeMenuSystem : MonoBehaviour
{
    public GameObject logoObj;
    public GameObject settingErrorButtonObj;

    public float speed;
    public float scaleVal;

    public TestSoundSystem bgmPlayer;

    public Slider bgmSlider;

    public Slider soundEffectSlider;

    private void Awake()
    {
        settingErrorButtonObj.SetActive(false);
    }
    private void Start()
    {
        bgmPlayer.PlayOneShotBGM(0);
        bgmPlayer.audioSource.loop = true;
    }
    void Update()
    {
        logoObj.gameObject.transform.localScale = new Vector3(Mathf.PingPong(Time.time*speed, scaleVal)+1, Mathf.PingPong(Time.time*speed, scaleVal)+1, 1);
        bgmPlayer.SetVolume(bgmSlider.value);
    }
    public void GoGameStart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("IntroScene");
    }
    public void GoExitGame()
    {
        Application.Quit();
    }
    public void GoSetting()
    {
        settingErrorButtonObj.SetActive(true);
    }
    public void SettingErrorButtonClose()
    {
        settingErrorButtonObj.SetActive(false);
    }
   
}
