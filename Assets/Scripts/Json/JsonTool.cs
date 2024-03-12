using LitJson;
using System.IO;
using UnityEngine;

/// <summary>
/// Json序列化与反序列工具类
/// </summary>
public static class JsonTool
{
    /// <summary>
    /// 将数据对象序列化为Json文件
    /// </summary>
    /// <param name="data">数据对象</param>
    /// <param name="fileName">文件名</param>
    /// <param name="type">Json序列化方式</param>
    public static void SaveData(object data, string fileName, JsonSerializeType type = JsonSerializeType.LitJson)
    {
        string jsonString;
        string path = string.Format($"{Application.persistentDataPath}/{fileName}.json");
        switch (type)
        {
            case JsonSerializeType.JsonUtility:
                jsonString = JsonUtility.ToJson(data);
                break;
            case JsonSerializeType.LitJson:
                jsonString = JsonMapper.ToJson(data);
                break;
            default:
                jsonString = string.Empty;
                break;
        }
        File.WriteAllText(path, jsonString);
    }

    /// <summary>
    /// 将Json文件反序列化为对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="fileName"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static T LoadData<T>(string fileName, JsonSerializeType type = JsonSerializeType.LitJson) where T : new()
    {
        string path = string.Format($"{Application.persistentDataPath}/{fileName}.json");
        if (!File.Exists(path))
        {
            path = string.Format($"{Application.streamingAssetsPath}/{fileName}.json");
            if (!File.Exists(path))
            {
                return new T();
            }
        }
        string jsonString = File.ReadAllText(path);
        T data = default;
        switch (type)
        {
            case JsonSerializeType.JsonUtility:
                data = JsonUtility.FromJson<T>(jsonString);
                break;
            case JsonSerializeType.LitJson:
                data = JsonMapper.ToObject<T>(jsonString);
                break;
        }
        return data;
    }

}
