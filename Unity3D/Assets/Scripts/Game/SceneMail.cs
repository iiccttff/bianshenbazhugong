/*
 * 脚本名(ScriptName)：    SceneMail.cs
 * 作者(Author):           Circle
 * 官网(Url):              http://blog.csdn.net/u013108312
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SceneMail : SceneBase 
{
    #region 初始化相关
    protected override void OnInitSkin()
    {
        base.SetMainSkinPath("Game/UI/SceneMail");
        base.OnInitSkin();
    }

    protected override void OnInitDone()
    {
        base.OnInitDone();
        
        mItem = skinTransform.Find("PanelMove/Items/Item").gameObject;
        ShowItems();
    }

    protected override void OnClick(GameObject click)
    {
        base.OnClick(click);
        ClickButton(click);
    }
    #endregion


    private GameObject mItem;
    private List<GameObject> mItemList;
	
    /// <summary>
    /// 显示Item列表
    /// </summary>
    void ShowItems()
    {
        if (mItem == null)
        {
            Debug.LogError("Item null!!");
            return;
        }

        mItemList = new List<GameObject>();

        for (int i = 0; i < 10; i++)
        {
            GameObject item = Instantiate(mItem) as GameObject;
            item.transform.parent = mItem.transform.parent;
            item.SetActive(true);
            item.transform.localEulerAngles = Vector3.zero;
            item.transform.localScale = Vector3.one;
            InitItem(item, i);
            item.name = i.ToString();
            
        }
    }
    /// <summary>
    /// 初始化item
    /// </summary>
    /// <param name="item"></param>
    /// <param name="index"></param>
    void InitItem(GameObject item,int index)
    {
        //设定显示坐标
        item.transform.localPosition = new Vector3(0, 135 - index * 90, 0);

        GameObject btnDelete = item.transform.Find("BtnDelete").gameObject;

        UIEventListener listener = UIEventListener.Get(btnDelete);
        listener.onClick = ClickButton;

        UILabel title = item.transform.Find("Title").GetComponent<UILabel>();
        UILabel time = item.transform.Find("SendTime").GetComponent<UILabel>();
        title.text = "王麻子的来信 " + index.ToString();
        time.text = DateTime.Now.ToString();
        mItemList.Add(item);
    }

    /// <summary>
    /// 点击事件
    /// </summary>
    /// <param name="click"></param>
    void ClickButton(GameObject click)
    {
        if(click.name.Equals("BtnDelete"))
        {
            //Debug.Log("点击了" + click.transform.parent.name);

            //mItemList.Remove(click.transform.parent.gameObject);
            //Destroy(click.transform.parent.gameObject);
            //ChanagePosition();
            SceneMgr.Instatance.SwitchScene(SceneType.SceneChat);
        }
        else if(click.name.Equals("BtnReturn"))
        {
            SceneMgr.Instatance.SwitchToPrevScene();
        }
    }


    void ChanagePosition()
    {
        for (int i = 0; i < mItemList.Count; i++)
        {
            //GameObject item = mItemList[i];
            mItemList[i].transform.localPosition = new Vector3(0, 135 - i * 90, 0);
        }
    }

}
