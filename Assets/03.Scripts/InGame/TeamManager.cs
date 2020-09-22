using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    Dictionary<int, int> _viewIDIndex = new Dictionary<int, int>();

    [SerializeField]
    private CharacterUI[] _team1ChacUI = null;
    [SerializeField]
    private CharacterUI[] _team2ChacUI = null;

    int _maxTeamPlayerCount = 2;
    public void Register(PlayerController pCtrl)
    {
        if(_viewIDIndex.ContainsKey(pCtrl._pView.ViewID)) { return; }

        for (int i = 0; i < _maxTeamPlayerCount * 2; ++i)
        {
            if (!_viewIDIndex.ContainsValue(i))
            {
                if (_maxTeamPlayerCount > i)
                { // Red
                    InitUI(eTeam.Red, pCtrl, i);
                    break;
                }
                else
                { // Blue
                    InitUI(eTeam.Blue, pCtrl, i);
                    break;
                }
            }
        }
    }

    private void InitUI(eTeam team, PlayerController pCtrl, int index)
    {
        pCtrl.Init(team);

        (team == eTeam.Red ? _team1ChacUI : _team2ChacUI)[index % 2].Init(pCtrl);
        LogManager.Log(team + "  " + index.ToString());


        _viewIDIndex.Add(pCtrl._pView.ViewID, index);
    }

    public void Remove(int viewId)
    {

    }
}
