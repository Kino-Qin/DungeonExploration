using System.Collections.Generic;
using UnityEngine.Events;

/// <summary>
/// 事件中心
/// </summary>
public class EventCenter : Singleton<EventCenter>
{
    private Dictionary<EventType, EventInfoBase> _events = new Dictionary<EventType, EventInfoBase>();
    
    /// <summary>
    /// 添加事件监听
    /// </summary>
    /// <param name="eventType">监听的事件名</param>
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
    /// 添加事件监听，无参数回调
    /// </summary>
    /// <param name="eventType">监听的事件名</param>
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
    /// 移除事件监听
    /// </summary>
    /// <param name="eventType">移除监听的事件名</param>
    /// <param name="func">移除的回调函数</param>
    public void RemoveEventListener<T>(EventType eventType, UnityAction<T> func)
    {
        if (_events.ContainsKey(eventType))
        {
            (_events[eventType] as EventInfo<T>).Actions -= func;
        }
    }
    /// <summary>
    /// 移除事件监听，无参数回调
    /// </summary>
    /// <param name="eventType">移除监听的事件名</param>
    /// <param name="func">移除的回调函数</param>
    public void RemoveEventListener(EventType eventType, UnityAction func)
    {
        if (_events.ContainsKey(eventType))
        {
            (_events[eventType] as EventInfo).Actions -= func;
        }
    }

    /// <summary>
    /// 事件触发
    /// </summary>
    /// <param name="eventType">触发的事件名</param>
    /// <param name="info">事件信息</param>
    public void EventTrigger<T>(EventType eventType, T info)
    {
        if (_events.ContainsKey(eventType))
        {
            (_events[eventType] as EventInfo<T>).Actions?.Invoke(info);
        }
    }
    /// <summary>
    /// 事件触发，无参数回调
    /// </summary>
    /// <param name="eventType">触发的事件名</param>
    /// <param name="info">事件信息</param>
    public void EventTrigger(EventType eventType)
    {
        if (_events.ContainsKey(eventType))
        {
            (_events[eventType] as EventInfo).Actions?.Invoke();
        }
    }

    /// <summary>
    /// 清空所有事件的监听函数
    /// </summary>
    public void Clear()
    {
        _events.Clear();
    }
    /// <summary>
    /// 清除指定事件的所有监听函数
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
