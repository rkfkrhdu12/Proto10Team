using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TestSoundSystem : MonoBehaviour
{
    //enum BgmName
    //{
    //    Home,
    //    Lobby,
    //    InGame

    //}

    //private BgmName bgmName;

    string nowSceneName;


    public AudioSource bgmSource;
    public AudioSource sfxSource;

    public AudioClip[] bgmClips;
    public AudioClip[] sfxClips;
   // public float volumeScale;
    public void Init()
    {
        nowSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        bgmSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();

        //for (int i = 0; i < audioSources.Length; i++)
        //{
        //    audioSources[i].playOnAwake = false;
        //}
    }

    private void Awake()
    {
        Init();

        GameManager.Instance.NetManager.JoinRandomRoom();
    }
    // Start is called before the first frame update
    void Start()
    {
        switch (nowSceneName)
        {
            case "HomeScene":
                PlayOneShotBGM(0);
                bgmSource.loop = true;
                break;
            case "LobbyScene":
                PlayOneShotBGM(1);
                bgmSource.loop = true;
                break;

            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()   
    {
        //audioSource.volume = volumeScale;
        bgmSource.volume = PlayerPrefs.GetFloat("BgmVolume", 0.1f);
        sfxSource.volume = PlayerPrefs.GetFloat("sfxVolume", 0.1f);
    }
    public void SetVolume(float v)
    {
        bgmSource.volume = v;
    }
    public void SetSFXVolume(float v)
    {
        sfxSource.volume = v;
    }
    public void PlayOneShotBGM(int index)
    {
        bgmSource.PlayOneShot(bgmClips[index]);
        LogManager.Log("BGM Play.");

    }

    public void PlaySFX(int index)
    {

        sfxSource.clip = sfxClips[index];
        sfxSource.Play();
        LogManager.Log("SFX Play.");
    }
    public void PlayOneShotSFX(int index)
    {
        sfxSource.PlayOneShot(sfxClips[index]);
        LogManager.Log("SFX Play.");
    }

    public void SetLoop(bool b)
    {
        bgmSource.loop = b;
    }
    public void SetSFXLoop(bool b)
    {
        sfxSource.loop = b;
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }
    public void StopSFX()
    {
        sfxSource.Stop();

    }

}
