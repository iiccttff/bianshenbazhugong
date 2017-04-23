using System;
using UnityEngine;

public class Action1005 : GameAction
{
    private ActionResult actionResult;

    public Action1005()
        : base(1005)
    {
    }

    protected override void SendParameter(NetWriter writer, ActionParam actionParam)
    {
        writer.writeString("UserName", actionParam.Get<string>("roleName"));
        writer.writeInt32("Sex", actionParam.Get<int>("Sex"));
        writer.writeString("HeadID", "head.gif");
        writer.writeString("RetailID", GameSetting.Instance.RetailID);
        writer.writeString("Pid", GameSetting.Instance.Pid);
        writer.writeInt32("MobileType", GameSetting.Instance.MobileType);
        writer.writeInt32("ScreenX", GameSetting.Instance.ScreenX);
        writer.writeInt32("ScreenY", GameSetting.Instance.ScreenY);
        writer.writeString("ClientAppVersion", GameSetting.Instance.ClientAppVersion);
        writer.writeInt32("GameType", GameSetting.Instance.GameType);
        writer.writeInt32("ServerID", GameSetting.Instance.ServerID);
    }

    protected override void DecodePackage(NetReader reader)
    {
        actionResult = new ActionResult();
    }

    public override ActionResult GetResponseData()
    {
        return actionResult;
    }
}
