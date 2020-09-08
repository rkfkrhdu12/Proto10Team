using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraManager : MonoBehaviour
{
    private Transform _playerCharTransform;

    private Vector3 _curVelocity = Vector3.zero;

    public bool _isActive = true;

    private int _mouseSensitivity = 80;

    [SerializeField]
    private ScrollController _scrCtrl;

    void Start()
    {
        if (_playerCharTransform == null)
        {
            if (GameManager.Instance != null)
                if (GameManager.Instance.PlayerCharacter != null)
                    _playerCharTransform = GameManager.Instance.PlayerCharacter.transform;
        }
    }

    void Update()
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
        transform.position = Vector3.SmoothDamp(transform.position, _playerCharTransform.position, ref _curVelocity, .35f);

        // Rotation Update
        float horizontal = Input.GetAxis("Mouse X");

        transform.eulerAngles += new Vector3(0, horizontal * _scrCtrl.Value * Time.deltaTime, 0);
    }
}
