using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

public class Team
{
    private eTeam _team = eTeam.None;
    public eTeam CurTeam { get { return _team; } }

    public List<PlayerController> _Players = new List<PlayerController>();
    public CharacterUI[] _CharacterUI = null;

    public int CurPlayerCount { get { return _Players.Count - 1; } }
    private static int _maxPlayerCount = -1;
    public void Init(int maxPlayerCount, CharacterUI[] charUIs = null, eTeam curTeam = eTeam.None)
    {
        if (_maxPlayerCount <= -1) { _maxPlayerCount = maxPlayerCount; }

        if (_team == eTeam.None)
        {
            _team = curTeam;
        }

        if (_CharacterUI == null)
        {
            _CharacterUI = new CharacterUI[_maxPlayerCount];
            _CharacterUI = charUIs;
        }
    }

    public bool AddPlayer(PlayerController player)
    {
        if (player == null) { return false; }
        if (_Players == null) { _Players = new List<PlayerController>(); }

        if (CurPlayerCount < _maxPlayerCount - 1)
        {
            _Players.Add(player);

            _Players[CurPlayerCount].Init(CurTeam);

            _CharacterUI[CurPlayerCount].gameObject.SetActive(true);

            _CharacterUI[CurPlayerCount].Init(_Players[CurPlayerCount]);
            return true;
        }
        else
            return false;
    }

    public void RemovePlayer(PlayerController player)
    {
        if (player == null) { return; }

        // for()
    }
}

public class InGameManager : MonoBehaviourPunCallbacks
{
    private PhotonView _photonView;
    public PhotonView PView;

    [SerializeField]
    private GameObject _optionObject;

    public CameraManager CameraManager;

    [SerializeField]
    private ScrollController _mouseSensitivityOptionScroll;

    public Canvas Canvas;

    public Team RedTeam;
    public Team BlueTeam;

    [SerializeField]
    private CharacterUI[] _RedCharacterUIs;
    [SerializeField]
    private CharacterUI[] _BlueCharacterUIs;

    private const int _teamCount = 2;
    public int TeamCount => _teamCount;

    public List<PlayerController> GetPlayers(eTeam team)
    {
        switch (team)
        {
            case eTeam.Red: return RedTeam._Players;
            case eTeam.Blue: return BlueTeam._Players;
        }

        return null;
    }

    [PunRPC]
    public void LeftRoom(GameObject leftPlayer)
    {
        PlayerController pCtrl = leftPlayer.GetComponent<PlayerController>();
        if (pCtrl == null) { return; }

        for (int i = 0; i < RedTeam._Players.Count; ++i)
        {
            if (RedTeam._Players[i] == pCtrl)
            {


                return;
            }
        }

        for (int i = 0; i < BlueTeam._Players.Count; ++i)
        {
            if (BlueTeam._Players[i] == pCtrl)
            {

                return;
            }
        }
    }

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();

        RedTeam = new Team();
        RedTeam.Init(GameManager.Instance.NetManager.MaxPlayerCount / TeamCount, _RedCharacterUIs);
        BlueTeam = new Team();
        BlueTeam.Init(GameManager.Instance.NetManager.MaxPlayerCount / TeamCount, _BlueCharacterUIs);

        GameManager.Instance.InGameManager = this;

        _mouseSensitivityOptionScroll.Init();

        CameraManager.SetMouseSensitivity(in _mouseSensitivityOptionScroll.Data);

        GameManager.Instance.CurState = GameManager.eSceneState.InGame;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            _optionObject.SetActive(!_optionObject.activeSelf);

            CameraManager._isOption = !_optionObject.activeSelf;
        }
    }

    public void AddPlayer(int photonView)
    {
        _photonView.RPC("SetTeam", RpcTarget.AllBuffered, photonView);
    }

    [PunRPC]
    private void SetTeam(int photonView)
    {
        PhotonView pView = PhotonView.Find(photonView);
        if (pView == null) { return; }

        PlayerController pCtrl = pView.GetComponent<PlayerController>();
        if (pCtrl == null) { return; }

        if (!RedTeam.AddPlayer(pCtrl))
            BlueTeam.AddPlayer(pCtrl);
    }
}
