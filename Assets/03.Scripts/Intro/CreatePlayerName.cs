using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePlayerName : MonoBehaviour
{
    /// <summary>
    /// 유저 이름 만들기 시작
    /// </summary>
    /// <param name="playerPrefKey"></param>
    public void OnStart(string playerPrefKey)
    {
        _playerPrefKey = playerPrefKey;

        _curState = eCreateNameState.Making;
    }

    /// <summary>
    /// InputField 의 값을 업데이트
    /// </summary>
    /// <param name="name"></param>
    public void UpdateName(string name)
    {
        _name = name;
    }

    /// <summary>
    /// PlayerPref에 유저 이름 저장
    /// </summary>
    public void SaveName()
    {
        if (string.IsNullOrWhiteSpace(_name)) { return; }

        PlayerPrefs.SetString(_playerPrefKey, _name);

        _curState = eCreateNameState.Check;

        gameObject.SetActive(false);
    }

    #region Variable
    public enum eCreateNameState
    {
        Unknown,
        Making,
        Check,
    }
    // 유저이름 PrefKey 변수
    private string _playerPrefKey;

    // InputField에 적힌 유저이름
    [SerializeField]
    private string _name;

    // 현재 만들 이름의 상태
    [SerializeField]
    eCreateNameState _curState = eCreateNameState.Unknown;
    public eCreateNameState CurState { get { return _curState; } } 
    #endregion

}
