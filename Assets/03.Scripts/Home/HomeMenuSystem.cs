using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using TMPro;

public class HomeMenuSystem : MonoBehaviour
{
    public GameObject logoObj;
    public GameObject settingErrorButtonObj;

    public float speed;
    public float scaleVal;

    public TestSoundSystem bgmPlayer;

    public Slider bgmSlider;
    public Slider sfxSlider;

    public TMP_Text versionText;

    private int introSceneIndex;

    public GameObject enterTextGroup;

    private void Awake()
    {
        enterTextGroup.SetActive(false);
        introSceneIndex = UnityEngine.SceneManagement.SceneManager.GetSceneByName("IntroScene").buildIndex;
        settingErrorButtonObj.SetActive(false);
    }

    private void Start()
    {
        Time.maximumDeltaTime = 1f / 3f;
        Time.fixedDeltaTime = 1f / 60f;
        Application.targetFrameRate = 60;
        useGUILayout = false;
        Screen.SetResolution(960, 540, false);
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
        enterTextGroup.SetActive(true);
    }
    public void GoLobbyScene()
    {
        PhotonNetwork.LoadLevel(1);
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
