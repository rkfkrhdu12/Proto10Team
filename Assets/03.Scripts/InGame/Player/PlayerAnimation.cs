using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField]
    private Animator _anim = null;

    [SerializeField]
    private UnitController _uCtrl = null;

    private struct AniKey
    {
        public const string MoveValue = "MoveValue";
        public const string TakeObjectWeight = "TakeObjectWeight";
        public const string IsTake = "IsTake";
        public const string IsFloating = "IsFloating";
        public const string Expect = "Expect";
        public const string Win = "Win";
        public const string Lose = "Lose";
    }

    void Awake()
    {
        if (_anim == null) { _anim = GetComponent<Animator>(); }
    }

    void LateUpdate()
    {
        if (_uCtrl == null) { return; }

        LogManager.Log(" " + _uCtrl.MoveDelta + "   " + _uCtrl.IsJump);

        // _anim.SetFloat(AniKey.MoveValue, _uCtrl.MoveDelta);
    }
}
