using UnityEngine.Events;

/// <summary>
/// 事件信息基类
/// </summary>
public abstract class EventInfoBase
{

}

/// <summary>
/// 泛型事件信息类
/// </summary>
/// <typeparam name="T"></typeparam>
public class EventInfo<T> : EventInfoBase
{
    public UnityAction<T> Actions;
    public EventInfo(UnityAction<T> action)
    {
       Actions += action;
    }
}

/// <summary>
/// 事件信息类
/// </summary>
public class EventInfo : EventInfoBase
{
    public UnityAction Actions;
    public EventInfo(UnityAction action)
    {
        Actions += action;
    }
}