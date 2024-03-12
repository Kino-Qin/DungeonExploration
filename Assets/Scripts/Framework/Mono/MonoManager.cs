using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 公共Mono管理类
/// </summary>
public class MonoManager : SingletonAutoMono<MonoManager>
{
    private event UnityAction _updateEvent;
    private event UnityAction _fixedUpdateEvent;
    private event UnityAction _lateUpdateEvent;

    /// <summary>
    /// 添加Update事件的监听函数
    /// </summary>
    /// <param name="updateFunc"></param>
    public void AddUpdateListener(UnityAction updateFunc)
    {
        _updateEvent += updateFunc;
    }
    /// <summary>
    /// 移除Update事件的监听函数
    /// </summary>
    /// <param name="updateFunc"></param>
    public void RemoveUpdateListener(UnityAction updateFunc)
    {
        _updateEvent -= updateFunc;
    }

    /// <summary>
    /// 添加FixedUpdate事件的监听函数
    /// </summary>
    /// <param name="updateFunc"></param>
    public void AddFixedUpdateListener(UnityAction updateFunc)
    {
        _fixedUpdateEvent += updateFunc;
    }
    /// <summary>
    /// 移除FixedUpdate事件的监听函数
    /// </summary>
    /// <param name="updateFunc"></param>
    public void RemoveFixedUpdateListener(UnityAction updateFunc)
    {
        _fixedUpdateEvent -= updateFunc;
    }

    /// <summary>
    /// 添加LateUpdate事件的监听函数
    /// </summary>
    /// <param name="updateFunc"></param>
    public void AddLateUpdateListener(UnityAction updateFunc)
    {
        _lateUpdateEvent += updateFunc;
    }
    /// <summary>
    /// 添加LateUpdate事件的监听函数
    /// </summary>
    /// <param name="updateFunc"></param>
    public void RemoveLateUpdateListener(UnityAction updateFunc)
    {
        _lateUpdateEvent -= updateFunc;
    }

    private void Update()
    {
        _updateEvent?.Invoke();
    }
    private void FixedUpdate()
    {
        _fixedUpdateEvent?.Invoke();
    }
    private void LateUpdate()
    {
        _lateUpdateEvent?.Invoke();
    }
}
