using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Steam_Item;

public class SuperPower : ItemBase
{ // Item

    // 지속시간 Init
    protected override void Start()
    {
        base.Start();

        _curItem = (int)eitemNum.SuperPower;
        _actionTime = new WaitForSeconds(7f);
    }

    protected void Enable(int team)
    {
        // 전체 유저들(PlayerCharacter) Get
        var playerControllers = GameManager.Instance.InGameManager.PlayerCharacters;

        // SuperPower 효과
        // 아군 팀 
        // 이동속도, 힘 2배로 증가
        for (int i = 0; i < playerControllers.Count; ++i)
        {
            // 아군팀에게 적용
            if (playerControllers[i].Team != (eTeam)team || playerControllers[i] == null) { continue; }

            if (playerControllers[i].ItemEffectStateCount[_curItem] == 0)
            {
                LogManager.Log(playerControllers[i].gameObject.GetPhotonView().ToString());
                playerControllers[i].MoveSpeed *= 2;
                playerControllers[i].Power *= 2;
            }
            // 혹시 아이템을 또 획득할 시 지속시간을 연장시킬 수단(야매)
            playerControllers[i].OnEffect(_curItem);
        }
    }

    protected  void Disable(int team)
    {
        // 전체 유저들(PlayerCharacter) Get
        var playerControllers = GameManager.Instance.InGameManager.PlayerCharacters;

        for (int i = 0; i < playerControllers.Count; ++i)
        {
            if (playerControllers[i] == null) continue;
            if (playerControllers[i].Team != (eTeam)team) { continue; }

            // 지속시간 체크
            playerControllers[i].OffEffect(_curItem);

            // 지속시간 확인
            if (playerControllers[i].ItemEffectStateCount[_curItem] <= 0)
            {
                playerControllers[i].ResetMoveSpeed();
                playerControllers[i].ResetPower();
            }
        }

        LogManager.Log("Item End " + (eitemNum)_curItem);
    }

    public IEnumerator Action(PlayerController getPlayer)
    {
        if (getPlayer == null) { yield break; }

        _itemMgr.SetGetItemTeam(getPlayer.Team);

        _itemMgr.AddRPC(Enable);
        _itemMgr.ActiveRPC();

        yield return _actionTime;

        _itemMgr.AddRPC(Disable);
        _itemMgr.ActiveRPC();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController pCtrl = other.GetComponent<PlayerController>();
            if (pCtrl != null)
            {
                if (pCtrl.gameObject.GetPhotonView().IsMine)
                {
                    LogManager.Log("Item Action " + (eitemNum)_curItem);

                    StartCoroutine(Action(pCtrl));
                }
            }

            GetComponent<BoxCollider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
