using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Darkness : ItemBase
{ // Item

    // 지속시간 Init
    void Start()
    {
        _curItem = (int)eitemNum.Darkness;

        _actionTime = new WaitForSeconds(1f);
    }

    public override IEnumerator Action(PlayerController getPlayer)
    {
        // 현재 아이템을 획득한 팀 Get
        eTeam getCharTeam = getPlayer.Team;

        // 전체 유저들(PlayerCharacter) Get
        playerControllers = GameManager.Instance.InGameManager.PlayerCharacters;

        // Darkness 효과
        // 적군 팀 
        // 화면 가림 이펙트 출력
        for (int i = 0; i < playerControllers.Count; ++i)
        {
            if (playerControllers[i] == null) continue;

            // 적군팀에게 적용
            if (playerControllers[i].Team == getCharTeam) { playerControllers.Remove(playerControllers[i]); continue; }

            // 혹시 아이템을 또 획득할 시 지속시간을 연장시킬 수단(야매) 
            playerControllers[i].ItemEffectStateCount[_curItem] += 1;
        }

        yield return _actionTime;

        for (int i = 0; i < playerControllers.Count; ++i)
        {
            if (playerControllers[i] == null) continue;

            // 지속시간 체크
            playerControllers[i].ItemEffectStateCount[_curItem] -= 1;
        }

        LogManager.Log("Item End " + (eitemNum)_curItem);
    }
}
