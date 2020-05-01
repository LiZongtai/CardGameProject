using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour
{
    private Text txt_username;
    private Text txt_coincount;
    private Image headIcon;
    private Button btn_rank;
    private Button btn_bank;
    private Button btn_single;
    private Button btn_online;
    private void Awake()
    {
        Init();
        
    }
    private void Init()
    {
        txt_username = transform.Find("txt_username").GetComponent<Text>();
        txt_coincount = transform.Find("txt_coincount").GetComponent<Text>();
        headIcon = transform.Find("Mask/headIcon").GetComponent<Image>();
        btn_rank = transform.Find("btn_rank").GetComponent<Button>();
        btn_bank = transform.Find("btn_bank").GetComponent<Button>();
        btn_online = transform.Find("Room/btn_online").GetComponent<Button>();
        btn_single = transform.Find("Room/btn_single").GetComponent<Button>();

        txt_username.text = Models.GameModel.userDto.UserName;
        txt_coincount.text = Models.GameModel.userDto.CoinCount.ToString();
        headIcon.sprite = ResourcesManager.GetSprite(Models.GameModel.userDto.IconName);
    }
}
