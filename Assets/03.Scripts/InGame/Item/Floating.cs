using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : ItemBase
{ // Item

    float _defaultGravity = 0.0f;
    // 지속시간 Init
    void Start()
    {
        _curItem = (int)eitemNum.Floating;
        _actionTime = new WaitForSeconds(10f);

        _defaultGravity = Physics.gravity.y;
    }

    public override IEnumerator Action(PlayerController getPlayer)
    {
        // 현재 아이템을 획득한 팀 Get
        eTeam getCharTeam = getPlayer.Team;

        // 전체 유저들(PlayerCharacter) Get
        playerControllers = GameManager.Instance.InGameManager.PlayerCharacters;

        // Floating 효과
        // 모든 팀 
        // 중력 .5배
        for (int i = 0; i < playerControllers.Count; ++i)
        {
            if (playerControllers[i] == null) continue;

            // 모든팀에게 적용
            // if (playerControllers[i].Team != getCharTeam) continue;

            if (playerControllers[i].ItemEffectStateCount[_curItem] == 0 && Physics.gravity.y == _defaultGravity)
            {
                Physics.gravity = new Vector3(Physics.gravity.x, _defaultGravity / 2, Physics.gravity.z);
            }
            // 혹시 아이템을 또 획득할 시 지속시간을 연장시킬 수단(야매) 
            playerControllers[i].ItemEffectStateCount[_curItem] += 1;
        }

        yield return _actionTime;

        for (int i = 0; i < playerControllers.Count; ++i)
        {
            if (playerControllers[i] == null) continue;

            // 지속시간 체크
            playerControllers[i].ItemEffectStateCount[_curItem] -= 1;

            // 지속시간 확인
            if (playerControllers[i].ItemEffectStateCount[_curItem] <= 0 && Physics.gravity.y != _defaultGravity)
            {
                Physics.gravity = new Vector3(Physics.gravity.x, _defaultGravity, Physics.gravity.z);
            }
        }

        LogManager.Log("Item End " + (eitemNum)_curItem);
    }
}
