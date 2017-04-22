/*
 * 脚本名(ScriptName)：    PanelPlayInfoAll.cs
 * 作者(Author):           Circle
 * 官网(Url):              http://blog.csdn.net/u013108312
 */
using UnityEngine;
using System.Collections;

public class PanelPlayInfoAll : PanelBase 
{
    #region 初始化相关
    protected override void OnInitSkin()
    {
        base.SetMainSkinPath("Game/UI/Home/PanelPlayInfoAll");
        base.OnInitSkin();
        _type = PanelType.PanelPlayInfoAll;
        _showStyle = PanelMgr.PanelShowStyle.DownToSlide;
    }

    protected override void OnInitDone()
    {
        base.OnInitDone();


    }

    protected override void OnClick(GameObject click)
    {
        base.OnClick(click);
        ClickButton(click);
    }
    #endregion

    public void ClickButton(GameObject click)
    {
        if (click.name.Equals("BtnClose"))
        {
            Colse();
        }
    }
}
