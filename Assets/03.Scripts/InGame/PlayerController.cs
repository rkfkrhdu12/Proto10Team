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
    public float[] ItemEffectStateCount { get => _datas._itemEffect; set => _datas._itemEffect = value; }
    public GameObject HeadObj { get { return _curTeamObject.Head; } }

    public PhotonView _pView;

    [SerializeField]
    private PlayerObjectData _redObject = null;
    [SerializeField]
    private PlayerObjectData _blueObject = null;

    private PlayerObjectData _curTeamObject = null;

    public bool isInit = false;

    public void Init(eTeam team)
    {
        _datas._curTeam = team;

        _curTeamObject = Team == eTeam.Red ? _redObject : _blueObject;
        _curTeamObject.Active();
    }

    [PunRPC]
    void Register()
    {
        GameManager.Instance.InGameManager.AddPlayer(this);
    }

    void Awake()
    {
        _pView = GetComponent<PhotonView>();
        if (!_pView.IsMine) { return; }

        _pView.RPC("Register", RpcTarget.AllBuffered);

        MoveSpeed = 7.0f;
        JumpPower = 6.0f;
        ItemEffectStateCount = new float[6];
    }

    private void Start()
    {
        if (_curTeamObject == null)
            _curTeamObject = Team == eTeam.Red ? _redObject : _blueObject;

        LogManager.Log(_pView.Owner.NickName + " " + Team.ToString());

        isInit = true;
    }
}
