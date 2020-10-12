using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    // index, ViewID
    Dictionary<int, int> _indexViewidBox = new Dictionary<int, int>();

    [SerializeField]
    private CharacterUI[] _team1ChacUI = null;
    [SerializeField]
    private CharacterUI[] _team2ChacUI = null;

    int _maxTeamPlayerCount = 2;
    public void Register(PlayerController pCtrl)
    {
        if(pCtrl == null) { return; }

        List<PlayerController> pCtrls = GameManager.Instance.InGameManager.PlayerCharacters;

        List<int> searchPlayer = new List<int>();

        for (int i = 0; i < pCtrls.Count - 1; ++i)
        { // 이번에 추가된 pCtrl 뺀 나머지를 Search
            if (pCtrls[i] == null)
            { // 현재 유저 데이터가 제대로 되지않은 경우 Error
                LogManager.Log("Register DeletePlayer is Error !");
                GameManager.Instance.InGameManager.PlayerCharacters.Remove(pCtrls[i]);
                continue;
            }

            if (_indexViewidBox.ContainsValue(pCtrls[i]._pView.ViewID))
            {
                searchPlayer.Add(i);
            }
        }

        var keys = _indexViewidBox.Keys;

        foreach (int i in keys)
        {
            if (!searchPlayer.Contains(i))
            {
                _indexViewidBox.Remove(i);
                break;
            }
        }

        if (_indexViewidBox.ContainsValue(pCtrl._pView.ViewID)) { return; }

        for (int i = 0; i < _maxTeamPlayerCount * 2; ++i)
        {
            if (!_indexViewidBox.ContainsKey(i))
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

        _indexViewidBox.Add(index, pCtrl._pView.ViewID);
    }

    public void Remove(int viewId)
    {

    }
}
