using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

public class Team
{
    public PlayerController[] _Players = null;
    public CharacterUI[] _CharacterUI = null;

    private int _curPlayerCount = 0;
    private static int _maxPlayerCount = -1;
    public void Init(int maxPlayerCount, CharacterUI[] charUIs = null)
    {
        if (_maxPlayerCount <= -1) { _maxPlayerCount = maxPlayerCount; }

        if (_Players == null)
        {
            _Players = new PlayerController[_maxPlayerCount];
        }

        if (_CharacterUI == null)
        {
            _CharacterUI = new CharacterUI[_maxPlayerCount];
            _CharacterUI = charUIs;
        }
    }

    public bool AddPlayer(PlayerController player)
    {
        if (_Players == null) { Init(2); }

        if (_curPlayerCount < _maxPlayerCount)
        {
            _Players[_curPlayerCount] = player;
            _CharacterUI[_curPlayerCount].gameObject.SetActive(true);

            _CharacterUI[_curPlayerCount].Init(_Players[_curPlayerCount]);

            ++_curPlayerCount;
            return true;
        }
        else
            return false;
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

    public PlayerController[] GetPlayers(eTeam team)
    {
        switch (team)
        {
            case eTeam.Red: return RedTeam._Players;
            case eTeam.Blue: return BlueTeam._Players;
        }

        return null;
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
