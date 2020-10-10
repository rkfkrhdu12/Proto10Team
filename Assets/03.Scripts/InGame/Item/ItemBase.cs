using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eApplyTeam
{
    Friend,
    Enemy,
    All,
}

public class ItemBase : MonoBehaviour
{
    public enum eitemNum
    {
        SuperPower      ,
        Stun            , // Stun
        Dizzlness       , // 크아 악마
        Darkness        ,
        Slowly          ,
        Floating        ,
    }

    protected int _curItem;

    protected List<PlayerController> playerControllers;

    public virtual IEnumerator Action(PlayerController getPlayer) { yield return null; }
    protected WaitForSeconds _actionTime = null;

    readonly string _playerTagName = "Player";

    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_playerTagName))
        {
            LogManager.Log("Item Action " + (eitemNum)_curItem);
            StartCoroutine(Action(other.GetComponent<PlayerController>()));
            gameObject.SetActive(false);
        }
    }
}
