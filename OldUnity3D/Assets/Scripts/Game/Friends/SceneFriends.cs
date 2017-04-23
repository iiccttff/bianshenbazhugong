using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneFriends : SceneBase
{
    #region 界面加载相关
    protected override void OnInitFront()
    {
        base.OnInitFront();
        _type = SceneType.SceneFriends;
    }
    protected override void OnInitSkinFront()
    {
        base.OnInitSkinFront();
        SetMainSkinPath("Game/Friends/SceneFriends");
    }
    protected override void OnInitDone()
    {
        base.OnInitDone();
        InitData();

    }
    protected override void OnDestroyDone()
    {
        base.OnDestroyDone();

    }
    protected override void OnDestroyFront()
    {
        base.OnDestroyFront();
        
    }
    protected override void OnClick(GameObject target)
    {
        base.OnClick(target);
        ButtonClick(target);
    }

    public override void OnInit(params object[] sceneArgs)
    {
        base.OnInit(sceneArgs);
    }
    public override void OnShowed()
    {
        base.OnShowed();

    }
    #endregion

    #region 数据定义
    private GameObject mItem;
    private GameObject mBtnFriends;
    private GameObject mBtnInfo;
    private GameObject mBtnFind;
    private GameObject mNowClickButon;
    private UIInput mInputFriendID;
    private UILabel mContent;
    /// <summary> 好友列表 </summary>
    private List<FriendsServerData> mFriendList;
    private List<GameObject> mNowMoveListItem;
    private int mNowType;
    private UILabel mNoticeLabel;
    private UILabel mId;
    private UILabel mFriednsNumsLabel;
    private UserModel mUserModel;
    #endregion

    #region UI逻辑

    void InitData()
    {
        mUserModel = ModelMgr.GetInstance().GetModel<UserModel>();
        mNowMoveListItem = new List<GameObject>();
        mFriednsNumsLabel = skinTransform.Find("Back/Top/FriendNums").GetComponent<UILabel>();
        mId = skinTransform.Find("Back/Top/ID").GetComponent<UILabel>();
        mItem = skinTransform.Find("PanelMove/Items/Item").gameObject;
        mBtnFriends = skinTransform.Find("Back/BtnFriends").gameObject;
        mBtnInfo = skinTransform.Find("Back/BtnInfo").gameObject;
        mBtnFind = skinTransform.Find("Back/BtnFind").gameObject;
        mInputFriendID = skinTransform.Find("Back/InputFriendID").GetComponent<UIInput>();
        mContent = skinTransform.Find("Back/Content").GetComponent<UILabel>();
        mNoticeLabel = skinTransform.Find("Back/Notice/Sprite/Label").GetComponent<UILabel>();

        //mId.text = mUserModel.i

        mFriednsNumsLabel.text = "0 / 100";

        //mNowClickButon = mBtnFriends;
        ButtonClick(mBtnFriends);
    }

    #endregion

    #region 点击事件

    void ButtonClick(GameObject click)
    {
        if(click.name.Equals("BtnFriends"))
        {
            SwitchButtonType(click,1);
        }
        else if (click.name.Equals("BtnInfo"))
        {
            SwitchButtonType(click,2);
        }
        else if (click.name.Equals("BtnFind"))
        {
            SwitchButtonType(click,3);
        }
    }


    void SwitchButtonType(GameObject click,int type)
    {
        mNoticeLabel.transform.parent.parent.gameObject.SetActive(false);
        GameObject sprite = click.transform.Find("Sprite").gameObject;
        BoxCollider box = click.GetComponent<BoxCollider>();
        sprite.SetActive(true);
        box.enabled = false;

        if (mNowClickButon != null)
        {
            sprite = mNowClickButon.transform.Find("Sprite").gameObject;
            box = mNowClickButon.GetComponent<BoxCollider>();
            sprite.SetActive(false);
            box.enabled = true;
        }
        mNowClickButon = click;
        mNowType = type;
        if(type == 3)
        {
            mContent.gameObject.SetActive(false);
            mInputFriendID.gameObject.SetActive(true);
            Net.Instance.Send((int)ActionType.IntroduceFriend, AddFriendListReturn, null);
        }
        else if(type == 2)
        {
            mContent.gameObject.SetActive(true);
            mInputFriendID.gameObject.SetActive(false);
            mContent.text = "这是显示想要加你为好友的玩家！";
            DestroyMoveList();
            //Net.Instance.Send((int)ActionType.FriendList, FriendListReturn, null);
        }
        else
        {
            mContent.gameObject.SetActive(true);
            mInputFriendID.gameObject.SetActive(false);
            mContent.text = "结交好友，互赠体力，还可获得成就奖励呢！";
            Net.Instance.Send((int)ActionType.FriendList, FriendListReturn, null);
        }

    }
    #endregion

    #region 服务器返回
    /// <summary>
    /// 获取好友列表服务器返回
    /// </summary>
    /// <param name="action"></param>
    void FriendListReturn(ActionResult action)
    {
        if (action == null) return;
        mFriendList = action.Get<List<FriendsServerData>>("list");
        DestroyMoveList();
        ShowItemList(mFriendList, 1);
        mFriednsNumsLabel.text = mFriendList.Count.ToString() + " / 100";
    }

    void AddFriendListReturn(ActionResult action)
    {
        if (action == null) return;
        mFriendList = action.Get<List<FriendsServerData>>("list");
        DestroyMoveList();
        ShowItemList(mFriendList, 3);
    }

    #endregion

    #region 其他逻辑

    void ShowItemList(List<FriendsServerData> list,int type)
    {
        int index = 0;
        if(list.Count <= 0)
        {
            mNoticeLabel.transform.parent.parent.gameObject.SetActive(true);
            mNoticeLabel.text = "没有数据记录";
        }
        foreach(FriendsServerData data in list)
        {
            GameObject item = Instantiate(mItem) as GameObject;
            item.name = index.ToString();
            item.transform.parent = mItem.transform.parent;
            item.transform.localEulerAngles = Vector3.zero;
            item.transform.localScale = Vector3.one;
            item.transform.localPosition = new Vector3(0, 165 - (index * 110), 0);
            FriendsItem friendsItem = item.GetComponent<FriendsItem>();
            friendsItem.InitData(data, type);
            item.SetActive(true);
            index++;
            mNowMoveListItem.Add(item);
        }
    }

    /// <summary> 清空之前列表item </summary>
    void DestroyMoveList()
    {
        foreach(GameObject obj in mNowMoveListItem)
        {
            Destroy(obj);
        }
        mNowMoveListItem.Clear();
    }

    #endregion
}
