using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ����Mono������
/// </summary>
public class MonoManager : SingletonAutoMono<MonoManager>
{
    private event UnityAction _updateEvent;
    private event UnityAction _fixedUpdateEvent;
    private event UnityAction _lateUpdateEvent;

    /// <summary>
    /// ���Update�¼��ļ�������
    /// </summary>
    /// <param name="updateFunc"></param>
    public void AddUpdateListener(UnityAction updateFunc)
    {
        _updateEvent += updateFunc;
    }
    /// <summary>
    /// �Ƴ�Update�¼��ļ�������
    /// </summary>
    /// <param name="updateFunc"></param>
    public void RemoveUpdateListener(UnityAction updateFunc)
    {
        _updateEvent -= updateFunc;
    }

    /// <summary>
    /// ���FixedUpdate�¼��ļ�������
    /// </summary>
    /// <param name="updateFunc"></param>
    public void AddFixedUpdateListener(UnityAction updateFunc)
    {
        _fixedUpdateEvent += updateFunc;
    }
    /// <summary>
    /// �Ƴ�FixedUpdate�¼��ļ�������
    /// </summary>
    /// <param name="updateFunc"></param>
    public void RemoveFixedUpdateListener(UnityAction updateFunc)
    {
        _fixedUpdateEvent -= updateFunc;
    }

    /// <summary>
    /// ���LateUpdate�¼��ļ�������
    /// </summary>
    /// <param name="updateFunc"></param>
    public void AddLateUpdateListener(UnityAction updateFunc)
    {
        _lateUpdateEvent += updateFunc;
    }
    /// <summary>
    /// ���LateUpdate�¼��ļ�������
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
