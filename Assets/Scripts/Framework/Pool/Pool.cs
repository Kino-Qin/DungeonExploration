using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����
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
    /// �ӻ�����л�ȡһ������
    /// </summary>
    /// <returns></returns>
    public GameObject Pop()
    {
        GameObject obj;
        if (IsOverMaxCapacity)
        {
            // ����������������ޣ���ȡ������ʹ�����б���ʹ����õĶ��󣬼�_usingObjects[0]ʹ��
            obj = _usingObjects[0];

            // �����������ʹ�����б����Ƴ�
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
    /// �򻺴����ѹ��һ������
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
    /// ��һ�������¼Ϊ����ʹ����
    /// </summary>
    /// <param name="obj"></param>
    public void AddUsingObject(GameObject obj)
    {
        _usingObjects.Add(obj);
    }

    /// <summary>
    /// ��һ�����������ʹ�����Ƴ�
    /// </summary>
    /// <param name="obj"></param>
    public void RemoveUsingObject(GameObject obj)
    {
        _usingObjects.Remove(obj);
    }
}
