using Protocol.Code;
using Protocol.Dto;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RegisterPanel : MonoBehaviour
{
    private InputField input_username;
    private InputField input_password;
    private Button btn_back;
    private Button btn_pwd;
    private Button btn_register;
    private bool isShowPassword;

    private void Awake()
    {
        EventCenter.AddListener(EventDefine.ShowRegisterPanel, Show);
        init();
        gameObject.SetActive(false);
    }

    private void init()
    {
        input_username = transform.Find("username/input_username").GetComponent<InputField>();
        input_password = transform.Find("password/input_password").GetComponent<InputField>();
        btn_back = transform.Find("btn_back").GetComponent<Button>();
        btn_pwd = transform.Find("btn_pwd").GetComponent<Button>();
        btn_register = transform.Find("btn_register").GetComponent<Button>();

        btn_back.onClick.AddListener(OnBackButtonClick);
        btn_register.onClick.AddListener(OnRegisterButtonClick);
        btn_pwd.onClick.AddListener(OnPwdButtonClick);
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowRegisterPanel, Show);
    }
    private void OnBackButtonClick()
    {
        EventCenter.Broadcast(EventDefine.ShowLoginPanel);
        //Debug.Log("back to Login Panel");
        gameObject.SetActive(false);
    }
    private void OnPwdButtonClick()
    {
        isShowPassword = !isShowPassword;
        if (isShowPassword)
        {
            input_password.contentType = InputField.ContentType.Standard;
            btn_pwd.GetComponentInChildren<Text>().text = "隐藏";
        }
        else
        {
            input_password.contentType = InputField.ContentType.Password;
            btn_pwd.GetComponentInChildren<Text>().text = "显示";
        }
        EventSystem.current.SetSelectedGameObject(input_password.gameObject);
    }
    private void OnRegisterButtonClick()
    {
        //Debug.Log("click register");
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
        // TODO
        Debug.Log(input_username.text + input_password.text);
        AccountDto dto = new AccountDto(input_username.text, input_password.text);
        NetMsgCenter.Instance.SendMsg(OpCode.Account, AccountCode.Register_CREQ, dto);
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }


}
