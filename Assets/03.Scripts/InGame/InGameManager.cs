using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

public class Team
{
    public PlayerController[] _Players  = new PlayerController[_maxPlayerCount];
    public CharacterUI[] _CharacterUI    = new CharacterUI[_maxPlayerCount];

    private int _curPlayerCount = 0;
    private static readonly int _maxPlayerCount = GameManager.Instance.NetManager.MaxPlayerCount / GameManager.Instance.InGameManager.TeamCount;

    public bool Init(PlayerController player, CharacterUI[] charUIs)
    {
        if (_curPlayerCount < _maxPlayerCount)
        {
            _Players[_curPlayerCount] = player;
            _CharacterUI[_curPlayerCount] = charUIs[_curPlayerCount];

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

    [SerializeField]
    private GameObject _optionObject;

    public CameraManager CameraManager;

    [SerializeField]
    private ScrollController _mouseSensitivityOptionScroll;

    public Canvas Canvas;

    public Team RedTeam = new Team();
    public Team BlueTeam = new Team();

    [SerializeField]
    private CharacterUI[] _RedCharacterUIs;
    [SerializeField]
    private CharacterUI[] _BlueCharacterUIs;

    //public PlayerController[] RedTeam = new PlayerController[2];
    //[SerializeField]
    //private GameObject[] RedCharacterUI = new GameObject[2];
    //int _curRedTeamCount = 0;
    //public PlayerController[] BlueTeam = new PlayerController[2];
    //[SerializeField]
    //private GameObject[] BlueCharacterUI = new GameObject[2];
    //int _curBlueTeamCount = 0;

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

        // if (!_photonView.IsMine) { enabled = false; }

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

        if (!RedTeam.Init(pCtrl, _RedCharacterUIs))
            BlueTeam.Init(pCtrl, _BlueCharacterUIs);
    }
}
