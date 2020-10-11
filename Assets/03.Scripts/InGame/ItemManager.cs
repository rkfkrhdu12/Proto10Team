using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using Steam_Item;
using System.Linq;

public class ItemManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _itemBoxObjects = null;
    private int _indexCount { get { return _itemBoxObjects.Length; } }

    private int _curItemBoxIndex = 0;

    [SerializeField]
    private Transform _itemBoxSpawnPoint = null;

    private eitemNum _curItem;
    public eitemNum CurSelectItem { get { return _curItem; } }

    eTeam _itemGetTeam = eTeam.None;

    PhotonView _pView = null;

    public delegate void RPCFunction(int team);
    public Queue<RPCFunction> OnRPC = new Queue<RPCFunction>();

    private List<RPCFunction> _rpcList = null;

    public void AddRPC(RPCFunction func)
    {
        OnRPC.Enqueue(func);

        _rpcList = OnRPC.OrderBy((x) => x != null).ToList();
        _pView.RPC("AddItemEffect", RpcTarget.All);
    }

    [PunRPC]
    private void AddItemEffect()
    {
        for (int i = 0; i < _rpcList.Count; ++i)
        {
            if (OnRPC.Contains(_rpcList[i])) { continue; }

            OnRPC.Enqueue(_rpcList[i]);
        }
    }

    public void ActiveRPC()
    {
        _pView.RPC("OnRpc", RpcTarget.All);
    }

    [PunRPC]
    private void OnRpc()
    {
        if (OnRPC.Count != 0)
        {
            OnRPC.Dequeue()((int)_itemGetTeam);
        }
    }

    public void SetGetItemTeam(eTeam team)
    {
        _pView.RPC("SetGetItemTeam", RpcTarget.All, (int)team);
    }

    [PunRPC]
    private void SetGetItemTeam(int team)
    {
        _itemGetTeam = (eTeam)team;
    }

    public void Spawn()
    {
        if (_itemBoxObjects == null) return;
        if (_itemBoxSpawnPoint == null || _indexCount == 0) return;

        if (_pView == null) { _pView = gameObject.GetPhotonView(); }

        _curItem = (int)eitemNum.SuperPower; // (ItemBase.eitemNum)Random.Range((int)ItemBase.eitemNum.SuperPower, (int)ItemBase.eitemNum.Floating + 1);

        LogManager.Log("_curItem " + _curItem);

        _pView.RPC("OnItemSpawn", RpcTarget.All, (int)_curItem);
    }

    [PunRPC]
    private void OnItemSpawn(int curItem)
    {
        GameObject curItemBox = _itemBoxObjects[_curItemBoxIndex++];
        Vector3 spawnPoint = _itemBoxSpawnPoint.position;

        switch (curItem)
        {
            case (int)eitemNum.SuperPower: curItemBox.AddComponent<SuperPower>();  break;
            case (int)eitemNum.Stun:       curItemBox.AddComponent<Stun>();        break;
            case (int)eitemNum.Dizzlness:  curItemBox.AddComponent<Dizziness>();   break;
            case (int)eitemNum.Darkness:   curItemBox.AddComponent<Darkness>();    break;
            case (int)eitemNum.Slowly:     curItemBox.AddComponent<Slowly>();      break;
            case (int)eitemNum.Floating:   curItemBox.AddComponent<Floating>();    break;
        }

        curItemBox.transform.position = spawnPoint;
        curItemBox.gameObject.SetActive(true);
    }


    private void Start()
    {
        _pView = gameObject.GetPhotonView();

        if (!_pView.IsMine) { enabled = false; return; }
    }
}
