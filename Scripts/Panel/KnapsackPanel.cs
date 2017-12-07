using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class KnapsackPanel : BasePanel
{
    private float m_originalPosX = 600;
    public override void OnEnter()
    {
        gameObject.active = true;
        transform.localPosition = new Vector3(m_originalPosX, transform.localPosition.y, transform.localPosition.z);
        transform.DOLocalMoveX(0, 0.5f)
            .SetEase(Ease.Linear);

    }

    public override void OnExit()
    {
        
        transform.DOLocalMoveX(m_originalPosX, 0.5f)
            .SetEase(Ease.OutBack)
            .OnComplete(()=> {
                gameObject.active = false;
            });
    }

    public void OnItemButtonClick()
    {
        UIManager.Instance.PushPanel(UIPanelType.ItemInfo);
    }
}
