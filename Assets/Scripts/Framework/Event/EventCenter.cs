using System.Collections.Generic;
using UnityEngine.Events;

/// <summary>
/// �¼�����
/// </summary>
public class EventCenter : Singleton<EventCenter>
{
    private Dictionary<EventType, EventInfoBase> _events = new Dictionary<EventType, EventInfoBase>();
    
    /// <summary>
    /// ����¼�����
    /// </summary>
    /// <param name="eventType">�������¼���</param>
    /// <param name="func"></param>
    public void AddEventListener<T>(EventType eventType, UnityAction<T> func)
    {
        if (_events.ContainsKey(eventType))
        {
            (_events[eventType] as EventInfo<T>).Actions += func;
        }
        else
        {
            _events.Add(eventType, new EventInfo<T>(func));
        }
    }
    /// <summary>
    /// ����¼��������޲����ص�
    /// </summary>
    /// <param name="eventType">�������¼���</param>
    /// <param name="func"></param>
    public void AddEventListener(EventType eventType, UnityAction func)
    {
        if (_events.ContainsKey(eventType))
        {
            (_events[eventType] as EventInfo).Actions += func;
        }
        else
        {
            _events.Add(eventType, new EventInfo(func));
        }
    }

    /// <summary>
    /// �Ƴ��¼�����
    /// </summary>
    /// <param name="eventType">�Ƴ��������¼���</param>
    /// <param name="func">�Ƴ��Ļص�����</param>
    public void RemoveEventListener<T>(EventType eventType, UnityAction<T> func)
    {
        if (_events.ContainsKey(eventType))
        {
            (_events[eventType] as EventInfo<T>).Actions -= func;
        }
    }
    /// <summary>
    /// �Ƴ��¼��������޲����ص�
    /// </summary>
    /// <param name="eventType">�Ƴ��������¼���</param>
    /// <param name="func">�Ƴ��Ļص�����</param>
    public void RemoveEventListener(EventType eventType, UnityAction func)
    {
        if (_events.ContainsKey(eventType))
        {
            (_events[eventType] as EventInfo).Actions -= func;
        }
    }

    /// <summary>
    /// �¼�����
    /// </summary>
    /// <param name="eventType">�������¼���</param>
    /// <param name="info">�¼���Ϣ</param>
    public void EventTrigger<T>(EventType eventType, T info)
    {
        if (_events.ContainsKey(eventType))
        {
            (_events[eventType] as EventInfo<T>).Actions?.Invoke(info);
        }
    }
    /// <summary>
    /// �¼��������޲����ص�
    /// </summary>
    /// <param name="eventType">�������¼���</param>
    /// <param name="info">�¼���Ϣ</param>
    public void EventTrigger(EventType eventType)
    {
        if (_events.ContainsKey(eventType))
        {
            (_events[eventType] as EventInfo).Actions?.Invoke();
        }
    }

    /// <summary>
    /// ��������¼��ļ�������
    /// </summary>
    public void Clear()
    {
        _events.Clear();
    }
    /// <summary>
    /// ���ָ���¼������м�������
    /// </summary>
    /// <param name="eventType"></param>
    public void Clear(EventType eventType)
    {
        if (_events.ContainsKey(eventType))
        {
            _events.Remove(eventType);
        }
    }
}
