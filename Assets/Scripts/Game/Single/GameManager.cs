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
    public static bool isRoundStart = true;
    public static bool isPlayerRoundStart = true;
    public static bool isBeforeRound = true;
    public static bool isBeforeAction = true;
    public static bool isAfterAction = true;
    public static bool isBeforeDraw = true;
    public static bool isBeforeDiscard = true;
    public static bool isAfterDiscard = true;
    public static bool isBeforeMaintenance = true;
    public static bool isPlayerRoundEnd = true;
    public static bool isRoundEnd = true;
    public static bool isGameEnd = true;
    private int RoundNum=0;
    private int PlayerStart;

    //GameObject
    private GameObject player0;
    private GameObject player1;
    private GameObject player2;
    private GameObject player3;
    private GameObject showPanel;
    private GameObject cardActing;
    private GameObject sciSelect;
    private GameObject cardISActing;
    private GameObject actingCard;
    private GameObject gamePanel;

    //Text
    private Text cardPileText;
    private static Text discardPileText;

    //Image
    private Image maskBG;

    //Int
    private int playerNum=0;
    private int cardNum = 0;
    private int CurPlayer;

    //List
    private static int[] PlayerDrawNum;
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
    private Button test1;

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
        if (isGameStart == true)
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
        //isGameStart = true;
        yield return StartCoroutine(GameStart());
        yield return StartCoroutine(RoundLoop());
    }
    /// <summary>
    /// 回合循环
    /// </summary>
    /// <returns></returns>
    private IEnumerator RoundLoop()
    {
        yield return StartCoroutine(s0_RoundStart());
        for (int i = PlayerStart; i < PlayerStart+playerNum; ++i)
        {
            CurPlayer = i % playerNum;
            yield return StartCoroutine(s1_PlayerRoundStart());
            yield return StartCoroutine(s2_BeforeRound());
            yield return StartCoroutine(s3_BeforeAction());
            yield return StartCoroutine(s4_AfterAction());
            yield return StartCoroutine(s5_BeforeDraw());
            yield return StartCoroutine(s6_BeforeDiscard());
            yield return StartCoroutine(s7_AfterDiscard());
            yield return StartCoroutine(s8_BeforeMaintenance());
            yield return StartCoroutine(s9_PlayerRoundEnd());
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
            yield return StartCoroutine(s10_RoundEnd());
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
    private IEnumerator s0_RoundStart()
    {
        isRoundStart = true;
        EventCenter.Broadcast<string>(EventDefine.Hint, "Round " + RoundNum + " Start");
        Debug.Log("Round " + RoundNum + " Start");
        yield return new WaitForSeconds(2f);
    }
    /// <summary>
    /// 玩家回合开始
    /// </summary>
    /// <returns></returns>
    private IEnumerator s1_PlayerRoundStart()
    {
        Debug.Log("Player " + CurPlayer);
   
        isPlayerRoundStart = true;
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
    private IEnumerator s2_BeforeRound()
    {
        isBeforeRound = true;
        EventCenter.Broadcast(EventDefine.s2_BeforeRound, CurPlayer);
        yield return null;
    }
    /// <summary>
    /// 行动阶段开始
    /// </summary>
    /// <returns></returns>
    private IEnumerator s3_BeforeAction()
    {
        isBeforeAction = true;
        EventCenter.Broadcast<string>(EventDefine.Hint, "行动阶段");
        Debug.Log("Player " + CurPlayer + " is Acting");
        EventCenter.Broadcast(EventDefine.s3_BeforeAction, CurPlayer);
        while (isBeforeAction)
        {
            Debug.Log("is Acting");
            yield return null;
        }
        
    }
    /// <summary>
    /// 行动阶段结束
    /// </summary>
    /// <returns></returns>
    private IEnumerator s4_AfterAction()
    {
        isAfterAction = true;
        EventCenter.Broadcast(EventDefine.s4_AfterAction, CurPlayer);
        Debug.Log("Player " + CurPlayer + " After Action");
        yield return null;
    }
    /// <summary>
    /// 研究阶段开始
    /// </summary>
    /// <returns></returns>
    private IEnumerator s5_BeforeDraw()
    {
        isBeforeDraw = true;
        EventCenter.Broadcast(EventDefine.s5_BeforeDraw,CurPlayer);
        EventCenter.Broadcast<string>(EventDefine.Hint, "研究阶段");
        Debug.Log("研究阶段 " + CurPlayer + " " + PlayerDrawNum[CurPlayer]);
        EventCenter.Broadcast(EventDefine.CardDraw, CurPlayer, PlayerDrawNum[CurPlayer]);
        //switch (CurPlayer)
        //{
        //    case 0:
        //        EventCenter.Broadcast(EventDefine.DrawCard0, PlayerDrawNum[0]);
        //        Debug.Log("player 0 draw card");
        //        break;
        //    case 1:
        //        EventCenter.Broadcast(EventDefine.DrawCard1, PlayerDrawNum[1]);
        //        Debug.Log("player 1 draw card");
        //        break;
        //    case 2:
        //        EventCenter.Broadcast(EventDefine.DrawCard2, PlayerDrawNum[2]);
        //        Debug.Log("player 2 draw card");
        //        break;
        //    case 3:
        //        EventCenter.Broadcast(EventDefine.DrawCard3, PlayerDrawNum[3]);
        //        Debug.Log("player 3 draw card");
        //        break;
        //}
        while (isBeforeDraw)
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
    private IEnumerator s6_BeforeDiscard()
    {
        isBeforeDiscard = true;
        EventCenter.Broadcast(EventDefine.s6_BeforeDiscard,CurPlayer);
        while (isBeforeDiscard)
        {
            Debug.Log("is Discarding");
            yield return null;
        }
    }
    /// <summary>
    /// 研究阶段结束
    /// </summary>
    /// <returns></returns>
    private IEnumerator s7_AfterDiscard()
    {
        EventCenter.Broadcast(EventDefine.s7_AfterDiscard, CurPlayer);
        isAfterDiscard = true;
        yield return null;
    }
    /// <summary>
    /// 维护阶段开始
    /// </summary>
    /// <returns></returns>
    private IEnumerator s8_BeforeMaintenance()
    {
        isBeforeMaintenance = true;
        EventCenter.Broadcast<string>(EventDefine.Hint, "维护阶段");
        EventCenter.Broadcast(EventDefine.s8_BeforeMaintenance,CurPlayer);
        while (isBeforeMaintenance)
        {
            Debug.Log("Maintenance");
            yield return null;
        }

    }
    /// <summary>
    /// 玩家回合结束
    /// </summary>
    /// <returns></returns>
    private IEnumerator s9_PlayerRoundEnd()
    {
        isPlayerRoundEnd = true;
        yield return new WaitForSeconds(2f);
    }
    /// <summary>
    /// 回合结束
    /// </summary>
    /// <returns></returns>
    private IEnumerator s10_RoundEnd()
    {
        isRoundEnd = true;
        ++RoundNum;
        yield return new WaitForSeconds(2f);
    }
    #endregion
    private void Init()
    {
        showPanel = transform.Find("ShowPanel").gameObject;
        sciSelect = transform.Find("ShowPanel/SciSelect").gameObject;
        cardActing=transform.Find("ShowPanel/CardActing").gameObject;
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
            SetFalse();
        });
        EventCenter.AddListener(EventDefine.DrawCardFinish, () =>
        {
            SetFalse();
        });
        EventCenter.AddListener(EventDefine.DiscardFinish, () =>
        {
            SetFalse();
        });
        EventCenter.AddListener(EventDefine.MaintenanceFinish, () =>
        {
            SetFalse();
        });

        //maskButton.onClick.AddListener(StopCardActing);

        btn_draw.onClick.AddListener(() =>
        {
            DrawCard(0, 1);
        });

        test1 = transform.Find("test1").GetComponent<Button>();
        test1.onClick.AddListener(() =>
        {
            EventCenter.Broadcast(EventDefine.CardSE, 51);
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
    public static void SetDisPile(Card card)
    {
        disCardPile.Add(card);
        discardPileText.text = disCardPile.Count.ToString();
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
                    EventCenter.Broadcast(EventDefine.CardDraw, 0 , 3);
                    break;
                case 1:
                    EventCenter.Broadcast(EventDefine.CardDraw, 1, 3);
                    Debug.Log("player 1 draw card");
                    break;
                case 2:
                    EventCenter.Broadcast(EventDefine.CardDraw, 2, 3);
                    Debug.Log("player 2 draw card");
                    break;
                case 3:
                    EventCenter.Broadcast(EventDefine.CardDraw, 3, 3);
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
    public static void SetDrawNum(int player,int num)
    {
        PlayerDrawNum[player] = num;
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
    private void SetFalse()
    {
        isGameStart = false;
        isRoundStart = false;
        isPlayerRoundStart = false;
        isBeforeRound = false;
        isBeforeAction = false;
        isAfterAction = false;
        isBeforeDraw = false;
        isBeforeDiscard = false;
        isAfterDiscard = false;
        isBeforeMaintenance = false;
        isPlayerRoundEnd = false;
        isRoundEnd = false;
        isGameEnd = false;
    }
    private void RoundInit()
    {

    }
    #endregion
}
