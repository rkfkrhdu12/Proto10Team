using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class InGameManager : MonoBehaviour
{
    private PhotonView _photonView;

    [SerializeField]
    private GameObject _optionObject;

    [SerializeField]
    private CameraManager _cameraManager;

    private void Awake()
    {
        GameManager.Instance.CurState = GameManager.eSceneState.InGame;

        _photonView = GetComponent<PhotonView>();

        if (!_photonView.IsMine) { enabled = false; }
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            _optionObject.SetActive(!_optionObject.activeSelf);

            _cameraManager._isActive = !_optionObject.activeSelf;
        }
    }
}
