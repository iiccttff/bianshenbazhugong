using System;
using UnityEngine;

public class Action9103 : GameAction
{
    private ActionResult actionResult;

    public Action9103()
        : base(9103)
    {
    }

    protected override void SendParameter(NetWriter writer, ActionParam actionParam)
    {

        writer.writeString("FriendID", actionParam.Get<string>("friendID"));
        writer.writeString("FriendName", actionParam.Get<string>("friendName"));
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
