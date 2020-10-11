using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeMenuSystem : MonoBehaviour
{
    public GameObject logoObj;
    public GameObject settingErrorButtonObj;

    public float speed;
    public float scaleVal;
    private void Awake()
    {
        settingErrorButtonObj.SetActive(false);
    }
    void Update()
    {
        logoObj.gameObject.transform.localScale = new Vector3(Mathf.PingPong(Time.time*speed, scaleVal)+1, Mathf.PingPong(Time.time*speed, scaleVal)+1, 1);
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
