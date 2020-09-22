using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

public class InGameManager : MonoBehaviour
{
    public CameraManager CameraManager;
    public Canvas Canvas;

    [SerializeField]
    private ScrollController _mouseSensitivityOptionScroll;

    public List<PlayerController> playerCharacters = new List<PlayerController>();

    [SerializeField]
    private TeamManager teamMgr = null;

    public void AddPlayer(PlayerController pCtrl)
    {
        playerCharacters.Add(pCtrl);
        teamMgr.Register(pCtrl);
    }

    private void Awake()
    {
        GameManager.Instance.InGameManager = this;

        _mouseSensitivityOptionScroll.Init();

        CameraManager.SetMouseSensitivity(in _mouseSensitivityOptionScroll.Data);

        GameManager.Instance.CurState = GameManager.eSceneState.InGame;
    }
}
