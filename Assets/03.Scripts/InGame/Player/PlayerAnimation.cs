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
        public const string MoveDelta = "MoveDelta";
        public const string JumpDelta = "JumpDelta";
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

        _anim.SetFloat(AniKey.MoveDelta, _uCtrl.MoveDelta);
        _anim.SetFloat(AniKey.JumpDelta, _uCtrl.JumpDelta);
        _anim.SetBool(AniKey.IsFloating, _uCtrl.IsJump);
    }
}
