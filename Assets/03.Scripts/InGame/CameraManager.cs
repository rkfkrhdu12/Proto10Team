using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Transform _playerCharTransform;

    private Vector3 _curVelocity = Vector3.zero;

    void Start()
    {
        if (GameManager.Instance != null)
            if (GameManager.Instance._playerCharacter != null)
                _playerCharTransform = GameManager.Instance._playerCharacter.transform;
    }

    void FixedUpdate()
    {
        if(_playerCharTransform == null)
        {
            _playerCharTransform = GameManager.Instance._playerCharacter.transform;

            return;
        }

        // Position Update
        transform.position = Vector3.SmoothDamp(transform.position, _playerCharTransform.position, ref _curVelocity, .35f);

        // Rotation Update
        float horizontal = Input.GetAxis("Mouse X");

        transform.eulerAngles += new Vector3(0, horizontal, 0);
    }
}
