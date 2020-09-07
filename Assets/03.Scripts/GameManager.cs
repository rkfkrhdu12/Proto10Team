using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
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
    private string _playerName;
    // 현재 이름이 설정되어 있지 않으면 저장
    public string PlayerName
    {
        get { return _playerName; }
        set { if (string.IsNullOrWhiteSpace(_playerName)) _playerName = value; }
    }

    // 네트워크를 관리할 변수
    public NetworkManager NetManager;

    public GameObject _playerCharacter = null;
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