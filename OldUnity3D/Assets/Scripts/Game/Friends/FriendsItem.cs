using UnityEngine;
using System.Collections;

public class FriendsItem : MonoBehaviour
{
    private FriendsServerData mData;
    private int mType;
    public UILabel name;
    public UILabel lv;
    public UISprite btn1;
    public UISprite btn2;
    public UILabel btn1Label;
    public UILabel btn2Label;
    public UILabel vip;
    
    public void InitData(FriendsServerData data,int type)
    {
        mData = data;
        mType = type;
        name.text = mData.name;
        lv.text = mData.lv.ToString();
        vip.text = "VIP0";

        if(type == 1)
        {
            btn1.gameObject.name = "BtnGift";
            btn2.gameObject.name = "BtnRemove";
            btn1Label.text = "赠送";
            btn2Label.text = "删除";
        }
        else if(type == 2)
        {
            btn1.gameObject.name = "BtnOk";
            btn2.gameObject.name = "BtnNo";
            btn1Label.text = "同意";
            btn2Label.text = "拒绝";
        }
        else if(type == 3)
        {
            btn1.gameObject.name = "BtnAddFirend";
            btn2.gameObject.SetActive(false);
            btn1Label.text = "添加";
        }
    }
}
