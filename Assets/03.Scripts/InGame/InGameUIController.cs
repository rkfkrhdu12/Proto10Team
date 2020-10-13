using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;
using UnityEngine.UI;
using TMPro;

public class InGameUIController : MonoBehaviour
{
    [SerializeField]
    Image _startDownUI = null;
    [SerializeField]
    TMP_Text _countDownText = null;
    [SerializeField]
    AudioClip[] _countSounds = null;
    private Queue<AudioClip> _countDownSounds = new Queue<AudioClip>();

    [Space]
    [SerializeField]
    Image _recipeUI = null;

    [Space]
    [SerializeField]
    Image _feverUI = null;
    [SerializeField]
    Image _timerUI = null;
    [SerializeField]
    AudioClip _feverSound = null;

    [Space]
    [SerializeField]
    Image _timeOutUI = null;
    [SerializeField]
    AudioClip _timeOutSound = null;

    [Space, Space]
    [SerializeField]
    private AudioSource _audio;

    private void Awake()
    {
        if (_countSounds == null || _countDownSounds.Count == 0) return;

        for (int i = 0; i < _countSounds.Count(); ++i)
        {
            if (_countSounds[i] == null) { continue; }

            _countDownSounds.Enqueue(_countSounds[i]);
        }
    }

    public void OnStartUI()
    {
        if (_startDownUI != null)
            _startDownUI.gameObject.SetActive(true);
    }

    public void OnCountDownUI()
    {
        if (_countDownSounds != null && _audio != null && _countDownSounds.Count != 0)
            _audio.PlayOneShot(_countDownSounds.Dequeue());

        if (_countDownText != null)
            _countDownText.text = _countDownSounds.Count().ToString();
    }

    public void OnRecipeUI()
    {
    }

    public void OnFeverUI()
    {
        if (_feverSound != null && _audio != null)
            _audio.PlayOneShot(_feverSound);

        if (_feverUI != null)
            _feverUI.gameObject.SetActive(true);
    }

    public void OnTimeOutUI()
    {
        if (_timeOutSound != null && _audio != null)
            _audio.PlayOneShot(_timeOutSound);

        if (_timeOutUI != null)
            _timeOutUI.gameObject.SetActive(true);
    }
}
