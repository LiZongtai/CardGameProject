using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.Events;
public class GameManager : MonoBehaviour
{
    //Bool
    

    //GameObject
    private GameObject player0;
    private GameObject player1;
    private GameObject player2;
    private GameObject player3;
    private GameObject showPanel;
    private GameObject sciSelect;
    private GameObject cardActing;
    private GameObject cardISActing;
    private GameObject actingCard;
    private GameObject gamePanel;

    //Text
    private Text cardPileText;
    private Text discardPileText;

    //Image
    private Image maskBG;

    private int playerNum=0;
    private int cardNum = 0;

    //List
    public static List<Card> cardPile = new List<Card>();
    public static List<Card> disCardPile = new List<Card>();
    public static List<Patent> patentAll = new List<Patent>();
    public static List<GameObject> patentObjectAll = new List<GameObject>();

    //Button
    private Button btn_ie;
    private Button btn_se;
    private Button btn_ds;
    private Button maskButton;
    private Button btn_draw;

    // Start is called before the first frame update

    void Start()
    {
        Init();
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void Init()
    {
        showPanel = transform.Find("ShowPanel").gameObject;
        sciSelect = transform.Find("ShowPanel/SciSelect").gameObject;
        cardActing = transform.Find("ShowPanel/CardActing").gameObject;
        btn_ie = transform.Find("ShowPanel/CardActing/btn_ie").GetComponent<Button>();
        showPanel.SetActive(true);
        cardActing.SetActive(false) ;
        sciSelect.SetActive(true);
        maskButton = transform.Find("ShowPanel/CardActing/Button").GetComponent<Button>();
        maskBG = transform.Find("ShowPanel/CardActing/GameBGMask").GetComponent<Image>();
        actingCard = transform.Find("ShowPanel/CardActing/Card").gameObject;
        btn_ie = transform.Find("ShowPanel/CardActing/btn_ie").GetComponent<Button>();
        btn_se = transform.Find("ShowPanel/CardActing/btn_se").GetComponent<Button>();
        btn_ds = transform.Find("ShowPanel/CardActing/btn_ds").GetComponent<Button>();
        btn_draw = transform.Find("btn_draw").GetComponent<Button>();
        gamePanel = transform.Find("GamePanel").gameObject;
        cardPileText = transform.Find("GameBG/CardPile/Mask/Count").GetComponent<Text>();
        discardPileText = transform.Find("GameBG/DisPile/Mask/Count").GetComponent<Text>();

        CardDataManager.GetCardData();
        cardNum = CardDataManager.GetCardNum();
        playerNum = Models.GameModel.playNum;
        Debug.Log(playerNum);
        PlayerSet();
        Croupier();

        EventCenter.AddListener<GameObject>(EventDefine.CardActing, CardActing);
        EventCenter.AddListener(EventDefine.StopCardActing, StopCardActing);
        EventCenter.AddListener<GameObject>(EventDefine.PatentShowing, PatentShow);
        EventCenter.AddListener<GameObject>(EventDefine.PatentEndShowing, EndPatentShow);

        //A Round


        maskButton.onClick.AddListener(StopCardActing);
        btn_ie.onClick.AddListener(ActingIE);
        btn_se.onClick.AddListener(ActingSE);
        btn_ds.onClick.AddListener(ActingDS);
        btn_draw.onClick.AddListener(() =>
        {
            DrawCard(0, 1);
        });

        //Test
        //float screenWidth = Screen.width;
        //float screenHeight = Screen.height;
        //Rect screenRect = new Rect(-screenWidth / 2, -screenHeight / 2, screenWidth, screenHeight);
        //Debug.Log(screenRect.xMin + " " + screenRect.yMin + " " + screenRect.xMax + " " + screenRect.yMax);
    }
    private void PlayerSet()
    {
        player0 = transform.Find("Player0").gameObject;
        player1 = transform.Find("Player1").gameObject;
        player2 = transform.Find("Player2").gameObject;
        player3 = transform.Find("Player3").gameObject;
        switch (playerNum)
        {
            case 2:
                player2.SetActive(false);
                player3.SetActive(false);
                break;
            case 3:
                player2.SetActive(true);
                player3.SetActive(false);
                //TODO
                break;
            case 4:
                player2.SetActive(true);
                player3.SetActive(true);
                break;
        }
    }
    /// <summary>
    /// 发牌
    /// </summary>
    private void Croupier()
    {
        for(int i = 0; i < cardNum; i++)
        {
            Card card = new Card(i,CardDataManager.GetCardSubject(i),CardDataManager.GetCardType(i));
            cardPile.Add(card);
            
        }
        cardPileText.text = cardPile.Count.ToString();
        discardPileText.text = disCardPile.Count.ToString();
        ShuffleCardPile();
        int randStartPlayer = Random.Range(0, playerNum);
        for(int i = 0; i < playerNum; i++)
        {
            //cardPileText.text = (cardPile.Count - (playerNum - 1) * 3).ToString();
            switch (i)
            {
                case 0:
                    EventCenter.Broadcast(EventDefine.DrawCard0, 3);
                    break;
                case 1:
                    EventCenter.Broadcast(EventDefine.DrawCard1, 3);
                    Debug.Log("player 1 draw card");
                    break;
                case 2:
                    EventCenter.Broadcast(EventDefine.DrawCard2, 3);
                    Debug.Log("player 2 draw card");
                    break;
                case 3:
                    EventCenter.Broadcast(EventDefine.DrawCard3, 3);
                    Debug.Log("player 3 draw card");
                    break;
            }
        }
    }
    /// <summary>
    /// 洗牌
    /// </summary>
    private void ShuffleCardPile()
    {
        for(int i = 0; i < cardNum; i++)
        {
            int rand = Random.Range(0, cardNum);
            Card temp = cardPile[i];
            cardPile[i] = cardPile[rand];
            cardPile[rand] = temp;
        }
    }
    private void CardActing(GameObject go)
    {
        showPanel.SetActive(true);
        cardActing.SetActive(true);
        actingCard.SetActive(true);
        maskBG.DOFade((float)0.75, 0.05f);
        cardActing.GetComponent<Image>().DOFade((float)0.75, 0.05f);
        actingCard.GetComponent<CardUI>().GetCardId(go.GetComponent<CardUI>().CardID);
        actingCard.GetComponent<CardUI>().CardInit();
        actingCard.GetComponent<CardUI>().ShowEffect(0.05f);

    }
    public void StopCardActing()
    {
        Tweener tweener= maskButton.image.DOFade(0, 0.05f);
        actingCard.GetComponent<CardUI>().EndShowEffect(0.05f);
        maskBG.DOFade(0, 0.05f);
        tweener.OnComplete(() =>
        {
            showPanel.SetActive(false);
            cardActing.SetActive(false);
            actingCard.SetActive(false);
        });
    }
    private void ActingIE()
    {
        Debug.Log("Acting IE");
    }
    private void ActingSE()
    {
        EventCenter.Broadcast(EventDefine.Card2Patent, actingCard.GetComponent<CardUI>().CardID, actingCard.GetComponent<CardUI>().CardSubject);
        Debug.Log("Acting SE");
    }
    private void ActingDS()
    {
        StopCardActing();
        Debug.Log("Acting Ab");
        EventCenter.Broadcast(EventDefine.CardDiscard, actingCard.GetComponent<CardUI>().CardID);
        Card card = new Card(actingCard.GetComponent<CardUI>().CardID, actingCard.GetComponent<CardUI>().CardSubject, actingCard.GetComponent<CardUI>().CardType);
        disCardPile.Add(card);
        discardPileText.text = disCardPile.Count.ToString();
        for (int i = 0; i < disCardPile.Count; i++)
        {
            Debug.Log("disCardPile: " + disCardPile[i].Id);
        }
    }
    private void DrawCard(int player,int num)
    {
        cardPileText.text = (cardPile.Count - num).ToString();
        switch (player)
        {
            case 0:
                EventCenter.Broadcast(EventDefine.DrawCard0, num);
                Debug.Log("player 0 draw card");
                break;
            case 1:
                EventCenter.Broadcast(EventDefine.DrawCard1,num);
                Debug.Log("player 1 draw card");
                break;
            case 2:
                EventCenter.Broadcast(EventDefine.DrawCard2, num);
                Debug.Log("player 2 draw card");
                break;
            case 3:
                EventCenter.Broadcast(EventDefine.DrawCard3, num);
                Debug.Log("player 3 draw card");
                break;
        }
    }
    private void PatentShow(GameObject go)
    {
        go.GetComponent<PatentUI>().PatentInfoShow();
    }
    private void EndPatentShow(GameObject go)
    {
        go.GetComponent<PatentUI>().EndPatentInfoShow();
    }


}
