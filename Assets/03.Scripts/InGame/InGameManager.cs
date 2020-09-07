using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class InGameManager : MonoBehaviour
{
    void Start()
    {
        PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
    }

}
