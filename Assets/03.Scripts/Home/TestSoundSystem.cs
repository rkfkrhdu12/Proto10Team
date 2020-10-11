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
    public AudioSource audioSource;
    public AudioClip[] audioClips;

    public float volumeScale;
    public void Init()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        
        //for (int i = 0; i < audioSources.Length; i++)
        //{
        //    audioSources[i].playOnAwake = false;
        //}
    }



    private void Awake()
    {
        Init();

    }
    // Start is called before the first frame update
    void Start()
    {
  
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.volume = volumeScale;
    }
    public void SetVolume(float v)
    {
        audioSource.volume = v;
    }
    public void PlayOneShotBGM(int index)
    {
        audioSource.PlayOneShot(audioClips[index], volumeScale);
        LogManager.Log("Play.");

    }
    public void SetLoop(int index, bool b)
    {
        audioSource.loop = b;
    }
    public void StopBGM(int index)
    {
        audioSource.Stop();
    }
}
