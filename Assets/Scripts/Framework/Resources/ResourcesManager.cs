using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ��Դ���ع�����
/// </summary>
public class ResourcesManager : Singleton<ResourcesManager>
{
    /// <summary>
    /// ͬ��������Դ
    /// </summary>
    /// <typeparam name="T">��Դ����</typeparam>
    /// <param name="path">��Դ·��</param>
    /// <returns></returns>
    public T Load<T>(string path) where T : Object
    {
        T res = Resources.Load<T>(path);
        if (res is GameObject)
        {
            return GameObject.Instantiate(res);
        }
        return res;
    }

    /// <summary>
    /// �첽������Դ
    /// </summary>
    /// <typeparam name="T">��Դ����</typeparam>
    /// <param name="path">��Դ·��</param>
    /// <param name="callback">�ص�����������Ϊ��Դ����</param>
    public void LoadAsync<T>(string path, UnityAction<T> callback) where T : Object
    {
        MonoManager.Instance.StartCoroutine(LoadAsyncCoroutine<T>(path, callback));
    }
    private IEnumerator LoadAsyncCoroutine<T>(string path, UnityAction<T> callback) where T : Object
    {
        ResourceRequest rq = Resources.LoadAsync<T>(path);
        yield return rq;
        if (rq.asset is GameObject)
        {
            callback?.Invoke(GameObject.Instantiate(rq.asset) as T);
            yield break;
        }
        callback?.Invoke(rq.asset as T);
    }
}
