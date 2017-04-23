using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 功能：
/// 1.根据资源地址，创建实例，主资源和child资源.如果主资源不存在则生成一个空的gameobject,child资源会放主资源实例下，child资源可以有0到多个。
/// 注：
/// 1）OnInitSkinFront中需要设置主资源（mainSkinPath）和child资源（AddChildSkinPath）
/// 2）资源默认是resource下的资源。如是assetBundle.需在OnInitSkinFront中修改skinSrcType
/// 
/// 2.主资源及主资源下所有的boxCollider,如果boxCollider.gameobject.name已"Btn"开头。则会触发OnClick(target)
/// 3.在2的基础上，实现点击时间间隔约束功能，默认打开，如需开启设置clickIntervalRestrict
/// 4.SetColliderEnabled 设置该对象下所有boxcollider是否可以交互
/// 5.OnSecondUpdate，每秒刷新。默认为不开启，如需开启，设置secondUpdateEnabled = true
/// 
/// 6.生命周期OnInitFront->OnInitSkinFront->OnInitSkin->OnInitSkinDone->OnInitDone->OnSecondUpdate-OnUpdate-OnFixedUpdate-OnLateUpdate->（OnDestroyFront->OnOnDestroy->OnDestroyDone）
/// 注：如果没有调用Init,这些逻辑不会执行.除（）内删除相关
/// </summary>
public class UIBase : MonoBehaviour
{
    /// <summary>
    /// 是否起用秒刷新
    /// </summary>
    protected bool secondUpdateEnabled = false;
    /// <summary>
    /// 秒刷新时间点
    /// </summary>
    private float secondUpdateTime = 0.0f;

    private GameObject _skin;
    /// <summary>
    /// 皮肤
    /// </summary>
    public GameObject skin
    {
        get
        {
            return _skin;
        }
    }
    /// <summary>
    /// 主皮肤
    /// </summary>
    private string mainSkinPath;
    /// <summary>
    /// skin 's transfrom
    /// </summary>
    public Transform skinTransform
    {
        get
        {
            if (skin != null)
            {
                return skin.transform;
            }
            return null;
        }
    }


    /// <summary>
    /// 所有boxCollider
    /// </summary>
    private List<Collider> colliderList = new List<Collider>();

   
    private bool _initDoneFlag = false;
    /// <summary>
    /// 初始化完成标识
    /// </summary>
    public bool initDoneFlag
    {
        get
        {
            return _initDoneFlag;
        }
    }

    public void OnDestroy()
    {
        OnDestroyFront();

        OnOnDestroy();
        colliderList.Clear();
        colliderList = null;
        StopAllCoroutines();
       
        OnDestroyDone();
    }

    void Update()
    {
        if (! initDoneFlag)
        {
            return;
        }

        OnUpdate();

        if(secondUpdateEnabled)
        {
            float delta = Time.deltaTime;
            secondUpdateTime += delta;
            if (secondUpdateTime > 1.0f)
            {
                secondUpdateTime -= 1.0f;
                OnSecondUpdate();
            }
        }
    }

    void FixedUpdate()
    {
        if (!initDoneFlag)
        {
            return;
        }
        OnFixedUpdate();
    }
    void LateUpdate()
    {
        if (!initDoneFlag)
        {
            return;
        }
        OnLateUpdate();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    protected void Init()
    {
        if (initDoneFlag)
        {
            return;
        }
        OnInitFront();
		OnInitSkinFront();
        OnInitSkin();
        OnInitSkinDone();

        Profiler.BeginSample("SpriteBase:UIEventListener BoxCollider");
        Collider[] triggers = this.GetComponentsInChildren<Collider>(true);
        for (int i = 0, max = triggers.Length; i < max; i++)
        {
            Collider trigger = triggers[i];
            if (trigger.gameObject.name.StartsWith("Btn") == true)//以"Btn"开头命名的按钮才会触发OnClick
            {
                UIEventListener listener = UIEventListener.Get(trigger.gameObject);
                listener.onClick = Click;
            }
            colliderList.Add(trigger);
            UIButtonScale btnScale = trigger.gameObject.GetComponent<UIButtonScale>();//设置over没有缩放
            if (btnScale != null)
            {
                btnScale.hover = Vector3.one;
            }
        }
        Profiler.EndSample();
        Profiler.BeginSample("SpriteBase:OnInitDone");
        OnInitDone();
        Profiler.EndSample();
        _initDoneFlag = true;
    }

    /// <summary>
    /// 初始化前
    /// </summary>
    protected virtual void OnInitFront() { }
    /// <summary>
    /// 初始化皮肤前
    /// </summary>
    protected virtual void OnInitSkinFront(){ }
    /// <summary>
    /// 初始化皮肤
    /// </summary>
    protected virtual void OnInitSkin()
    {
        if(mainSkinPath != null)
        {
            _skin = LoadSrc(mainSkinPath);
        }
        else
        {
            _skin = new GameObject("Skin");
        }
        skin.transform.parent = this.transform;
        //skin.transform.localPosition = Vector3.zero;//皮肤的位置不固定--
        skin.transform.localEulerAngles = Vector3.zero;
        skin.transform.localScale = Vector3.one;
    }
    /// <summary>
    /// 初始化皮肤完成
    /// </summary>
    protected virtual void OnInitSkinDone() { }
    /// <summary>
    /// 初始化完成
    /// </summary>
    protected virtual void OnInitDone(){ }

    /// <summary>
    /// 基于秒的update
    /// </summary>
    protected virtual void OnSecondUpdate(){ }
    protected virtual void OnUpdate() { }
    protected virtual void OnFixedUpdate() { }
    protected virtual void OnLateUpdate() { }
    protected virtual void OnDestroyFront() { }
    protected virtual void OnOnDestroy() { }
    protected virtual void OnDestroyDone() { }
    /// <summary>
    /// 点击按钮
    /// </summary>
    /// <param name="target">点击的目标对象</param>
    protected virtual void OnClick(GameObject target) { }

    /// <summary>
    /// 设置该对象下所有boxcollider是否可以交互
    /// </summary>
    /// <param name="enabled"></param>
    public void SetColliderEnabled(bool enabled)
    {
        foreach(BoxCollider bc in colliderList)
        {
            bc.enabled = enabled;
        }
    }

    protected void Click(GameObject target)
    {
        if (initDoneFlag)
        {
            OnClick(target);
        }
    }
    /// <summary>
    /// 设置主skin
    /// </summary>
    /// <param name="path"></param>
    protected void SetMainSkinPath(string path)
    {
        if(initDoneFlag)
        {
            Debug.LogWarning("初始化已完成，请在初始化皮肤前设置主皮肤！path=" + path);
        }
        mainSkinPath = path;
    }
    /// <summary>
    /// 加载资源
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    protected GameObject LoadSrc(string path)
    {
        return ResourceMgr.GetInstance().CreateGameObject(path,false);
    }

}

