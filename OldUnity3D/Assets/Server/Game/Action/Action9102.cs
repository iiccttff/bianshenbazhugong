using System;
using UnityEngine;
using System.Collections.Generic;

public class Action9102 : GameAction
{
    private ActionResult actionResult;

    public Action9102()
        : base(9102)
    {
    }

    protected override void SendParameter(NetWriter writer, ActionParam actionParam)
    {

        //writer.writeInt32("PageIndex", actionParam.Get<int>("pageIndex"));
        //writer.writeInt32("PageSize", actionParam.Get<int>("pageSize"));
    }

    protected override void DecodePackage(NetReader reader)
    {
        actionResult = new ActionResult();
        //actionResult["PageCount"] = reader.getInt();

        int subRecordCount = reader.getInt();
        List<FriendsServerData> list = new List<FriendsServerData>();
        for (int i = 0; i < subRecordCount; i++)
        {
            FriendsServerData data = new FriendsServerData();
            reader.recordBegin();
            data.friendID = reader.readString();
            data.name = reader.readString();
            data.sex = reader.getShort();
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

public class FriendsServerData
{
    public string friendID;
    public string name;
    public int sex;
    public int lv;
    public DateTime loginTime;
}
