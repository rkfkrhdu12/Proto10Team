using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperPower : ItemBase
{ // Item

    // 지속시간 Init
    void Start()
    {
        _actionTime = new WaitForSeconds(7f);
    }

    public override IEnumerator Action(PlayerController getPlayer)
    {
        // 현재 아이템을 획득한 팀 Get
        eTeam getCharTeam = getPlayer.Team;

        // 전체 유저들(PlayerCharacter) Get
        playerControllers = GameManager.Instance.InGameManager.playerCharacters;

        // SuperPower 효과
        // 아군 팀 
        // 이동속도, 힘 2배로 증가
        for (int i = 0; i < playerControllers.Count; ++i)
        {
            if (playerControllers[i] == null) continue;

            // 아군팀에게 적용
            if (playerControllers[i].Team != getCharTeam) continue;

            playerControllers[i].MoveSpeed *= 2;
            playerControllers[i].Power *= 2;

            // 혹시 아이템을 또 획득할 시 지속시간을 연장시킬 수단(야매) 
            playerControllers[i].ItemEffectStateCount[(int)eitemNum.SuperPower] += 1;
        }

        yield return _actionTime;

        for (int i = 0; i < playerControllers.Count; ++i)
        {
            if (playerControllers[i] == null) continue;

            // 지속시간 체크
            playerControllers[i].ItemEffectStateCount[(int)eitemNum.SuperPower] -= 1;

            // 지속시간 확인
            if (playerControllers[i].ItemEffectStateCount[(int)eitemNum.SuperPower] <= 0)
            {
                playerControllers[i].MoveSpeed *= 2;
                playerControllers[i].Power *= 2;
            }
        }
    }
}
