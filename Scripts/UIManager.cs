using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

/// <summary>
/// ui框架的管理类
/// 1.解析保存所有面板信息（m_panelPathDict）
/// 2.创建保存所有面板的事例（m_panelDict）
/// 3.管理保存所有显示的面板
/// </summary>
public class UIManager {
    /// <summary>
    /// 1.定义一个静态对象，外界可以通过类名直接访问
    /// 2.构造方法私有化
    /// </summary>
    #region 单例模式
    private static UIManager m_instance = null;
    public static UIManager Instance
    {
        get
        {
            if(m_instance == null)
            {
                m_instance = new UIManager();
            }
            return m_instance;
        }
    }
    #endregion

    private Dictionary<UIPanelType, string> m_panelPathDict;//存储所有面板prefab的路径
    private Dictionary<UIPanelType, BasePanel> m_panelDict; //存储所有面板
    private Stack<BasePanel> m_panelStack;//管理所有显示的面板
    private Transform m_canvasTransform;
    private Transform CanvasTransform
    {
        get
        {
            if(m_canvasTransform == null)
            {
                m_canvasTransform = GameObject.Find("Canvas").GetComponent<Transform>();
            }
            return m_canvasTransform;
        }
    }

    UIManager()
    {
        ParseUIPanelTypeJson();
    }

    /// <summary>
    /// 解析存储面板信息的json数据
    /// </summary>
    void ParseUIPanelTypeJson()
    {
        m_panelPathDict = new Dictionary<UIPanelType, string>();
        //加载文本  TextAsset  
        TextAsset panelInfoJson = Resources.Load<TextAsset>("UIPanelInfo");
        
        JsonData jsonObject = JsonMapper.ToObject(panelInfoJson.text);

        for (int i = 0; i < jsonObject.Count; i++)
        {
            string typeStr = (string)jsonObject[i]["PanelType"];
            UIPanelType type = (UIPanelType)System.Enum.Parse(typeof(UIPanelType), typeStr);//枚举类型的强转
            string path = (string)jsonObject[i]["Path"];

            m_panelPathDict.Add(type, path);
        }
    }


    /// <summary>
    /// 获取面板
    /// 1.如果存储panel的字典为空，则创建一个
    /// 2.从字典获取对应panel类型的panel
    /// 3.如果没获取到此类型的panel，则实例化一个，反之直接返回
    /// </summary>
    /// <param name="panelType"></param>
    public BasePanel GetPanel(UIPanelType panelType)
    {
        if(m_panelDict == null)
        {
            m_panelDict = new Dictionary<UIPanelType, BasePanel>();
        }

        BasePanel panel = m_panelDict.TryGet(panelType);
        
        if(panel == null)
        {
            string path = m_panelPathDict.TryGet(panelType);
            GameObject panelObj = GameObject.Instantiate(Resources.Load(path)) as GameObject;
            panelObj.transform.SetParent(CanvasTransform, false);//不保留世界坐标的位置
            panel = panelObj.GetComponent<BasePanel>();
            m_panelDict.Add(panelType, panel);
        }

        return panel;
    }
   

    /// <summary>
    /// 页面入栈，显示在界面上
    /// </summary>
    public void PushPanel(UIPanelType panelType)
    {
        if(m_panelStack == null)
        {
            m_panelStack = new Stack<BasePanel>();
        }

        //栈里有页面 暂停已有
        if(m_panelStack.Count > 0)
        {
            BasePanel topPanel = m_panelStack.Peek();
            topPanel.OnPause();
        }

        //显示新页面
        BasePanel panel = GetPanel(panelType);
        panel.OnEnter();
        m_panelStack.Push(panel);
    }


    /// <summary>
    /// 移除页面
    /// </summary>
    public void PopPanel()
    {
        if (m_panelStack == null)
        {
            m_panelStack = new Stack<BasePanel>();
        }
        //弹出栈顶元素
        if(m_panelStack.Count > 0)
        {
            BasePanel topPanel = m_panelStack.Pop();
            topPanel.OnExit();
        }
        //顶部的第二个元素resume
        if(m_panelStack.Count > 0)
        {
            BasePanel topPanel = m_panelStack.Peek();
            topPanel.OnResume();
        }
        
    }

    public void Test()
    {
        string path = m_panelPathDict.TryGet(UIPanelType.Knapsack);

        
        Debug.Log(path);
    }
}
