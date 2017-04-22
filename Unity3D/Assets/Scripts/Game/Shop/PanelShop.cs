/*
 * 脚本名(ScriptName)：    PanelShop.cs
 * 作者(Author):           Circle
 * 官网(Url):              http://blog.csdn.net/u013108312
 */
using UnityEngine;
using System.Collections;

public class PanelShop : PanelBase 
{
    #region 初始化相关
    protected override void OnInitSkin()
    {
        base.SetMainSkinPath("Game/UI/Shop/PanelShop");
        base.OnInitSkin();
        _type = PanelType.PanelShop;
        _showStyle = PanelMgr.PanelShowStyle.CenterScaleBigNomal;
    }

    protected override void OnInitDone()
    {
        base.OnInitDone();
        int id = (int)sceneArgs[0];
        init();
    }

    protected override void OnClick(GameObject click)
    {
        base.OnClick(click);
        ClickButton(click);
    }
    #endregion

    #region 数据定义
    private UILabel mName;
    //private UILabel mInputNumLabel;
    private UIInput mInput;
    private int mNowSeleNums = 95;
    private UserModel mUserModel;
    #endregion

    #region 初始化

    void init()
    {
        mUserModel = ModelMgr.Instatance.GetModel<UserModel>();

        mName = skinTransform.Find("Name").GetComponent<UILabel>();
        //mInputNumLabel = skinTransform.Find("InputNum/Label").GetComponent<UILabel>();
        mInput = skinTransform.Find("InputNum").GetComponent<UIInput>();
        mInput.value = mNowSeleNums.ToString();
        mInput.onChange.Add(new EventDelegate(ChanageInput));
    }

    void ChanageInput()
    {
        Debug.Log("Input值改变了" + mInput.value);
        //int nums = 1;
        int.TryParse(mInput.value, out mNowSeleNums);
        //mNowSeleNums = int.Parse(mInput.value);
    }

    #endregion

    #region 点击事件
    void ClickButton(GameObject click)
    {
        if(click.name.Equals("BtnAdd"))
        {
            if(mNowSeleNums < 99)
            {
                mNowSeleNums++;
            }
            mInput.value = mNowSeleNums.ToString();
        }
        else if (click.name.Equals("BtnSub"))
        {
            if (mNowSeleNums > 1)
            {
                mNowSeleNums--;
            }
            mInput.value = mNowSeleNums.ToString();
        }
        else if(click.name.Equals("BtnClose"))
        {
            Colse();
        }
        else if(click.name.Equals("BtnShop"))
        {
            mUserModel.gold -= 100;
            //Debug.Log("发送购买消息");
        }
    }
    #endregion
}
