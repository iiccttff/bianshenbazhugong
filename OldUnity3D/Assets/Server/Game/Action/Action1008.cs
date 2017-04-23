using System;
using UnityEngine;

public class Action1008 : GameAction
{
    private ActionResult actionResult;

    public Action1008()
        : base(1008)
    {
    }

    protected override void SendParameter(NetWriter writer, ActionParam actionParam)
    {

    }

    protected override void DecodePackage(NetReader reader)
    {

        UserModel userModel = ModelMgr.GetInstance().GetModel<UserModel>();
        actionResult = new ActionResult();
        actionResult["NickName"] = reader.readString();
        actionResult["UserId"] = reader.getInt();
        actionResult["PassportId"] = reader.readString();
        actionResult["Sex"] = reader.getInt();
        actionResult["Action"] = reader.getInt();
        actionResult["Exp"] = reader.getInt();
        actionResult["Lv"] = reader.getInt();
        actionResult["Gold"] = reader.getInt();
        actionResult["Ingot"] = reader.getInt();

        

        userModel.name = actionResult.Get<string>("NickName");
        userModel.sex = actionResult.Get<int>("Sex");
        userModel.action = actionResult.Get<int>("Action");
        userModel.exp = actionResult.Get<int>("Exp");
        userModel.lv = actionResult.Get<int>("Lv");
        userModel.gold = actionResult.Get<int>("Gold");
        userModel.ingot = actionResult.Get<int>("Ingot");
    }

    public override ActionResult GetResponseData()
    {
        return actionResult;
    }
}
