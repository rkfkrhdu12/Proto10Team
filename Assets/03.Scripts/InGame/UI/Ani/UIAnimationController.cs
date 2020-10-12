using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimationController : MonoBehaviour
{
    [SerializeField]
    protected Animator _ani;
    [SerializeField]
    protected string _startKey = "Play";

    public void OnEnable()
    {
        _ani.SetTrigger("_startKey");
    }

    public virtual void OnEvent()
    {

    }
}
