/*
 * 脚本名(ScriptName)：    ResourcesMgr.cs
 * 作者(Author):           Circle
 * 官网(Url):              http://blog.csdn.net/u013108312
 */
using UnityEngine;
using System.Collections;

public class ResourcesMgr : MonoBehaviour
{
    #region 初始化
    private static ResourcesMgr mInstance;
    public static bool hasIntance
    {
        get
        {
            return mInstance != null;
        }
    }

    /// <summary>
    /// 是否正在删除，当程序退出时设置为true
    /// </summary>
    public static bool isDestorying = false;
    /// <summary>
    /// 获取单例
    /// </summary>
    /// <returns></returns>
    public static ResourcesMgr GetInstance()
    {
        if (!hasIntance)
        {
            if (isDestorying)
            {
                return null;
            }
            mInstance = new GameObject("_ResourcesMgr").AddComponent<ResourcesMgr>();
        }
        return mInstance;
    }
    void OnApplicationQuit()
    {
        isDestorying = true;
    }

    private ResourcesMgr()
    {
        hashtable = new Hashtable();
    }
    #endregion
    /// <summary> 资源缓存集合 </summary>
    private Hashtable hashtable;
    /// <summary>
    /// 从Res中加载资源
    /// </summary>
    /// <typeparam name="T">资源裂隙</typeparam>
    /// <param name="path">资源路径</param>
    /// <param name="cache">是否缓存</param>
    /// <returns></returns>
    public T Load<T>(string path,bool cache) where T : UnityEngine.Object
    {
        if(hashtable.Contains(path))
        {
            return hashtable[path] as T;
        }

        T assetObj = Resources.Load<T>(path);
        if(assetObj == null)
        {
            Debug.LogError("资源不存在 path=" + path);
        }
        if(cache)
        {
            hashtable.Add(path, assetObj);
            Debug.Log("对象缓存 path=" + path);
        }
        return assetObj;
    }
    /// <summary>
    /// 从Res中创建一个GameObject对象
    /// </summary>
    /// <param name="path">路径</param>
    /// <param name="cache">是否缓存</param>
    /// <returns></returns>
    public GameObject CreateGameObject(string path,bool cache)
    {
        GameObject assetObj = Load<GameObject>(path, cache);
        GameObject go = Instantiate(assetObj) as GameObject;
        if(go == null)
        {
            Debug.LogError("从Res中创建游戏对象失败，path=" + path);
        }
        return go;
    }
}
