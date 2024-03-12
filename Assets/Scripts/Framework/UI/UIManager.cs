using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// 面板所属的层级
/// </summary>
public enum PanelLayer
{
    /// <summary>
    /// 最高层级，用于系统通知的面板
    /// </summary>
    System,
    /// <summary>
    /// 高层级面板
    /// </summary>
    Top,
    /// <summary>
    /// 中等层级面板
    /// </summary>
    Mid,
    /// <summary>
    /// 底层级面板
    /// </summary>
    Bot
}

public class UIManager : Singleton<UIManager>
{
    private Dictionary<string, BasePanel> _panels = new Dictionary<string, BasePanel>();
    private Transform _system;
    private Transform _top;
    private Transform _mid;
    private Transform _bot;
    private const string UI_RESOURCE_PATH = "UI/";
    /// <summary>
    /// 场景的Canvas
    /// </summary>
    public RectTransform Canvas;

    public UIManager()
    {
        Canvas = ResourcesManager.Instance.Load<GameObject>($"{UI_RESOURCE_PATH}Canvas").transform as RectTransform;
        _system = Canvas.transform.Find("System");
        _top = Canvas.transform.Find("Top");
        _mid = Canvas.transform.Find("Mid");
        _bot = Canvas.transform.Find("Bot");
        GameObject.DontDestroyOnLoad(Canvas);
        ResourcesManager.Instance.Load<GameObject>($"{UI_RESOURCE_PATH}EventSystem");
    }

    /// <summary>
    /// 获取对应面板层级的对应根对象
    /// </summary>
    /// <param name="panelLayer">面板层级</param>
    /// <returns></returns>
    public Transform GetPanelLayerRoot(PanelLayer panelLayer)
    {
        switch (panelLayer)
        {
            case PanelLayer.System:
                return _system;
            case PanelLayer.Top:
                return _top;
            case PanelLayer.Mid:
                return _mid;
            case PanelLayer.Bot:
                return _bot;
            default:
                return null;
        }
    }

    /// <summary>
    /// 显示面板
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="panelName">面板名</param>
    /// <param name="panelLayer">面板所属层级</param>
    /// <param name="callback">创建面板完成后的回调函数，参数为面板脚本</param>
    public void ShowPanel<T>(string panelName, PanelLayer panelLayer = PanelLayer.Mid, UnityAction<T> callback = null) where T : BasePanel
    {
        if (_panels.ContainsKey(panelName))
        {
            _panels[panelName].Show();
            callback?.Invoke(_panels[panelName] as T);
            return;
        }

        ResourcesManager.Instance.LoadAsync<GameObject>($"{UI_RESOURCE_PATH}{panelName}", (obj) =>
        {
            obj.transform.SetParent(GetPanelLayerRoot(panelLayer));

            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            (obj.transform as RectTransform).offsetMax = Vector2.zero;
            (obj.transform as RectTransform).offsetMin = Vector2.zero;

            T panel = obj.GetComponent<T>();
            _panels.Add(panelName, panel);
            panel.Show();
            callback?.Invoke(panel);
        });
    }
    /// <summary>
    /// 隐藏面板
    /// </summary>
    /// <param name="panelName">面板名</param>
    public void HidePanel(string panelName)
    {
        if (_panels.ContainsKey(panelName))
        {
            _panels[panelName].Hide();
            GameObject.Destroy(_panels[panelName].gameObject);
            _panels.Remove(panelName);
        }
    }
    /// <summary>
    /// 获取面板
    /// </summary>
    /// <typeparam name="T">面板名</typeparam>
    /// <param name="panelName"></param>
    /// <returns></returns>
    public T GetPanel<T>(string panelName) where T : BasePanel
    {
        if (_panels.ContainsKey(panelName))
        {
            return _panels[panelName] as T;
        }
        return null;
    }

    /// <summary>
    /// 为UI控件增加自定义事件监听，如鼠标拖拽等
    /// </summary>
    /// <param name="control">UI控件</param>
    /// <param name="type">监听的事件类型</param>
    /// <param name="callback">回调函数</param>
    public static void AddCustomEventListener(UIBehaviour control, EventTriggerType type, UnityAction<BaseEventData> callback)
    {
        EventTrigger trigger = control.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = control.gameObject.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener(callback);

        trigger.triggers.Add(entry);
    }
}
