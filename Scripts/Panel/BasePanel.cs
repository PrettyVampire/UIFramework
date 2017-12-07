using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BasePanel : MonoBehaviour {

    private CanvasGroup m_canvasGroup = null;
    public CanvasGroup CanvasGroup
    {
        get
        {
            if(m_canvasGroup == null)
            {
                m_canvasGroup = GetComponent<CanvasGroup>();
            }
            return m_canvasGroup;
        }
    }

    public virtual void OnEnter()
    {
    }

    public virtual void OnPause()
    {
        CanvasGroup.blocksRaycasts = false;
    }

    public virtual void OnResume()
    {
        CanvasGroup.blocksRaycasts = true;
    }

    public virtual void OnExit()
    {
    }

    public virtual void OnClose()
    {
        UIManager.Instance.PopPanel();
    }
}
