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
    #region Variable
    PlayerData _datas;

    UnitController _uCtrl;

    RefData _moveSpeed = new RefData(); public ref RefData GetMoveSpeed() => ref _moveSpeed;
    RefData _jumpPower = new RefData(); public ref RefData GetJumpPower() => ref _jumpPower;
    RefData _power = new RefData(); public ref RefData GetPower() => ref _power;

    public eTeam Team { get => _datas._curTeam; set => _datas._curTeam = value; }
    public float MoveSpeed { get => _datas._moveSpeed; set => _datas._moveSpeed = _moveSpeed._Value = value; }
    public float Power { get => _datas._power; set => _datas._power = _power._Value = value; }
    public float JumpPower { get => _datas._jumpPower; set => _datas._jumpPower = _jumpPower._Value = value; }
    public float[] ItemEffectStateCount { get => _datas._itemEffect; set => _datas._itemEffect = value; }
    public GameObject HeadObj { get { return _curTeamObject.Head; } }
    public GameObject LeftHandObj { get { return _curTeamObject.LeftHand; } }
    public GameObject RightHandObj { get { return _curTeamObject.RightHand; } }

    public PhotonView _pView;

    [SerializeField]
    private PlayerObjectData _redObject = null;
    [SerializeField]
    private PlayerObjectData _blueObject = null;

    private PlayerObjectData _curTeamObject = null;

    public bool isInit = false;

    public enum eHandState
    {
        Default,
        Catch,
        Last,
    }

    eHandState _curHandState = eHandState.Default;
    public eHandState CurHandState { get { return _curHandState; } }

    bool _isJump;

    #endregion

    public void Init(eTeam team)
    {
        _datas._curTeam = team;

        _curTeamObject = Team == eTeam.Red ? _redObject : _blueObject;
        _curTeamObject.Active();

        LogManager.Log(_pView.Owner.NickName + " " + Team.ToString());
        isInit = true;
    }

    public void OnDizziy()
    {

    }

    public void OnStun()
    {

    }

    #region Monobehaviour Function

    void Awake()
    {
        _pView = GetComponent<PhotonView>();
        if (!_pView.IsMine) { return; }

        GameManager.Instance.PlayerCharacter = gameObject;

        MoveSpeed = 10.0f;
        JumpPower = 10.0f;
        ItemEffectStateCount = new float[6];
    }

    private void Start()
    {
        _pView.RPC("Register", RpcTarget.AllBuffered);
    }

    private void Update()
    {
        if (!_pView.IsMine) { return; }

        if (Input.GetMouseButton(0) && _curHandState != eHandState.Catch)
        {
            // 마우스 왼클릭을 누르고 있음
            // 잡기 모드

            Debug.Log("Catching Mode");

            _pView.RPC("OnChangeHand", RpcTarget.All, (int)eHandState.Catch);
        }
        else if (!Input.GetMouseButton(0) && _curHandState != eHandState.Default)
        {   // 마우스 왼클릭을 안 누르고 있음
            // 기본 모드

            Debug.Log("Default Mode");

            _pView.RPC("OnChangeHand", RpcTarget.All, (int)eHandState.Default);
        }
    }

    #endregion

    [PunRPC]
    void Register()
    {
        GameManager.Instance.InGameManager.AddPlayer(this);
    }

    [PunRPC]
    void OnChangeHand(int state)
    {
        if(state >= (int)eHandState.Last) { return; }

        _curHandState = (eHandState)state;
    }
}
