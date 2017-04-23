using System;
using UnityEngine;

public class Action100 : GameAction
{
    private ActionResult actionResult;

    public Action100()
        : base(100)
    {
    }

    protected override void SendParameter(NetWriter writer, ActionParam actionParam)
    {
        //writer.writeString("str", actionParam.Get<string>("str"));
    }

    protected override void DecodePackage(NetReader reader)
    {
        actionResult = new ActionResult();
        actionResult["content"] = reader.readString();

    }

    public override ActionResult GetResponseData()
    {
        return actionResult;
    }
}
