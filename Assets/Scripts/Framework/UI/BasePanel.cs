using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// UI面板基类
/// </summary>
public class BasePanel : MonoBehaviour
{
    private Dictionary<string, List<UIBehaviour>> _uiControls = new Dictionary<string, List<UIBehaviour>>();

    private void Awake()
    {
        FindChildrenUIControls<Image>();
        FindChildrenUIControls<Text>();
        FindChildrenUIControls<Button>();   
        FindChildrenUIControls<Slider>();   
        FindChildrenUIControls<Toggle>();
        FindChildrenUIControls<ScrollRect>();
    }

    /// <summary>
    /// 显示面板时由UIManager调用
    /// </summary>
    public virtual void Show()
    {

    }
    /// <summary>
    /// 隐藏面板时由UIManager调用
    /// </summary>
    public virtual void Hide()
    {

    }

    private void FindChildrenUIControls<T>() where T : UIBehaviour
    {
        T[] controls = GetComponentsInChildren<T>();
        foreach (T item in controls)
        {
            string name = item.gameObject.name;
            if (_uiControls.ContainsKey(name))
            {
                _uiControls[name].Add(item);
            }
            else
            {
                _uiControls.Add(name, new List<UIBehaviour>() { item });
            }
        }
    }

    /// <summary>
    /// 获取面板子对象中的UI控件脚本
    /// </summary>
    /// <typeparam name="T">UI控件类型</typeparam>
    /// <param name="name">UI控件所在对象名</param>
    /// <returns></returns>
    protected T GetControl<T>(string name) where T : UIBehaviour
    {
        if (_uiControls.ContainsKey(name))
        {
            foreach (UIBehaviour item in _uiControls[name])
            {
                if (item is T)
                {
                    return item as T;
                }
            }
        }

        return null;
    }
}
