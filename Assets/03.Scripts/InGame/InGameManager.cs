using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class InGameManager : SceneManager
{
    private PhotonView _photonView;

    [SerializeField]
    private GameObject _optionObject;

    [SerializeField]
    private CameraManager _cameraManager;

    [SerializeField]
    private ScrollController _mouseSensitivityOptionScroll;

    private void Awake()
    {
        GameManager.Instance.CurState = GameManager.eSceneState.InGame;

        _photonView = GetComponent<PhotonView>();

        if (!_photonView.IsMine) { enabled = false; }

        _mouseSensitivityOptionScroll.Init();

        _cameraManager.SetMouseSensitivity(in _mouseSensitivityOptionScroll.Data);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            _optionObject.SetActive(!_optionObject.activeSelf);

            _cameraManager._isOption = !_optionObject.activeSelf;
        }
    }
}
