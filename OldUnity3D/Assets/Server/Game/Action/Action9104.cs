using System;
using UnityEngine;

public class Action9104 : GameAction
{
    private ActionResult actionResult;

    public Action9104()
        : base(9104)
    {
    }

    protected override void SendParameter(NetWriter writer, ActionParam actionParam)
    {

        writer.writeString("FriendID", actionParam.Get<string>("friendID"));
    }

    protected override void DecodePackage(NetReader reader)
    {
        actionResult = new ActionResult();
        actionResult["isSuccess"] = reader.getInt();

    }

    public override ActionResult GetResponseData()
    {
        return actionResult;
    }
}
