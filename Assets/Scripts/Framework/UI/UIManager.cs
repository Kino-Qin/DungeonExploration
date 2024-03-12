using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// ��������Ĳ㼶
/// </summary>
public enum PanelLayer
{
    /// <summary>
    /// ��߲㼶������ϵͳ֪ͨ�����
    /// </summary>
    System,
    /// <summary>
    /// �߲㼶���
    /// </summary>
    Top,
    /// <summary>
    /// �еȲ㼶���
    /// </summary>
    Mid,
    /// <summary>
    /// �ײ㼶���
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
    /// ������Canvas
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
    /// ��ȡ��Ӧ���㼶�Ķ�Ӧ������
    /// </summary>
    /// <param name="panelLayer">���㼶</param>
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
    /// ��ʾ���
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="panelName">�����</param>
    /// <param name="panelLayer">��������㼶</param>
    /// <param name="callback">���������ɺ�Ļص�����������Ϊ���ű�</param>
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
    /// �������
    /// </summary>
    /// <param name="panelName">�����</param>
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
    /// ��ȡ���
    /// </summary>
    /// <typeparam name="T">�����</typeparam>
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
    /// ΪUI�ؼ������Զ����¼��������������ק��
    /// </summary>
    /// <param name="control">UI�ؼ�</param>
    /// <param name="type">�������¼�����</param>
    /// <param name="callback">�ص�����</param>
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
