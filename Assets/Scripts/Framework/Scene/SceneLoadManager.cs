using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// �����л�������
/// </summary>
public class SceneLoadManager : Singleton<SceneLoadManager>
{
    /// <summary>
    /// ͬ�����س���
    /// </summary>
    /// <param name="sceneName">������</param>
    /// <param name="callback">�ص�����</param>
    public void LoadScene(string sceneName, UnityAction callback)
    {
        SceneManager.LoadScene(sceneName);
        callback();
    }
    /// <summary>
    /// �첽���س��������Ի�ȡ�������ؽ���
    /// </summary>
    /// <param name="sceneName">������</param>
    /// <param name="callback">�ص�����</param>
    public void LoadSceneAsync(string sceneName, UnityAction callback)
    {
        MonoManager.Instance.StartCoroutine(LoadSceneCoroutine(sceneName, callback));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName, UnityAction callback)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        while (!async.isDone)
        {
            // ͨ���¼����ķַ��������ؽ���
            EventCenter.Instance.EventTrigger(EventType.SceneLoading, async.progress);
            // returnһ����ֵʱ����һ֡����ִ��
            yield return async.progress;
        }
        callback?.Invoke();
    }


}
