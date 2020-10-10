using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slowly : ItemBase
{ // Item

    // 지속시간 Init
    void Start()
    {
        _curItem = (int)eitemNum.Slowly;
        _actionTime = new WaitForSeconds(3f);
    }

    public override IEnumerator Action(PlayerController getPlayer)
    {
        // 현재 아이템을 획득한 팀 Get
        eTeam getCharTeam = getPlayer.Team;

        // 전체 유저들(PlayerCharacter) Get
        playerControllers = GameManager.Instance.InGameManager.PlayerCharacters;

        // Slowly 효과
        // 적군 팀 
        // 이동속도 1/2배로 증가
        for (int i = 0; i < playerControllers.Count; ++i)
        {
            if (playerControllers[i] == null) continue;

            // 적군팀에게 적용
            if (playerControllers[i].Team == getCharTeam) { playerControllers.Remove(playerControllers[i]); continue; }

            if (playerControllers[i].ItemEffectStateCount[_curItem] == 0)
            {
                playerControllers[i].MoveSpeed *= .5f;
            }

            // 혹시 아이템을 또 획득할 시 지속시간을 연장시킬 수단(야매) 
            playerControllers[i].ItemEffectStateCount[_curItem] += 1;
        }

        yield return _actionTime;

        playerControllers = GameManager.Instance.InGameManager.PlayerCharacters;

        for (int i = 0; i < playerControllers.Count; ++i)
        {
            if (playerControllers[i] == null) continue;

            if (playerControllers[i].Team == getCharTeam) continue;

            // 지속시간 체크
            playerControllers[i].ItemEffectStateCount[_curItem] -= 1;

            // 지속시간 확인
            if (playerControllers[i].ItemEffectStateCount[_curItem] <= 0)
            {
                playerControllers[i].MoveSpeed *= 2;
            }
        }

        LogManager.Log("Item End " + (eitemNum)_curItem);
    }
}
