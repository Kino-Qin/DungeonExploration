using UnityEngine.Events;

/// <summary>
/// �¼���Ϣ����
/// </summary>
public abstract class EventInfoBase
{

}

/// <summary>
/// �����¼���Ϣ��
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
/// �¼���Ϣ��
/// </summary>
public class EventInfo : EventInfoBase
{
    public UnityAction Actions;
    public EventInfo(UnityAction action)
    {
        Actions += action;
    }
}