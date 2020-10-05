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

    public List<PlayerController> PlayerCharacters = new List<PlayerController>();

    [SerializeField]
    private TeamManager _teamMgr = null;

    [SerializeField]
    private Timer _Timer = null;

    public void AddPlayer(PlayerController pCtrl)
    {
        for (int i = 0; i < PlayerCharacters.Count; ++i)
        {
            if(PlayerCharacters[i] == null)
            {
                PlayerCharacters.Remove(PlayerCharacters[i]);
            }
        }

        PlayerCharacters.Add(pCtrl);
        _teamMgr.Register(pCtrl);
    }

    private void Awake()
    {
        GameManager.Instance.InGameManager = this;

        _mouseSensitivityOptionScroll.Init();

        CameraManager.SetMouseSensitivity(in _mouseSensitivityOptionScroll.Data);

        GameManager.Instance.CurState = GameManager.eSceneState.InGame;
    }

    [PunRPC]
    public void OnItemEvent(int eventNum)
    {

    }
}
