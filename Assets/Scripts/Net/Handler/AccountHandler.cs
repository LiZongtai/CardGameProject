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
            case AccountCode.Login_SRES:
                Login_SRES((int)value);
                break;
            case AccountCode.GetUserInfo_SRES:
                Models.GameModel.userDto = (UserDto)value;
                //jump to main scene
                SceneManager.LoadScene("Main");
                //Debug.Log(Models.GameModel.userDto.UserName + "" + Models.GameModel.userDto.IconName + "" + Models.GameModel.userDto.CoinCount.ToString());
                break;
            default:
                Debug.Log("wrong subCode");
                break;
        }
    }
    private void Login_SRES(int value)
    {
        if (value == -1)
        {
            EventCenter.Broadcast(EventDefine.Hint, "username doesn't exist!");
        }
        else if (value == -2)
        {
            EventCenter.Broadcast(EventDefine.Hint, "password is wrong!");
        }
        else if (value == -3)
        {
            EventCenter.Broadcast(EventDefine.Hint, "your account is already online");
        }
        else if (value == 0)
        {
            NetMsgCenter.Instance.SendMsg(OpCode.Account, AccountCode.GetUserInfo_CREQ, null);
            EventCenter.Broadcast(EventDefine.Hint, "login succeeds");
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
