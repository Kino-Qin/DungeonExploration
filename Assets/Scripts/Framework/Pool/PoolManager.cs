using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ����ع�����
/// </summary>
public class PoolManager : Singleton<PoolManager>
{
    private Dictionary<string, Pool> _pools = new Dictionary<string, Pool>();
    private GameObject _poolRoot;
    /// <summary>
    /// �Ƿ���Hierarchy���ڴ�������صĲ㼶�����壬���ڱ༭��ģʽ�п���
    /// </summary>
    public readonly bool isOpenHierarchyLayout;

    public PoolManager()
    {
#if UNITY_EDITOR
        isOpenHierarchyLayout = true;
#else
        isOpenHierarchyLayout = false;
#endif
    }

    /// <summary>
    /// �ӻ�����л�ȡһ������ʹ��ǰ��Ҫ��ʼ������
    /// </summary>
    /// <param name="poolName">��Ҫ��ȡ�Ķ�����������һ��ΪResources·��</param>
    /// <returns></returns>
    public void Pop(string poolName, UnityAction<GameObject> callback)
    {
        // ��Ҫ��̬������������
        if (!_pools.ContainsKey(poolName) ||
            (_pools[poolName].Count == 0 && !_pools[poolName].IsOverMaxCapacity))
        {
            ResourcesManager.Instance.LoadAsync<GameObject>(poolName, (obj) =>
            {
                obj.name = poolName;

                // û�ж�Ӧ�Ļ���أ����������
                if (!_pools.ContainsKey(poolName))
                {
                    PoolObject poolObject = obj.GetComponent<PoolObject>();
                    if (poolObject == null)
                    {
                        Debug.LogError($"��Ϊ{obj.name}���PoolObject�ű�");
                        return;
                    }

                    if (isOpenHierarchyLayout)
                    {
                        if (_poolRoot == null)
                        {
                            _poolRoot = new GameObject("Pool");
                        }
                        if (!_pools.ContainsKey(poolName))
                        {
                            _pools.Add(poolName, new Pool(poolName, _poolRoot, poolObject.MaxCapacity));
                        }
                    }
                    else
                    {
                        if (!_pools.ContainsKey(poolName))
                        {
                            _pools.Add(poolName, new Pool(poolObject.MaxCapacity));
                        }
                    }
                }
                
                // Pool��¼����ʹ���е�Object
                _pools[poolName].AddUsingObject(obj);

                callback?.Invoke(obj);
            });
        }
        else
        {
            callback?.Invoke(_pools[poolName].Pop());
        }

    }

    /// <summary>
    /// �򻺴����ѹ��һ������
    /// </summary>
    /// <param name="poolName">���յ��ĸ�����أ�һ��ΪResources·��</param>
    /// <param name="obj">���յĶ���</param>
    public void Push(string poolName, GameObject obj)
    {
        _pools[poolName].Push(obj);
    }

    /// <summary>
    /// ������л����
    /// </summary>
    public void Clear()
    {
        _pools.Clear();
        _poolRoot = null;
    }

}
