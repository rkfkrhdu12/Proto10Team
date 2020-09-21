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
    protected enum eitemNum
    {
        SuperPower,
        Twinkle, // Stun
        Dizzle, // 크아 악마
        Dark,
        Slow,
        Float,
    }

    public virtual IEnumerator Action(PlayerController getPlayer) { yield return null; }
    protected WaitForSeconds _actionTime = null;

    readonly string _playerTagName = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_playerTagName))
        {
            StartCoroutine(Action(other.GetComponent<PlayerController>()));
        }
    }
}
