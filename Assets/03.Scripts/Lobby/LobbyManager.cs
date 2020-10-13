using NetWork;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public GameObject testLauncher;
    public void JoinRandomRoom()
    {
        GameManager.Instance.NetManager.JoinRandomRoom();
    }
}
