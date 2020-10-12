using System.Collections;
using System.Collections.Generic;
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
        DontDestroyOnLoad(this.gameObject);

    }
    // Start is called before the first frame update
    void Start()
    {
  
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
        LogManager.Log("Play.");

    }

    public void PlayOneShotSFX(int index)
    {

    }
    public void SetLoop(int index, bool b)
    {
        bgmSource.loop = b;
    }
    public void StopBGM(int index)
    {
        bgmSource.Stop();
    }
}
