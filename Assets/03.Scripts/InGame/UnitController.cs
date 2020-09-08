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
    private float _moveSpeed = 7;

    Vector3 _curVelocity = Vector3.zero;

    private void Awake()
    {
        if (!_photonView.IsMine) { enabled = false; return; }

        GameManager.Instance.PlayerCharacter = gameObject;
    }

    private void FixedUpdate()
    {
        if (!_photonView.IsMine) { return; }

        _targetPos.x += Input.GetAxis("Horizontal") * Time.fixedDeltaTime * _moveSpeed;
        _targetPos.y = transform.position.y;
        _targetPos.z += Input.GetAxis("Vertical") * Time.fixedDeltaTime * _moveSpeed;

        transform.position = Vector3.SmoothDamp(transform.position, _targetPos, ref _curVelocity, 1f);
    }
}
