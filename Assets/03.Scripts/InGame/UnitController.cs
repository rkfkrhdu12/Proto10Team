using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class UnitController : MonoBehaviour
{
    [SerializeField]
    private PhotonView _photonView;

    Vector3 _targetPos;

    [SerializeField,Range(3,20)]
    private float _moveSpeed = 10;

    Vector3 _curVelocity = Vector3.zero;

    private Transform _transform;

    private void Awake()
    {
        if (!_photonView.IsMine) { enabled = false; return; }

        GameManager.Instance.PlayerCharacter = gameObject;

        _transform = transform;
    }

    public float smoothTime = 5;

    private void FixedUpdate()
    {
        if (!_photonView.IsMine) { return; }

        _targetPos.x += Input.GetAxis("Horizontal") * Time.fixedDeltaTime * _moveSpeed;
        _targetPos.y = _transform.position.y;
        _targetPos.z += Input.GetAxis("Vertical") * Time.fixedDeltaTime * _moveSpeed;

        transform.position = Vector3.Lerp(_transform.position, _targetPos,Time.deltaTime * smoothTime);
    }
}
