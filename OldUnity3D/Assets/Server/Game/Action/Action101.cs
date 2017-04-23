using System;
using UnityEngine;

public class Action101 : GameAction
{
    private ActionResult actionResult;

    public Action101()
        : base(101)
    {
    }

    protected override void SendParameter(NetWriter writer, ActionParam actionParam)
    {
        writer.writeString("Account", actionParam.Get<string>("account"));
        writer.writeString("Password", actionParam.Get<string>("password"));
        writer.writeString("DeviceID", GameSetting.Instance.DeviceID);
    }

    protected override void DecodePackage(NetReader reader)
    {
        actionResult = new ActionResult();
        actionResult["Relu"] = reader.getInt();
    }

    public override ActionResult GetResponseData()
    {
        return actionResult;
    }
}
