using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public enum eApplyTeam
{
    Friend,
    Enemy,
    All,
}
namespace Steam_Item
{
    public enum eitemNum
    {
        SuperPower,
        Stun,
        Dizzlness, // 크아 악마
        Darkness,
        Slowly,
        Floating,
    }

    public class ItemBase : MonoBehaviour
    {
        protected int _curItem;
        protected ItemManager _itemMgr;
        protected WaitForSeconds _actionTime;
        protected virtual void Start()
        {
            _itemMgr = GameManager.Instance.InGameManager.GetComponent<ItemManager>();
        }

        protected virtual void OnTriggerEnter(Collider other)
        {

        }
    }

}