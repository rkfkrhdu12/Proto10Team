using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class ItemManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject[] _itemBoxObjects = null;
    private int _indexCount { get { return _itemBoxObjects.Length; } }

    private int _curItemBoxIndex = 0;

    [SerializeField]
    private Transform _itemBoxSpawnPoint = null;

    private ItemBase.eitemNum _curItem;
    public ItemBase.eitemNum CurSelectItem { get { return _curItem; } }

    PhotonView _pView = null;

    private void Start()
    {
        _pView = gameObject.GetPhotonView();
    }

    public void Spawn()
    {
        if (_itemBoxObjects == null) return;
        if (_itemBoxSpawnPoint == null || _indexCount == 0) return;
        if (_pView == null)
        {
            _pView = GameManager.Instance.PlayerCharacter.GetPhotonView();
        }

        _curItem = (ItemBase.eitemNum)Random.Range((int)ItemBase.eitemNum.SuperPower, (int)ItemBase.eitemNum.Floating + 1);

        LogManager.Log(" _curItem " + _curItem);

        _pView.RPC("OnItemSpawn", RpcTarget.All, _curItem);
    }

    [PunRPC]
    private void OnItemSpawn(int curItem)
    {
        GameObject curItemBox = _itemBoxObjects[_curItemBoxIndex];
        Vector3 spawnPoint = _itemBoxSpawnPoint.position;

        switch (curItem)
        {
            case (int)ItemBase.eitemNum.SuperPower: curItemBox.AddComponent<SuperPower>();  break;
            case (int)ItemBase.eitemNum.Stun:       curItemBox.AddComponent<Stun>();        break;
            case (int)ItemBase.eitemNum.Dizzlness:  curItemBox.AddComponent<Dizziness>();   break;
            case (int)ItemBase.eitemNum.Darkness:   curItemBox.AddComponent<Darkness>();    break;
            case (int)ItemBase.eitemNum.Slowly:     curItemBox.AddComponent<Slowly>();      break;
            case (int)ItemBase.eitemNum.Floating:   curItemBox.AddComponent<Floating>();    break;
        }

        curItemBox.transform.position = spawnPoint;
        curItemBox.gameObject.SetActive(true);

        ++_curItemBoxIndex;
    }
}
