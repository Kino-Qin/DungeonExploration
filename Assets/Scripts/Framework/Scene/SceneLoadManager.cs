using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// 场景切换管理器
/// </summary>
public class SceneLoadManager : Singleton<SceneLoadManager>
{
    /// <summary>
    /// 同步加载场景
    /// </summary>
    /// <param name="sceneName">场景名</param>
    /// <param name="callback">回调函数</param>
    public void LoadScene(string sceneName, UnityAction callback)
    {
        SceneManager.LoadScene(sceneName);
        callback();
    }
    /// <summary>
    /// 异步加载场景，可以获取场景加载进度
    /// </summary>
    /// <param name="sceneName">场景名</param>
    /// <param name="callback">回调函数</param>
    public void LoadSceneAsync(string sceneName, UnityAction callback)
    {
        MonoManager.Instance.StartCoroutine(LoadSceneCoroutine(sceneName, callback));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName, UnityAction callback)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        while (!async.isDone)
        {
            // 通过事件中心分发场景加载进度
            EventCenter.Instance.EventTrigger(EventType.SceneLoading, async.progress);
            // return一个数值时，下一帧继续执行
            yield return async.progress;
        }
        callback?.Invoke();
    }


}
