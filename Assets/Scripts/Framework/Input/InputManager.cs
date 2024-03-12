using UnityEngine;

/// <summary>
/// 输入管理类
/// </summary>
public class InputManager : Singleton<InputManager>
{
    /// <summary>
    /// 是否进行输入检测
    /// </summary>
    public bool CheckInput { get; set; }

    public InputManager()
    {
        MonoManager.Instance.AddUpdateListener(Update);
    }

    private void CheckKeyInput(KeyCode key)
    {
        if (Input.GetKeyDown(key))
        {
            //EventCenter.Instance.EventTrigger("KeyDown", key);
        }
        if (Input.GetKeyUp(key))
        {
            //EventCenter.Instance.EventTrigger("KeyUp", key);
        }
    }
    private void Update()
    {
        if (!CheckInput)
        {
            return;
        }
        CheckKeyInput(KeyCode.W);
        CheckKeyInput(KeyCode.S);
        CheckKeyInput(KeyCode.A);
        CheckKeyInput(KeyCode.D);
    }
}
