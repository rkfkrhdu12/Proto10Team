using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUIAni : UIAnimationController
{
    public override void OnEvent()
    {
        base.OnEvent();

        gameObject.SetActive(false);
    }
}
