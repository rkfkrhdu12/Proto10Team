using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public struct PlayerData
{
    public eTeam _curTeam;

    // 움직일 속도
    public float _moveSpeed;
    public float _defaultMoveSpeed;

    // 점프할 힘
    public float _jumpPower;
    public float _defaultJumpPower;

    // 물체를 들어올리는 힘
    public float _power;
    public float _defaultPower;

    public float[] _itemEffect;
}

public class PlayerController : MonoBehaviour
{
    PlayerData _datas;

    UnitController _uCtrl;

    RefData _moveSpeed  = new RefData(); public ref RefData GetMoveSpeed()  => ref _moveSpeed;
    RefData _jumpPower  = new RefData(); public ref RefData GetJumpPower()  => ref _jumpPower;
    RefData _power      = new RefData(); public ref RefData GetPower()      => ref _power;

    public eTeam Team { get => _datas._curTeam; set => _datas._curTeam = value; }
    public float MoveSpeed { get => _datas._moveSpeed;  set => _datas._moveSpeed = _moveSpeed._Value = value; }
    public float Power { get => _datas._power;  set => _datas._power = _power._Value = value; }
    public float JumpPower { get => _datas._jumpPower;  set => _datas._jumpPower = _jumpPower._Value = value; }
    public float[] ItemEffect { get => _datas._itemEffect; set => _datas._itemEffect = value; }

    public PhotonView _pView;

    void Awake()
    {
        _pView = GetComponent<PhotonView>();

        if(!_pView.IsMine) { return; }

        GameManager.Instance.InGameManager.AddPlayer(_pView.ViewID);

        MoveSpeed = 7.0f;
        JumpPower = 6.0f;
        ItemEffect = new float[6];
    }

    void Update()
    {
        if(!_pView.IsMine) { return; }
    }
}
