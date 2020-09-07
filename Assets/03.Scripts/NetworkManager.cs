using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    /// <summary>
    /// 랜덤매칭에 들어간다.
    /// </summary>
    public void JoinRandomRoom()
    {

    }

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

    #endregion
}
