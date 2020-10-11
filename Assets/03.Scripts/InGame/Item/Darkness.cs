using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Steam_Item;
public class Darkness : ItemBase
{ // Item

    // 지속시간 Init
    void Start()
    {
        _curItem = (int)eitemNum.Darkness;

        _actionTime = new WaitForSeconds(1f);
    }

    public  IEnumerator Action(PlayerController getPlayer)
    {
        // 현재 아이템을 획득한 팀 Get
        eTeam getCharTeam = getPlayer.Team;

        // 전체 유저들(PlayerCharacter) Get
        var _playerControllers = GameManager.Instance.InGameManager.PlayerCharacters;

        // Darkness 효과
        // 적군 팀 
        // 화면 가림 이펙트 출력
        for (int i = 0; i < _playerControllers.Count; ++i)
        {
            // 적군팀에게 적용
            if (_playerControllers[i].Team == getCharTeam || _playerControllers[i] == null) { continue; }

            // 혹시 아이템을 또 획득할 시 지속시간을 연장시킬 수단(야매) 
            _playerControllers[i].OnEffect(_curItem);
        }

        yield return _actionTime;

        for (int i = 0; i < _playerControllers.Count; ++i)
        {
            if (_playerControllers[i].Team == getCharTeam || _playerControllers[i] == null) { continue; }

            // 지속시간 체크
            _playerControllers[i].OffEffect(_curItem);
        }

        LogManager.Log("Item End " + (eitemNum)_curItem);
    }
}
