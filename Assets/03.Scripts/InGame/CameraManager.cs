using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using GameCamera;

namespace GameCamera
{
    class CameraData
    {
        public int _mouseSensitivity;
    }
}
public class CameraManager : MonoBehaviour
{
    private Transform _playerCharTransform;
    private Transform _pivotTransform;

    private CameraData _data = new CameraData();

    private Vector3 _curVelocity = Vector3.zero;
    public bool _isActive = true;


    public void SetMouseSensitivity(ref int Value)
    {
        _data._mouseSensitivity = Value;
    }

    private int _mouseSensitivity { get { return _data._mouseSensitivity; } }

    void Start()
    {
        if (_playerCharTransform == null)
        {
            if (GameManager.Instance != null)
                if (GameManager.Instance.PlayerCharacter != null)
                    _playerCharTransform = GameManager.Instance.PlayerCharacter.transform;
        }

        _pivotTransform = transform;
    }

    public float smoothTime = .1f;

    void LateUpdate()
    {
        if(_playerCharTransform == null)
        {
            if (GameManager.Instance != null)
                if (GameManager.Instance.PlayerCharacter != null)
                    _playerCharTransform = GameManager.Instance.PlayerCharacter.transform;

            return;
        }

        if(!_isActive) { return; }

        // Position Update


        _pivotTransform.position = Vector3.SmoothDamp(_pivotTransform.position, _playerCharTransform.position, ref _curVelocity, smoothTime);

        // Rotation Update
        float horizontal = Input.GetAxis("Mouse X");

        _pivotTransform.localEulerAngles += new Vector3(0, horizontal * _mouseSensitivity * Time.deltaTime, 0);
    }
}
