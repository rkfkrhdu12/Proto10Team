using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Stun : ItemBase
{ // Item

    PlayerController target = null;
    // 지속시간 Init
    void Start()
    {
        _actionTime = new WaitForSeconds(3f);
    }

    public override IEnumerator Action(PlayerController getPlayer)
    {
        // 현재 아이템을 획득한 팀 Get
        eTeam getCharTeam = getPlayer.Team;

        // 전체 유저들(PlayerCharacter) Get
        playerControllers = GameManager.Instance.InGameManager.PlayerCharacters;

        // Stun 효과
        // 적군 팀 
        // 한명 스턴
        for (int i = 0; i < playerControllers.Count; ++i)
        {
            if (playerControllers[i] == null) continue;

            // 아군팀에게 적용
            if (playerControllers[i].Team == getCharTeam) { playerControllers.Remove(playerControllers[i]); continue; }

            if (playerControllers[i].ItemEffectStateCount[_curItem] == 0)
            {
                var targets = playerControllers.OrderBy(x => x.Team != getCharTeam).ToList();

                target = targets[Random.Range(0, targets.Count)];

                // PlayerController 에서 직접 작업
                target.ItemEffectStateCount[_curItem] += 1;
            }
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
