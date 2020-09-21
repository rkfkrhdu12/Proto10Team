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

        var applyObjects = GameManager.Instance.InGameManager.GetPlayers(getCharTeam);

        for (int i = 0; i < applyObjects.Count; ++i)
        {
            PlayerController pCtrl = applyObjects[i];

            pCtrl.MoveSpeed *= 2;
            pCtrl.Power *= 2;
            pCtrl.ItemEffectStateCount[(int)eitemNum.SuperPower] += 1;
        }

        yield return _actionTime;

        for (int i = 0; i < applyObjects.Count; ++i)
        {
            PlayerController pCtrl = applyObjects[i];

            pCtrl.ItemEffectStateCount[(int)eitemNum.SuperPower] -= 1;

            if (pCtrl.ItemEffectStateCount[(int)eitemNum.SuperPower] <= 0)
            {
                pCtrl.MoveSpeed *= 2;
                pCtrl.Power *= 2;
            }
        }
    }
}
