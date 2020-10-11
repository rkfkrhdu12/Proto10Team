using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using Steam_Item;
public class Dizziness : ItemBase
{ // Item

    PlayerController target = null;

    // 지속시간 Init
    private void Start()
    {
        _curItem = (int)eitemNum.Dizzlness;
        _actionTime = new WaitForSeconds(3f);
    }

    public  IEnumerator Action(PlayerController getPlayer)
    {
        // 현재 아이템을 획득한 팀 Get
        eTeam getCharTeam = getPlayer.Team;

        // 전체 유저들(PlayerCharacter) Get
        var _playerControllers = GameManager.Instance.InGameManager.PlayerCharacters;

        // Dizziness 효과
        // 적팀 팀 
        // 한명에게 조작키를 반대로 바꿔버림
        for (int i = 0; i < _playerControllers.Count; ++i)
        {
            // 적군팀에게 적용
            if (_playerControllers[i].Team == getCharTeam || _playerControllers[i] == null) { _playerControllers.Remove(_playerControllers[i]); continue; }
        }

        if (_playerControllers.Count != 0)
        {
            target = _playerControllers[Random.Range(0, _playerControllers.Count)];

            // PlayerController 에서 직접 작업
            target.OnEffect(_curItem);
        }

        yield return _actionTime;

        for (int i = 0; i < _playerControllers.Count; ++i)
        {
            if (target == null) continue;

            // 지속시간 체크
            target.OffEffect(_curItem);
        }

        LogManager.Log("Item End " + (eitemNum)_curItem);
    }
}
