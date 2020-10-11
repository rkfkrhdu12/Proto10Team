using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Steam_Item;
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

    public  IEnumerator Action(PlayerController getPlayer)
    {
        // Floating 효과
        // 모든 팀 
        // 중력 .5배
        // 모든팀에게 적용

        if (Physics.gravity.y == _defaultGravity)
        {
            Physics.gravity = new Vector3(Physics.gravity.x, _defaultGravity / 2, Physics.gravity.z);
        }

        yield return _actionTime;

        // 지속시간 확인
        if (Physics.gravity.y != _defaultGravity)
        {
            Physics.gravity = new Vector3(Physics.gravity.x, _defaultGravity, Physics.gravity.z);
        }

        LogManager.Log("Item End " + (eitemNum)_curItem);
    }
}
