using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _itemBoxObjects = null;
    private int _indexCount { get { return _itemBoxObjects.Length; } }

    private int _curItemBoxIndex = 0;

    [SerializeField]
    private Transform[] _itemBoxSpawnPointTransform = null;

    private ItemBase.eitemNum _curItem;
    public ItemBase.eitemNum CurSelectItem { get { return _curItem; } }

    public void Spawn()
    {
        if (_itemBoxObjects == null) return;
        if (_itemBoxSpawnPointTransform == null || _indexCount == 0) return;

        int spawnPointIndex = Random.Range(0, _itemBoxSpawnPointTransform.Length);

        Vector3 spawnPoint = _itemBoxSpawnPointTransform[spawnPointIndex].position;

        _curItem = (ItemBase.eitemNum)Random.Range((int)ItemBase.eitemNum.SuperPower, (int)ItemBase.eitemNum.Floating + 1);

        LogManager.Log("spawnPointIndex :" + spawnPointIndex + " _curItem " + _curItem);

        GameObject curItemBox = _itemBoxObjects[_curItemBoxIndex];

        switch (_curItem)
        {
            case ItemBase.eitemNum.SuperPower:  curItemBox.AddComponent<SuperPower>();  break;
            case ItemBase.eitemNum.Stun:        curItemBox.AddComponent<Stun>();        break;
            case ItemBase.eitemNum.Dizzlness:   curItemBox.AddComponent<Dizziness>();   break;
            case ItemBase.eitemNum.Darkness:    curItemBox.AddComponent<Darkness>();    break;
            case ItemBase.eitemNum.Slowly:      curItemBox.AddComponent<Slowly>();      break;
            case ItemBase.eitemNum.Floating:    curItemBox.AddComponent<Floating>();    break;
        }

        curItemBox.transform.position = spawnPoint;
        curItemBox.gameObject.SetActive(true);

        ++_curItemBoxIndex;
    }
}
