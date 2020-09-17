using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public enum eTeam
{
    Red,
    Blue,
}

public class NetworkManager : MonoBehaviourPunCallbacks
{
    /// <summary>
    /// 랜덤매칭에 들어간다.
    /// </summary>
    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    private const int _maxPlayerCount = 4;
    public int MaxPlayerCount { get { return _maxPlayerCount; }}

    #region Variable

    WaitForSeconds _playerSpawnTime = new WaitForSeconds(.5f);


    #endregion

    #region Private Function

    private IEnumerator WaitPlayerSpawn()
    {
        bool isEnd = false;

        while (!isEnd)
        {
            yield return _playerSpawnTime;

            if (GameManager.Instance.CurState == GameManager.eSceneState.InGame)
            {
                PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);

                isEnd = true;
            }
        }
    }

    #endregion

    #region Monobehaviour Function
    private void Start()
    {
        if (GameManager.Instance.NetManager == null)
        {
            GameManager.Instance.NetManager = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    #region Photon Callback Function

    public override void OnDisconnected(DisconnectCause cause)
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(2);

        if (GameManager.Instance.PlayerCharacter == null)
            StartCoroutine(WaitPlayerSpawn());
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = _maxPlayerCount });
    }
    #endregion
}
