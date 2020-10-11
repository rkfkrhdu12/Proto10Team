using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using Steam_Item;
public class Stun : ItemBase
{ // Item

    PlayerController target = null;
    // 지속시간 Init
    void Start()
    {
        _curItem = (int)eitemNum.Stun;
        _actionTime = new WaitForSeconds(3f);
    }

    public  IEnumerator Action(PlayerController getPlayer)
    {
        // 현재 아이템을 획득한 팀 Get
        eTeam getCharTeam = getPlayer.Team;

        // 전체 유저들(PlayerCharacter) Get
        var _playerControllers = GameManager.Instance.InGameManager.PlayerCharacters;

        // Stun 효과
        // 적군 팀 
        // 한명 스턴
        for (int i = 0; i < _playerControllers.Count; ++i)
        {
            // 적군팀에게 적용
            if (_playerControllers[i].Team == getCharTeam ||                     // 아군이거나
                _playerControllers[i] == null)                                 // Null이거나
            {
                _playerControllers.Remove(_playerControllers[i]);
                continue;
            }
        }

        if (_playerControllers.Count != 0)
        {
            target = _playerControllers[Random.Range(0, _playerControllers.Count)];

            if(target.ItemEffectStateCount[_curItem] == 0)
            {
                target.MoveSpeed = 0;
            }

            // PlayerController 에서 직접 작업
            target.OnEffect(_curItem);
        }

        yield return _actionTime;

        // 지속시간 체크
        target.OffEffect(_curItem);

        if (target.ItemEffectStateCount[_curItem] == 0)
        {
            target.ResetMoveSpeed();
        }

        LogManager.Log("Item End " + (eitemNum)_curItem);
    }
}
