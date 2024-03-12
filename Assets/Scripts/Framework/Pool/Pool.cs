using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 缓存池
/// </summary>
public class Pool
{
    private GameObject _root;
    private Queue<GameObject> _objects = new Queue<GameObject>();
    private List<GameObject> _usingObjects = new List<GameObject>();
    private readonly int _maxCapacity;
    public int Count => _objects.Count;
    public bool IsOverMaxCapacity => _usingObjects.Count >= _maxCapacity;

    public Pool(int maxCapacity)
    {
        _maxCapacity = maxCapacity;
    }

    public Pool(string poolName, GameObject poolRoot, int maxCapacity) : this(maxCapacity)
    {
        _root = new GameObject(poolName);
        _root.transform.parent = poolRoot.transform;
    }

    /// <summary>
    /// 从缓存池中获取一个对象
    /// </summary>
    /// <returns></returns>
    public GameObject Pop()
    {
        GameObject obj;
        if (IsOverMaxCapacity)
        {
            // 超出缓存池容量上限，则取出正在使用中列表中使用最久的对象，即_usingObjects[0]使用
            obj = _usingObjects[0];

            // 将对象从正在使用中列表中移除
            _usingObjects.RemoveAt(0);
        }
        else
        {
            obj = _objects.Dequeue();
            obj.SetActive(true);
        }
        AddUsingObject(obj);

        if (PoolManager.Instance.isOpenHierarchyLayout)
        {
            obj.transform.parent = null;
        }
        return obj;
    }

    /// <summary>
    /// 向缓存池中压入一个对象
    /// </summary>
    /// <param name="obj"></param>
    public void Push(GameObject obj)
    {
        obj.SetActive(false);
        _objects.Enqueue(obj);
        RemoveUsingObject(obj);

        if (PoolManager.Instance.isOpenHierarchyLayout)
        {
            obj.transform.parent = _root.transform;
        }
    }

    /// <summary>
    /// 将一个对象记录为正在使用中
    /// </summary>
    /// <param name="obj"></param>
    public void AddUsingObject(GameObject obj)
    {
        _usingObjects.Add(obj);
    }

    /// <summary>
    /// 将一个对象从正在使用中移除
    /// </summary>
    /// <param name="obj"></param>
    public void RemoveUsingObject(GameObject obj)
    {
        _usingObjects.Remove(obj);
    }
}
