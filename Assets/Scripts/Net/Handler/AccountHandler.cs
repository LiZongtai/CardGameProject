using Protocol.Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountHandler : BaseHandler
{
    public override void OnReceive(int subCode, object value)
    {
        switch (subCode)
        {
            case AccountCode.Register_SRES:
                Register_SRES((int)value);
                break;
            default:
                break;
        }
    }
    //response of resgister server
    private void Register_SRES(int value)
    {
        if (value == -1)
        {
            EventCenter.Broadcast(EventDefine.Hint, "user name has been registered");
            return;
        }
        if (value == 0)
        {
            EventCenter.Broadcast(EventDefine.Hint, "register successfully!");
        }
    }
}
