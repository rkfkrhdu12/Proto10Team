using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

public class HomeMenuSystem : MonoBehaviour
{
    public GameObject logoObj;
    public GameObject settingErrorButtonObj;

    public float speed;
    public float scaleVal;

    public TestSoundSystem bgmPlayer;

    public Slider bgmSlider;
    public Slider sfxSlider;

    private void Awake()
    {
        settingErrorButtonObj.SetActive(false);
    }
    private void Start()
    {
            bgmPlayer.PlayOneShotBGM(0);
            bgmPlayer.bgmSource.loop = true;
    }
    void Update()
    {
        logoObj.gameObject.transform.localScale = new Vector3(Mathf.PingPong(Time.time*speed, scaleVal)+1, Mathf.PingPong(Time.time*speed, scaleVal)+1, 1);
        //bgmPlayer.SetVolume(bgmSlider.value);
        PlayerPrefs.SetFloat("BgmVolume", bgmSlider.value);
        PlayerPrefs.SetFloat("sfxVolume", sfxSlider.value);
    }
    public void GoGameStart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("InGameScene");
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
