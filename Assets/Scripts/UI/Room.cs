using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public enum GameType
{
    Online,
    Single,
};
public class Room : MonoBehaviour
{
    private GameType gameType;
    private Button btn_single;
    private Button btn_online;
    private Button btn_2;
    private Button btn_3;
    private Button btn_4;
    private Text btn_2_text;
    private Text btn_3_text;
    private Text btn_4_text;
    private void Awake()
    {
        Init();
    }
    private void Init()
    {
        Color color_0 = new Color(1, 1, 1, 0);
        Color color_1 = new Color(1, 1, 1, 1);
        btn_online = transform.Find("btn_online").GetComponent<Button>();
        btn_single = transform.Find("btn_single").GetComponent<Button>();
        btn_2 = transform.Find("btn_2").GetComponent<Button>();
        btn_3 = transform.Find("btn_3").GetComponent<Button>();
        btn_4 = transform.Find("btn_4").GetComponent<Button>();
        btn_2_text = transform.Find("btn_2/Text").GetComponent<Text>();
        btn_3_text = transform.Find("btn_3/Text").GetComponent<Text>();
        btn_4_text = transform.Find("btn_4/Text").GetComponent<Text>();

        btn_2.gameObject.SetActive(false);
        btn_3.gameObject.SetActive(false);
        btn_4.gameObject.SetActive(false);
        btn_2.image.color = color_0;
        btn_3.image.color = color_0;
        btn_4.image.color = color_0;
        btn_2_text.color = color_0;
        btn_3_text.color = color_0;
        btn_4_text.color = color_0;


        btn_single.onClick.AddListener(() =>
        {
            EventCenter.Broadcast(EventDefine.ShowRoomChoosePanel, GameType.Single);
            show_single();
        });

        btn_online.onClick.AddListener(() =>
        {
            EventCenter.Broadcast(EventDefine.ShowRoomChoosePanel, GameType.Online);
            show_online();
            
        });
        btn_2.onClick.AddListener(() =>
        {
            Models.GameModel.playNum = 2;
            EnterRoom();
        });
        btn_3.onClick.AddListener(() =>
        {
            Models.GameModel.playNum = 3;
            EnterRoom();
        });
        btn_4.onClick.AddListener(() =>
        {
            Models.GameModel.playNum = 4;
            EnterRoom();
        });

    }
    private void show_single()
    {
        gameType = GameType.Single;
        btn_online.image.DOFade(0, 2);
        btn_online.gameObject.SetActive(false);
        btn_2.gameObject.SetActive(true);
        btn_3.gameObject.SetActive(true);
        btn_4.gameObject.SetActive(true);
        btn_2.image.DOFade(1, 2);
        btn_3.image.DOFade(1, 2);
        btn_4.image.DOFade(1, 2);
        btn_2_text.DOFade(1, 2);
        btn_3_text.DOFade(1, 2);
        btn_4_text.DOFade(1, 2);
    }
    private void show_online()
    {
        gameType = GameType.Online;
        btn_single.image.DOFade(0, 2);
        btn_single.gameObject.SetActive(false);
        btn_2.gameObject.SetActive(true);
        btn_3.gameObject.SetActive(true);
        btn_4.gameObject.SetActive(true);
        float posX = -175;
        float posY = 100;
        Vector2 v2 = new Vector2(posX, posY);
        Vector2 v3 = new Vector2(posX, posY - 100);
        Vector2 v4 = new Vector2(posX, posY - 200);
        btn_2.transform.localPosition = v2;
        btn_3.transform.localPosition = v3;
        btn_4.transform.localPosition = v4;
        btn_2.image.DOFade(1, 2);
        btn_3.image.DOFade(1, 2);
        btn_4.image.DOFade(1, 2);
        btn_2_text.DOFade(1, 2);
        btn_3_text.DOFade(1, 2);
        btn_4_text.DOFade(1, 2);
    }
    private void EnterRoom()
    {
        switch (gameType) 
        {
            case GameType.Online:
                SceneManager.LoadScene("Online");
                break;
            case GameType.Single:
                SceneManager.LoadScene("Single");
                break;
        }
    }
}
