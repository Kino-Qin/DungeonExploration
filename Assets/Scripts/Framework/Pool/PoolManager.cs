using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 缓存池管理器
/// </summary>
public class PoolManager : Singleton<PoolManager>
{
    private Dictionary<string, Pool> _pools = new Dictionary<string, Pool>();
    private GameObject _poolRoot;
    /// <summary>
    /// 是否在Hierarchy窗口创建缓存池的层级空物体，仅在编辑器模式中开启
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
    /// 从缓存池中获取一个对象，使用前需要初始化对象
    /// </summary>
    /// <param name="poolName">想要获取的对象类型名，一般为Resources路径</param>
    /// <returns></returns>
    public void Pop(string poolName, UnityAction<GameObject> callback)
    {
        // 需要动态创建对象的情况
        if (!_pools.ContainsKey(poolName) ||
            (_pools[poolName].Count == 0 && !_pools[poolName].IsOverMaxCapacity))
        {
            ResourcesManager.Instance.LoadAsync<GameObject>(poolName, (obj) =>
            {
                obj.name = poolName;

                // 没有对应的缓存池，创建缓存池
                if (!_pools.ContainsKey(poolName))
                {
                    PoolObject poolObject = obj.GetComponent<PoolObject>();
                    if (poolObject == null)
                    {
                        Debug.LogError($"请为{obj.name}添加PoolObject脚本");
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
                
                // Pool记录正在使用中的Object
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
    /// 向缓存池中压入一个对象
    /// </summary>
    /// <param name="poolName">回收到哪个缓存池，一般为Resources路径</param>
    /// <param name="obj">回收的对象</param>
    public void Push(string poolName, GameObject obj)
    {
        _pools[poolName].Push(obj);
    }

    /// <summary>
    /// 清空所有缓存池
    /// </summary>
    public void Clear()
    {
        _pools.Clear();
        _poolRoot = null;
    }

}
