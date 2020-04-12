using Protocol.Code;
using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
                Debug.Log("wrong subCode");
                break;
        }
    }
    private void Register_SRES(int value)
    {
        if (value == -1)
        {
            Debug.Log("Username is already Registered!");
            EventCenter.Broadcast(EventDefine.Hint, "Username is already Registered!");
            return;
        }
        if (value == 0)
        {
            Debug.Log("Registration is Successful!");
            EventCenter.Broadcast(EventDefine.Hint, "Registration is Successful!");
        }
    }
}
