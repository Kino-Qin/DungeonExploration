using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 资源加载管理器
/// </summary>
public class ResourcesManager : Singleton<ResourcesManager>
{
    /// <summary>
    /// 同步加载资源
    /// </summary>
    /// <typeparam name="T">资源类型</typeparam>
    /// <param name="path">资源路径</param>
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
    /// 异步加载资源
    /// </summary>
    /// <typeparam name="T">资源类型</typeparam>
    /// <param name="path">资源路径</param>
    /// <param name="callback">回调函数，参数为资源对象</param>
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
