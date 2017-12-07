using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ItemInfoPanel : BasePanel {

    public override void OnEnter()
    {
        gameObject.active = true;
        CanvasGroup.alpha = 0;
        CanvasGroup.DOFade(1, 0.5f);
    }

    public override void OnExit()
    {
        CanvasGroup.DOFade(0, 0.5f)
            .OnComplete(()=> { gameObject.active = false; });
    }
}
