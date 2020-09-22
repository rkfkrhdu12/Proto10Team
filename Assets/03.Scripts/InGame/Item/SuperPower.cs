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

        playerControllers = GameManager.Instance.InGameManager.playerCharacters;

        for (int i = 0; i < playerControllers.Count; ++i)
        {
            if (playerControllers[i].Team != getCharTeam) continue;

            playerControllers[i].MoveSpeed *= 2;
            playerControllers[i].Power *= 2;
            playerControllers[i].ItemEffectStateCount[(int)eitemNum.SuperPower] += 1;
        }

        yield return _actionTime;

        playerControllers = GameManager.Instance.InGameManager.playerCharacters;

        for (int i = 0; i < playerControllers.Count; ++i)
        {
            playerControllers[i].ItemEffectStateCount[(int)eitemNum.SuperPower] -= 1;

            if (playerControllers[i].ItemEffectStateCount[(int)eitemNum.SuperPower] <= 0)
            {
                playerControllers[i].MoveSpeed *= 2;
                playerControllers[i].Power *= 2;
            }
        }
    }
}
