using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LitJson;

/// <summary>
/// 序列化和反序列化Json时，使用的方案
/// </summary>
public enum JsonDataType
{
    JsonUtility,
    LitJson
}

/// <summary>
/// Json数据管理类，用于管理Json数据的序列化和反序列化
/// 单例模式类对象
/// </summary>
public class JsonDataManager
{
    private static JsonDataManager instance = new JsonDataManager();
    public static JsonDataManager Instance => instance;
    //私有构造函数，避免外部实例化该对象
    private JsonDataManager(){ }

    /// <summary>
    /// 使用Json对对象进行序列化
    /// </summary>
    /// <param name="data">需要存储的对象</param>
    /// <param name="fileName">文件名，不加路径和后缀，文件将存储到PersistentDataPath中</param>
    /// <param name="type">存储类型，缺省值为LitJson</param>
    public void SaveData(object data, string fileName, JsonDataType type = JsonDataType.LitJson)
    {
        //确定存储路径
        string path = Application.persistentDataPath + "/" + fileName + ".json";

        //用于存储Json字符串
        string jsonStr = "";
        switch (type)
        {
            //使用JsonUtility
            case JsonDataType.JsonUtility:
                //将传入的对象转换成Json字符串
                jsonStr = JsonUtility.ToJson(data);
                break;
            //使用LitJson
            case JsonDataType.LitJson:
                //将传入的对象转换成Json字符串
                jsonStr = JsonMapper.ToJson(data);
                break;
        }
        //将Json字符串存储到对应路径中
        File.WriteAllText(path, jsonStr);
    }

    /// <summary>
    /// 读取Json文件并进行反序列化
    /// </summary>
    /// <param name="fileName">文件名，不加路径和后缀</param>
    /// <param name="type">存储类型，缺省值为LitJson</param>
    /// <typeparam name="T">反序列化的对象类型</typeparam>
    /// <returns>反序列化的对象</returns>
    public T LoadData<T>(string fileName, JsonDataType type = JsonDataType.LitJson) where T : new()
    {
        //确定存储路径
        //默认数据文件夹存储路径
        string path = Application.streamingAssetsPath + "/" + fileName + ".json";
        //先判断默认数据文件夹streamingAssetsPath中是否存在该文件
        //若不存在，则去读写文件夹persistentDataPath中寻找
        if (!File.Exists(path))
        {
            //运行时存储路径
            path = Application.persistentDataPath + "/" + fileName + ".json";
            //若读写文件夹persistentDataPath中也没有该文件，返回一个默认对象
            if (!File.Exists(path))
            {
                return new T();
            }
        }
        
        //从对应路径读取Json字符串
        string jsonStr = File.ReadAllText(path);
        //数据对象默认值
        T ret = default(T);
        switch (type)
        {
            case JsonDataType.JsonUtility:
                ret = JsonUtility.FromJson<T>(jsonStr);
                break;
            case JsonDataType.LitJson:
                ret = JsonMapper.ToObject<T>(jsonStr);
                break;
        }
        //返回反序列化后的对象
        return ret;
    }
}
