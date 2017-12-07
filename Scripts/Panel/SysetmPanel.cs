using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SysetmPanel : BasePanel
{

    public override void OnEnter()
    {
        gameObject.active = true;
        transform.localScale = Vector3.zero;
        transform.DOScale(1, 0.5f);
    }

    public override void OnExit()
    {
        transform.DOScale(0, 0.5f)
            .OnComplete(() => { gameObject.active = false; });
    }
}
