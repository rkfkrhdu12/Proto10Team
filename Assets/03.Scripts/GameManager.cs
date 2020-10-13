using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    struct GameEndData
    {
        public int teamNumber;
    }
    public static GameManager Instance
    {
        get
        {
            if(_instance == null) _instance = new GameManager();

            return _instance;
        }
    }
    private static GameManager _instance;

    // 현재 유저의 이름
    private string _playerName = "";
    // 현재 이름이 설정되어 있지 않으면 저장
    public string PlayerName
    {
        get { return _playerName; }
        set { if (string.IsNullOrWhiteSpace(_playerName)) _playerName = value; }
    }

    public int PlayerTeam;

    // 네트워크를 관리할 변수
    private NetworkManager _netManager = null;
    public NetworkManager NetManager 
    {
        get { return _netManager; }
        set { if (_netManager == null) _netManager = value; }
    }

    private InGameManager _ingameManager = null;
    public InGameManager InGameManager 
    {
        get { return _ingameManager; }
        set { if (_ingameManager == null) _ingameManager = value; }
    }

    // InGame의 캐릭터 오브젝트
    private GameObject _playerChar = null;
    public GameObject PlayerCharacter 
    {
        get { return _playerChar; }
        set { if (_playerChar == null)  _playerChar = value; }
    }

    private CameraManager _cameraMgr = null;
    public CameraManager CameraManager
    {
        get { return _cameraMgr; }
        set { if (_cameraMgr == null) _cameraMgr = value; }
    }

    // 현재 씬의 상태
    public enum eSceneState
    {
        Loading,
        Intro,
        Lobby,
        InGame,
    }
    public eSceneState CurState = eSceneState.Intro;

    public int MouseSensivity = 0;
}

public class LogManager
{
    public static void Log(string str)
    {
        #if UNITY_EDITOR
        Debug.Log(str);
        #endif
    }
}