using Protocol.Code;
using Protocol.Dto;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour
{
    private InputField input_username;
    private InputField input_password;
    private Button btn_login;
    private Button btn_register;
    private void Awake()
    {
        EventCenter.AddListener(EventDefine.ShowLoginPanel,Show);
        init();
    }
    private void init()
    {
        input_username = transform.Find("input_username").GetComponent<InputField>();
        input_password = transform.Find("input_password").GetComponent<InputField>();
        btn_login = transform.Find("btn_login").GetComponent<Button>();
        btn_register = transform.Find("btn_register").GetComponent<Button>();
        btn_register.onClick.AddListener(OnRegisterButtonClick);
        btn_login.onClick.AddListener(OnLoginButtonClick);
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowLoginPanel, Show);
    }
    private void OnRegisterButtonClick()
    {
        EventCenter.Broadcast(EventDefine.ShowRegisterPanel);
    }
    private void OnLoginButtonClick()
    {
        if (input_username.text == null || input_username.text == "")
        {
            EventCenter.Broadcast(EventDefine.Hint, "请输入用户名");
            //Debug.Log("请输入用户名");
            return;
        }
        if (input_password.text == null || input_password.text == "")
        {
            EventCenter.Broadcast(EventDefine.Hint, "请输入密码");
            //Debug.Log("请输入密码");
            return;
        }
        // login request to server
        AccountDto dto = new AccountDto(input_username.text, input_password.text);
        NetMsgCenter.Instance.SendMsg(OpCode.Account, AccountCode.Login_CREQ, dto);
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
}
