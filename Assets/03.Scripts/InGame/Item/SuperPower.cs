using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperPower : ItemBase
{ // Item

    void Start()
    {
        _actionTime = new WaitForSeconds(7f);
    }

    public override IEnumerator Action(PlayerController getPlayer)
    {
        eTeam getCharTeam = getPlayer.Team;

        _applyObjects = GameManager.Instance.NetManager.GetTeam(getCharTeam);

        for (int i = 0; i < _applyObjects.Length; ++i)
        {
            PlayerController pCtrl = _applyObjects[i];

            pCtrl.MoveSpeed *= 2;
            pCtrl.Power *= 2;
            pCtrl.ItemEffect[(int)eitemNum.SuperPower] += 1;
        }

        yield return _actionTime;


        for (int i = 0; i < _applyObjects.Length; ++i)
        {
            PlayerController pCtrl = _applyObjects[i];

            pCtrl.ItemEffect[(int)eitemNum.SuperPower] -= 1;

            if (pCtrl.ItemEffect[(int)eitemNum.SuperPower] <= 0)
            {
                _applyObjects[i].MoveSpeed *= 2;
                _applyObjects[i].Power *= 2;
            }
        }
    }
}
