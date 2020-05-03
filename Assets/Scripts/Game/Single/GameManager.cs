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
    //Game Process Contol
    public static bool isGameStart = false;
    private bool isRoundStart = false;
    private bool isPlayerRoundStart = false;
    private bool isBeforeRound = false;
    private bool isBeforeAction = false;
    private bool isAfterAction = false;
    private bool isBeforeDraw = false;
    private bool isBeforeDiscard = false;
    private bool isAfterDiscard = false;
    private bool isBeforeMaintenance = false;
    private bool isPlayerRoundEnd = false;
    private bool isRoundEnd = false;
    private bool isGameEnd = false;
    private int RoundNum=0;
    private int PlayerStart;

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

    //Int
    private int playerNum=0;
    private int cardNum = 0;
    private int CurPlayer;

    //List
    private int[] PlayerDrawNum;
    public static List<Card> cardPile = new List<Card>();
    public static List<Card> disCardPile = new List<Card>();
    public static List<Patent> patentAll = new List<Patent>();
    public static List<GameObject> patentObjectAll = new List<GameObject>();

    //Button
    private Button btn_ie;
    private Button btn_se;
    private Button btn_ds;
    private Button btn_show;
    private Button maskButton;
    private Button btn_draw;
    private Button startGame;

    // Start is called before the first frame update
    private void Awake()
    {
        Init();
    }
    void Start()
    {

        //StartCoroutine(GameLoop());
    }
    // Update is called once per frame
    void Update()
    {

    }
    #region Game Process
    private IEnumerator Fade()
    {
        Debug.Log("Fade hhh");
        yield return null;
    }
    private IEnumerator StartLoop()
    {
        if (isGameStart == false)
        {
            StartLoop();
        }
        else
        {
            GameLoop();
        }
        yield return null;
    }
    /// <summary>
    /// 游戏主循环
    /// </summary>
    /// <returns></returns>
    private IEnumerator GameLoop()
    {
        //isGameStart = false;
        yield return StartCoroutine(GameStart());
        yield return StartCoroutine(RoundLoop());
    }
    /// <summary>
    /// 回合循环
    /// </summary>
    /// <returns></returns>
    private IEnumerator RoundLoop()
    {
        yield return StartCoroutine(RoundStart());
        for (int i = PlayerStart; i < PlayerStart+playerNum; ++i)
        {
            CurPlayer = i % playerNum;
            yield return StartCoroutine(PlayerRoundStart());
            yield return StartCoroutine(BeforeRound());
            yield return StartCoroutine(BeforeAction());
            yield return StartCoroutine(AfterAction());
            yield return StartCoroutine(BeforeDraw());
            yield return StartCoroutine(BeforeDiscard());
            yield return StartCoroutine(AfterDiscard());
            yield return StartCoroutine(BeforeMaintenance());
            yield return StartCoroutine(PlayerRoundEnd());
            if (isGameEnd)
            {
                break;
            }
        }
        if (isGameEnd)
        {
            yield return StartCoroutine(GameEnd());
        }
        else
        {
            Debug.Log("Game Not End");
            yield return StartCoroutine(RoundEnd());
            StartCoroutine(RoundLoop());
        }        
    }
    /// <summary>
    /// 游戏开始
    /// </summary>
    /// <returns></returns>
    private IEnumerator GameStart()
    {
        Croupier();
        RoundNum = 1;
        cardPileText.text = cardPile.Count.ToString();
        EventCenter.Broadcast<string>(EventDefine.Hint, "Game Start");
        Debug.Log("Game Start");
        yield return new WaitForSeconds(2f);
    }
    /// <summary>
    /// 游戏结束
    /// </summary>
    /// <returns></returns>
    private IEnumerator GameEnd()
    {
        EventCenter.Broadcast<string>(EventDefine.Hint, "Game End");
        Debug.Log("Game End");
        yield return null;
    }
    /// <summary>
    /// 回合开始
    /// </summary>
    /// <returns></returns>
    private IEnumerator RoundStart()
    {
        isRoundStart = false;
        EventCenter.Broadcast<string>(EventDefine.Hint, "Round " + RoundNum + " Start");
        Debug.Log("Round " + RoundNum + " Start");
        yield return new WaitForSeconds(2f);
    }
    /// <summary>
    /// 玩家回合开始
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayerRoundStart()
    {
        isPlayerRoundStart = false;
        if (RoundNum == 1 && CurPlayer == 0)
        {
            EventCenter.Broadcast(EventDefine.InitHandCard);
            EventCenter.Broadcast<string>(EventDefine.Hint, "你的回合");
        }
        else if (CurPlayer == 0)
        {
            EventCenter.Broadcast<string>(EventDefine.Hint, "你的回合");
        }
        else
        {
            EventCenter.Broadcast<string>(EventDefine.Hint, "Player " + CurPlayer + "'s Turn");
            Debug.Log("Player " + CurPlayer + "'s Turn");
        }
        yield return new WaitForSeconds(2f);
    }
    /// <summary>
    /// 回合开始阶段
    /// </summary>
    /// <returns></returns>
    private IEnumerator BeforeRound()
    {
        isBeforeRound = false;
        yield return null;
    }
    /// <summary>
    /// 行动阶段开始
    /// </summary>
    /// <returns></returns>
    private IEnumerator BeforeAction()
    {
        isBeforeAction = false;
        EventCenter.Broadcast<string>(EventDefine.Hint, "行动阶段");
        Debug.Log("Player " + CurPlayer + " is Acting");
        while (!isBeforeAction)
        {
            Debug.Log("is Acting");
            yield return null;
        }
        
    }
    /// <summary>
    /// 行动阶段结束
    /// </summary>
    /// <returns></returns>
    private IEnumerator AfterAction()
    {
        isAfterAction = false;
        Debug.Log("Player " + CurPlayer + " After Action");
        yield return null;
    }
    /// <summary>
    /// 研究阶段开始
    /// </summary>
    /// <returns></returns>
    private IEnumerator BeforeDraw()
    {
        isBeforeDraw = false;
        EventCenter.Broadcast<string>(EventDefine.Hint, "研究阶段");
        Debug.Log("研究阶段");
        switch (CurPlayer)
        {
            case 0:
                EventCenter.Broadcast(EventDefine.DrawCard0, PlayerDrawNum[0]);
                Debug.Log("player 0 draw card");
                break;
            case 1:
                EventCenter.Broadcast(EventDefine.DrawCard1, PlayerDrawNum[1]);
                Debug.Log("player 1 draw card");
                break;
            case 2:
                EventCenter.Broadcast(EventDefine.DrawCard2, PlayerDrawNum[2]);
                Debug.Log("player 2 draw card");
                break;
            case 3:
                EventCenter.Broadcast(EventDefine.DrawCard3, PlayerDrawNum[3]);
                Debug.Log("player 3 draw card");
                break;
        }
        while (!isBeforeDraw)
        {
            Debug.Log("is Drawing");
            yield return null;
        }
        yield return new WaitForSeconds(2f);
    }
    /// <summary>
    /// 研究阶段弃牌
    /// </summary>
    /// <returns></returns>
    private IEnumerator BeforeDiscard()
    {
        isBeforeDiscard = false;
        EventCenter.Broadcast(EventDefine.BeforeDiscard);
        while (!isBeforeDiscard)
        {
            Debug.Log("is Discarding");
            yield return null;
        }
    }
    /// <summary>
    /// 研究阶段结束
    /// </summary>
    /// <returns></returns>
    private IEnumerator AfterDiscard()
    {
        isAfterDiscard = false;
        yield return null;
    }
    /// <summary>
    /// 维护阶段开始
    /// </summary>
    /// <returns></returns>
    private IEnumerator BeforeMaintenance()
    {
        EventCenter.Broadcast<string>(EventDefine.Hint, "维护阶段");
        EventCenter.Broadcast(EventDefine.BeforeMaintenance);
        isBeforeMaintenance = false;
        while (!isBeforeMaintenance)
        {
            Debug.Log("Maintenance");
            yield return null;
        }

    }
    /// <summary>
    /// 玩家回合结束
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayerRoundEnd()
    {
        isPlayerRoundEnd = false;
        yield return new WaitForSeconds(2f);
    }
    /// <summary>
    /// 回合结束
    /// </summary>
    /// <returns></returns>
    private IEnumerator RoundEnd()
    {
        isRoundEnd = false;
        ++RoundNum;
        yield return new WaitForSeconds(2f);
    }
    #endregion
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
        btn_show = transform.Find("ShowPanel/CardActing/btn_show").GetComponent<Button>();
        btn_draw = transform.Find("btn_draw").GetComponent<Button>();
        gamePanel = transform.Find("GamePanel").gameObject;
        cardPileText = transform.Find("GameBG/CardPile/Mask/Count").GetComponent<Text>();
        discardPileText = transform.Find("GameBG/DisPile/Mask/Count").GetComponent<Text>();

        CardDataManager.GetCardData();
        cardNum = CardDataManager.GetCardNum();
        playerNum = Models.GameModel.playNum;

        Debug.Log(playerNum);
        PlayerSet();
        

        EventCenter.AddListener<GameObject>(EventDefine.CardActing, CardActing);
        EventCenter.AddListener(EventDefine.StopCardActing, StopCardActing);
        EventCenter.AddListener<GameObject>(EventDefine.PatentShowing, PatentShow);
        EventCenter.AddListener<GameObject>(EventDefine.PatentEndShowing, EndPatentShow);

        //Game Process
        startGame = transform.Find("StartGame").GetComponent<Button>();
        startGame.onClick.AddListener(() =>
        {
            StartCoroutine("GameLoop");
            isGameStart = true;
            startGame.gameObject.SetActive(false);
            EventCenter.Broadcast(EventDefine.GameStart);
        });
        //EventCenter.AddListener(EventDefine.GameStart, () =>
        //{
        //    isGameStart = true;
        //});
        EventCenter.AddListener(EventDefine.Acted, () =>
        {
            isBeforeAction = true;
            isBeforeDraw = false;
            //isRoundStart = false;
        });
        EventCenter.AddListener(EventDefine.DrawCardFinish, () =>
        {
            isBeforeDraw = true;
            //isRoundStart = false;
        });
        EventCenter.AddListener(EventDefine.MaintenanceFinish, () =>
        {
            isBeforeMaintenance = true;
            //isRoundStart = false;
        });
        EventCenter.AddListener(EventDefine.DiscardFinish, () =>
        {
            Debug.Log("Discard Finish");
            isBeforeDiscard = true;
            //isRoundStart = false;
        });

        maskButton.onClick.AddListener(StopCardActing);
        btn_ie.onClick.AddListener(ActingIE);
        btn_se.onClick.AddListener(ActingSE);
        btn_ds.onClick.AddListener(ActingDS);
        btn_show.onClick.AddListener(ActingShow);
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
        PlayerDrawNum = new int[playerNum];
        for (int i = 0; i < playerNum; i++)
        {
            PlayerDrawNum[i] = 1;
        }
        //PlayerStart = Random.Range(0, playerNum);
        PlayerStart = 0;
    }
    #region Crouiper
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
        int randStartPlayer = UnityEngine.Random.Range(0, playerNum);
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
            int rand = UnityEngine.Random.Range(0, cardNum);
            Card temp = cardPile[i];
            cardPile[i] = cardPile[rand];
            cardPile[rand] = temp;
        }
    }
    private void DrawCard(int player, int num)
    {
        cardPileText.text = (cardPile.Count - num).ToString();
        switch (player)
        {
            case 0:
                EventCenter.Broadcast(EventDefine.DrawCard0, num);
                Debug.Log("player 0 draw card");
                break;
            case 1:
                EventCenter.Broadcast(EventDefine.DrawCard1, num);
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
    #endregion
    #region Card Acting
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
        EventCenter.Broadcast(EventDefine.Acted);
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
        //for (int i = 0; i < disCardPile.Count; i++)
        //{
        //    Debug.Log("disCardPile: " + disCardPile[i].Id);
        //}
    }
    private void ActingShow()
    {
        StopCardActing();
        Debug.Log("Acting Show");
        EventCenter.Broadcast(EventDefine.CardShow, actingCard.GetComponent<CardUI>().CardID);
    }
    #endregion
    #region Patent
    private void PatentShow(GameObject go)
    {
        go.GetComponent<PatentUI>().PatentInfoShow();
    }
    private void EndPatentShow(GameObject go)
    {
        go.GetComponent<PatentUI>().EndPatentInfoShow();
    }
    #endregion
    #region Game Process
    private void RoundInit()
    {

    }
    #endregion
}
