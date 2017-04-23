using System;
using UnityEngine;
using System.Collections.Generic;

public class Action9101 : GameAction
{
    private ActionResult actionResult;

    public Action9101()
        : base(9101)
    {
    }

    protected override void SendParameter(NetWriter writer, ActionParam actionParam)
    {

    }

    protected override void DecodePackage(NetReader reader)
    {
        actionResult = new ActionResult();
        actionResult["Count"] = reader.getInt();
        actionResult["PageCount"] = reader.getInt();
        List<FriendsServerData> list = new List<FriendsServerData>();
        int subRecordCount = actionResult.Get<int>("PageCount");
        
        for (int i = 0; i < subRecordCount; i++)
        {
            FriendsServerData data = new FriendsServerData();
            reader.recordBegin();
            data.friendID = reader.readString();
            data.name = reader.readString();
            data.lv = reader.getShort();
            reader.recordEnd();
            list.Add(data);
        }
        actionResult["list"] = list;
    }

    public override ActionResult GetResponseData()
    {
        return actionResult;
    }
}
