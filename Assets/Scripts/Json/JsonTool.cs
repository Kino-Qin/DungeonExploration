using LitJson;
using System.IO;
using UnityEngine;

/// <summary>
/// Json���л��뷴���й�����
/// </summary>
public static class JsonTool
{
    /// <summary>
    /// �����ݶ������л�ΪJson�ļ�
    /// </summary>
    /// <param name="data">���ݶ���</param>
    /// <param name="fileName">�ļ���</param>
    /// <param name="type">Json���л���ʽ</param>
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
    /// ��Json�ļ������л�Ϊ����
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
