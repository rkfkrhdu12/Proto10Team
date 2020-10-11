using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeMenuSystem : MonoBehaviour
{

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

    }
}
