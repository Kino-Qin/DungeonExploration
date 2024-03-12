/// <summary>
/// 事件中心监听的所有事件类型
/// </summary>
public enum EventType
{
    /// <summary>
    /// 怪物死亡 参数：Monster 死亡的怪物
    /// </summary>
    MonsterDead,
    /// <summary>
    /// 场景异步加载 参数：float 场景加载进度
    /// </summary>
    SceneLoading
}