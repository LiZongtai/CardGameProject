using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    //public
    public int PatentNum;
    public int Player;
    public int CardNum;
    public GameObject cardPre;
    public GameObject patentPre;

    //Int
    public int sciId;
    private int sciLimit;
    private int sciSubject;
    private int discardCount = 0;
    private int discardNum = 0;
    private int showcardCount = 0;
    private int showcardNum = 0;
    private int PlayerInf;
    private int PlayerDef;

    //Bool
    private bool isIniting = true;
    private bool isBeingInf = false;
    private bool isInf = false;

    //GameObject
    private GameObject canvas;
    private GameObject Sci;
    private GameObject userInfoBox;
    private GameObject cards;
    private GameObject cardsPoint;
    private GameObject patents;

    //Card Action
    private Image cardShowMask;

    //Scientist Box
    private Image sciIcon;
    private Text sciNameText;
    private Text userNameText;
    private Text sciSkillText;
    private Image SciPanel;
    private Image circle;
    private Image subject;
    private Image subjectMask;

    //Button
    private Button lockon;

    //Text
    private Text rankText;
    private Text vicRateText;
    private Text vicNumText;
    private Text rankNumText;
    private Text limitText;

    //List
    //public static List<Card> CardsList = new List<Card>();
    //private List<GameObject> CardsObjectsLists = new List<GameObject>();
    private List<Card> CardsList;
    private List<GameObject> CardsObjectsLists;
    private List<Patent> PatentsList = new List<Patent>();
    private List<GameObject> PatentsObjectsList = new List<GameObject>();

    //private Vector3 cardsPoint;
    private Vector3 patentsPoint;

    private void Awake()
    {
        Init();
        ListenerInit();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
    #region Init
    private void Init()
    {
        
        isIniting = true;
        PatentNum = 0;
        Sci = transform.Find("Sci").gameObject;
        sciNameText = transform.Find("Sci/SciNameText").GetComponent<Text>();
        sciIcon = transform.Find("Sci/SciIcon").GetComponent<Image>();
        SciPanel = transform.Find("Sci/SciPanel").GetComponent<Image>();
        userNameText = transform.Find("Sci/UserNameText").GetComponent<Text>();
        sciSkillText = transform.Find("Sci/SciSkillText").GetComponent<Text>();
        circle = transform.Find("Sci/Circle").GetComponent<Image>();
        subjectMask = transform.Find("Sci/Circle/SubjectMask").GetComponent<Image>();
        subject = transform.Find("Sci/Circle/SubjectMask/Subject").GetComponent<Image>();
        userInfoBox = transform.Find("Sci/UserInfoBox").gameObject;
        rankText = transform.Find("Sci/UserInfoBox/Rank/RankText").GetComponent<Text>();
        vicRateText = transform.Find("Sci/UserInfoBox/VicRate/VicRateText").GetComponent<Text>();
        vicNumText = transform.Find("Sci/UserInfoBox/VicNum/VicNumText").GetComponent<Text>();
        rankNumText = transform.Find("Sci/UserInfoBox/RankNum/RankNumText").GetComponent<Text>();
        limitText = transform.Find("Sci/LimitText").GetComponent<Text>();
        cards = transform.Find("Cards").gameObject;
        cardsPoint = transform.Find("Cards/CardsPoint").gameObject;
        patents = transform.Find("Patents").gameObject;
        patentsPoint = transform.Find("Patents/PatentsPoint").localPosition;
        lockon = transform.Find("LockOn").GetComponent<Button>();
        lockon.gameObject.SetActive(false);
        canvas = GameObject.Find("Canvas");
        cardShowMask = canvas.transform.Find("CardShowMask").GetComponent<Image>();

        CardsList = cards.GetComponent<CardManager>().CardsList;
        CardsObjectsLists = cards.GetComponent<CardManager>().CardsObjectsLists;

        userInfoBox.SetActive(false);
        sciIcon.DOFade(0, 0);
        sciNameText.DOFade(0, 0);
        sciSkillText.DOFade(0, 0);
        userNameText.DOFade(0, 0);
        subject.DOFade(0, 0);
        subjectMask.DOFade(0, 0);
        circle.DOFade(0, 0);
        sciIcon.gameObject.SetActive(false);
        ScientistManager.GetSciData();
        //EventCenter.AddListener<int>(EventDefine.SciSelected, ShowSelf);
        //EventCenter.AddListener<int>(EventDefine.CardDiscard, HandCardsDis);
        //Debug.Log(GetSciName(Random.Range(0, 6)));
        //EventCenter.AddListener<GameObject>(EventDefine.CardShowing, HandCardsShow);
        //EventCenter.AddListener<GameObject>(EventDefine.CardEndShowing, HandCardsEndShow);
        //EventCenter.AddListener<int, int>(EventDefine.Card2Patent, Card2Patent);

    }
    /// <summary>
    /// Add Listener
    /// </summary>
    private void ListenerInit()
    {
        EventCenter.AddListener<int, int>(EventDefine.CardDraw, DrawCards);
        EventCenter.AddListener<int>(EventDefine.InitHandCard, InitHandCards);
        //EventCenter.AddListener<int,int>(EventDefine.EnemySci, SciSet);
        EventCenter.AddListener<int>(EventDefine.SciSelected,ShowSelf);
        EventCenter.AddListener<int, int, int>(EventDefine.Card2Patent, Card2Patent);
        //Button
        lockon.onClick.AddListener(() =>
        {
            EventCenter.Broadcast(EventDefine.InfiltrationRes, Player,0);
        });

        //GameProcess
        EventCenter.AddListener(EventDefine.GameStart, () =>
        {
            InitHandCards(Player);
        });
        EventCenter.AddListener<int>(EventDefine.s2_BeforeRound, BeforeRound);
        EventCenter.AddListener<int>(EventDefine.s3_BeforeAction, BeforeAction);
        EventCenter.AddListener<int>(EventDefine.s4_AfterAction, AfterAction);
        EventCenter.AddListener<int>(EventDefine.s5_BeforeDraw, BeforeDraw);
        EventCenter.AddListener<int>(EventDefine.s6_BeforeDiscard, ResearchDiscard);
        EventCenter.AddListener<int>(EventDefine.s7_AfterDiscard, AfterDiscard);
        EventCenter.AddListener<int>(EventDefine.s8_BeforeMaintenance, Maintenance);
        EventCenter.AddListener(EventDefine.CardDiscardFinish, () =>
        {
            ++discardCount;
            EventCenter.Broadcast<string>(EventDefine.Hint, "弃置" + (discardNum - discardCount).ToString() + "张牌");
            //isDiscarding = true;
        });

        //CardAction
        EventCenter.AddListener<int>(EventDefine.Infiltration, Infiltration);
        EventCenter.AddListener<int,int>(EventDefine.InfiltrationRes, InfiltrationRes);
        EventCenter.AddListener<int, int>(EventDefine.InfiltrationFinish, InfiltrationFinish);
    }
    //public void SciSet(int player, int id)
    //{
    //    if (player != Player)
    //    {
    //        return;
    //    }
    //    sciId = id;
    //}
    public void ShowSelf(int id)
    {
        do
        {
            sciId = (Player - 1) * 2 + Random.Range(0, 1);
        } while (sciId==id);
        ShowSci();
        //InitHandCards();
    }
    #endregion
    #region Game Process
    /// <summary>
    /// 回合开始前
    /// </summary>
    /// <param name="player"></param>
    private void BeforeRound(int player)
    {
        if (player != Player)
        {
            return;
        }
        CardNum = CardsList.Count;
    }
    /// <summary>
    /// 行动阶段前
    /// </summary>
    /// <param name="player"></param>
    private void BeforeAction(int player)
    {
        if (player != Player)
        {
            return;
        }
        StartCoroutine(ActionDelay(1f));
    }
    private IEnumerator ActionDelay(float time)
    {
        yield return new WaitForSeconds(time);
        //渗透
        Card infCard = SearchInfiltrationCard();
        Card funCard = SearchFunCard();
        if (infCard != null)
        {
            isInf = true;
            EventCenter.Broadcast(EventDefine.IEaction, Player, infCard.Id, infCard.Subject, infCard.Type);
            //CardResponse(infCard,0);
        }
        //申请专利
        else if (PatentNum == 0 && funCard != null)
        {

            EventCenter.Broadcast(EventDefine.Card2Patent, Player, funCard.Id, funCard.Subject);
        }
        //随机弃牌
        else if (CardNum != 0)
        {
            int randDis = Random.Range(0, CardNum);
            Card card = CardsList[randDis];
            Debug.Log("Player " + Player + " discard " + card.Id);
            HandCardsDis(card.Id);
        }
        else if (CardNum == 0)
        {
            EventCenter.Broadcast(EventDefine.Acted);
        }
    }
    /// <summary>
    /// 行动阶段后
    /// </summary>
    /// <param name="player"></param>
    private void AfterAction(int player)
    {
        if (player != Player)
        {
            return;
        }
        CardNum = CardsList.Count;
    }
    /// <summary>
    /// 研究阶段摸牌至手牌上限
    /// </summary>
    /// <param name="player"></param>
    private void BeforeDraw(int player)
    {
        if (player != Player)
        {
            return;
        }
        int drawNum = sciLimit - CardNum;
        if (drawNum <= 0)
        {
            GameManager.SetDrawNum(Player, 0);
        }
        else
        {
            Debug.Log("Player " + Player + " Draw Card Num: " + drawNum);
            GameManager.SetDrawNum(Player, drawNum);
        }
        CardNum = CardsList.Count;
    }
    /// <summary>
    /// 研究阶段弃牌
    /// </summary>
    /// <param name="player"></param>
    private void ResearchDiscard(int player)
    {
        if (player != Player)
        {
            return;
        }
        int discardNum = CardNum - sciLimit;
        if (sciLimit - CardNum >= 0)
        {
            discardNum = 0;
            EventCenter.Broadcast(EventDefine.DiscardFinish);
            EventCenter.Broadcast<string>(EventDefine.Hint, "研究阶段结束");
        }
        else
        {
            StartCoroutine(WaitForDiscard());
            for(int i = 0; i < discardNum; i++)
            {
                Card card = CardsList[Random.Range(0, CardNum)];
                HandCardsDis(card.Id);
            }
        }
    }
    private IEnumerator WaitForDiscard()
    {
        EventCenter.Broadcast<string>(EventDefine.Hint, "弃置" + (discardNum).ToString() + "张牌");
        while (discardCount != discardNum)
        {
            Debug.Log("is Waiting for discard");
            yield return null;
        }
        discardCount = 0;
        discardNum = 0;
        EventCenter.Broadcast(EventDefine.DiscardFinish);
        EventCenter.Broadcast<string>(EventDefine.Hint, "研究阶段结束");
        yield return null;
    }
    /// <summary>
    /// 弃牌后
    /// </summary>
    /// <param name="player"></param>
    private void AfterDiscard(int player)
    {
        if (player != Player)
        {
            return;
        }
        CardNum = CardsList.Count;
    }
    /// <summary>
    /// 维护阶段
    /// </summary>
    /// <param name="player"></param>
    private void Maintenance(int player)
    {
        if (player != Player)
        {
            return;
        }
        EventCenter.Broadcast(EventDefine.MaintenanceFinish);
        Debug.Log("Player" + Player + " CardNum: " + CardNum);
    }

    #endregion
    #region Card Action
    /// <summary>
    /// 场上打出渗透牌，Lock On
    /// </summary>
    /// <param name="playerInf"></param>
    private void Infiltration(int playerInf)
    {
        lockon.interactable = false;
        if (playerInf != Player)
        {
            lockon.gameObject.SetActive(true);
            Tweener tweener = lockon.transform.DOScale(0.15f, 0.2f);
            tweener.OnComplete(() =>
            {
                //玩家打出渗透，lockon可交互
                if (playerInf == 0)
                {
                    lockon.interactable = true;
                }
            });
        }
        // AI initiate Infiltration
        else
        {
            lockon.gameObject.SetActive(false);
            Tweener tweener = lockon.transform.DOScale(0.15f, 0.2f);
            PlayerDef = 0;
            do
            {
                PlayerDef = Random.Range(0, GameManager.playerNum);
            } while (PlayerDef == Player);
            tweener.OnComplete(() =>
            {
                //AI打出渗透
                
                EventCenter.Broadcast(EventDefine.InfiltrationRes, PlayerDef, Player);
            });
        }
    }
    /// <summary>
    /// 被渗透响应
    /// </summary>
    /// <param name="playerDef">被渗透玩家</param>
    /// <param name="playerInf">渗透玩家</param>
    private void InfiltrationRes(int playerDef,int playerInf)
    {
        if (playerDef != Player)
        {
            lockon.interactable = false;
            Tweener tweener = lockon.transform.DOScale(0, 0.2f);
            tweener.OnComplete(() =>
            {
                lockon.gameObject.SetActive(false);
            });
            return;
        }
        else
        {
            EventCenter.Broadcast<string>(EventDefine.Hint, "玩家" + Player + "遭到渗透");
            PlayerInf = playerInf;
            isBeingInf = true;
            lockon.interactable = false;
            Card defCard = SearchDefenseCard();
            //Can't Defence
            if (defCard == null)
            {
                Debug.Log("Can't Defence");
                Card patentCard = SearchRandomCard();
                EventCenter.Broadcast(EventDefine.Card2Patent, Player, patentCard.Id, patentCard.Subject);
            }
            //Have Defence Card
            else
            {
                StartCoroutine(WaitForDefense(defCard));
                Debug.Log("Have Defence Card"); 
                //CardResponse(defCard);
                //CardShow(defCard.Id);
            }

        }
    }
    private IEnumerator WaitForDefense(Card defCard)
    {
        yield return new WaitForSeconds(2.6f);
        //InfiltrationFinish();
        EventCenter.Broadcast(EventDefine.IEaction, Player, defCard.Id, defCard.Subject, defCard.Type);
    }
    
    /// <summary>
    /// 被渗透响应结束
    /// </summary>
    private void InfiltrationFinish(int playerRef,int playerInf)
    {

        Tweener tweener = lockon.transform.DOScale(0, 0.2f);
        tweener.OnComplete(() =>
        {
            if (isBeingInf)
            {
                isBeingInf = false;
            }
            if (isInf)
            {
                isInf = false;
            }
        });
    }
    /// <summary>
    /// 寻找缓冲牌
    /// </summary>
    /// <returns></returns>
    private Card SearchDefenseCard()
    {
        for (int i = 0; i < CardNum; i++)
        {
            if (CardsList[i].Type == 1)
            {
                return CardsList[i];
            }
        }
        return null;
    }
    /// <summary>
    /// 寻找渗透牌
    /// </summary>
    /// <returns></returns>
    private Card SearchInfiltrationCard()
    {
        for (int i = 0; i < CardNum; i++)
        {
            if (CardsList[i].Type == 0)
            {
                return CardsList[i];
            }
        }
        return null;
    }
    private Card SearchFunCard()
    {
        for (int i = 0; i < CardNum; i++)
        {
            if (CardsList[i].Type != 0 && CardsList[i].Type != 1)
            {
                return CardsList[i];
            }
        }
        return null;
    }
    /// <summary>
    /// 寻找随机牌
    /// </summary>
    /// <returns></returns>
    private Card SearchRandomCard()
    {
        if (CardNum == 0)
        {
            return null;
        }
        else
        {
            return CardsList[Random.Range(0, CardNum)];
        }
    }
    
    #endregion
    #region Scientist
    private void ShowSci()
    {
        userNameText.text = "Player " + Player;
        sciSubject = ScientistManager.GetSciSubject(sciId);
        userNameText.text = Models.GameModel.userDto.UserName;
        sciIcon.gameObject.SetActive(true);
        sciIcon.sprite = ResourcesManager.GetSciSprite(ScientistManager.GetSciIcon(sciId));
        SciPanel.sprite = ResourcesManager.GetPanelSprite("Panel_" + ScientistManager.GetSciSubject(sciId));
        subject.sprite = ResourcesManager.GetSubjectSprite("Subject_" + ScientistManager.GetSciSubject(sciId));
        subjectMask.color = GameModel.Subject2Color(ScientistManager.GetSciSubject(sciId));
        sciNameText.text = ScientistManager.GetSciName(sciId);
        sciSkillText.text = ScientistManager.GetSciSkill(sciId);
        sciLimit = ScientistManager.GetSciLimit(sciId);
        limitText.text = sciLimit.ToString();
        Tweener tweener = sciIcon.DOFade(1, 3);
        sciNameText.DOFade(1, 3);
        sciSkillText.DOFade(1, 3);
        userNameText.DOFade(1, 3);
        circle.DOFade(1, 3);
        subjectMask.DOFade(1, 3);
        subject.DOFade(1, 3);
        circle.transform.DOLocalRotate(new Vector3(0, 360, 0), 20f, RotateMode.WorldAxisAdd).SetLoops(10);
        tweener.OnComplete(() =>
        {
            isIniting = false;
        });
    }
    #endregion
    #region Hand Cards
    //private void InitHandCards()
    //{
    //    discardCount = 0;
    //    showcardCount = 0;
    //    for (int i = CardNum; i < CardNum; i++)
    //    {
    //        DrawHandCards(CardsList[i].Id);
    //    }
    //}
    private void InitHandCards(int player)
    {
        if (player != Player)
        {
            return;
        }
        discardCount = 0;
        showcardCount = 0;
        for (int i = CardsObjectsLists.Count; i < CardNum; i++)
        {
            DrawHandCards(CardsList[i].Id);
        }
    }
    private void DrawCards(int player, int num)
    {
        if (player != Player)
        {
            return;
        }
        for (int i = 0; i < num; i++)
        {
            Card card = GameManager.cardPile[0];
            CardsList.Add(card);
            CardNum = CardsList.Count;
            GameManager.SetCardPile(card);
        }
        if (isIniting == false)
        {
            InitHandCards(Player);
        }

    }
    private void DrawHandCards(int id)
    {
        GameObject cardGO = Instantiate(cardPre, cards.transform, true);
        cardGO.transform.localScale = new Vector3(1,0.7f, 0.7f);
        cardGO.transform.Rotate(0, -90, 0);
        cardGO.transform.name = "Card_" + id;
        cardGO.GetComponent<CardUI>().GetCardId(id);
        cardGO.GetComponent<CardUI>().CardInit();
        cardGO.GetComponent<CardUI>().isBack = true;
        cardGO.GetComponent<CardUI>().isShowing = true;
        cardGO.transform.localPosition = new Vector2(cardsPoint.transform.localPosition.x + 300, cardsPoint.transform.localPosition.y - 100);
        Tweener tweener = cardGO.transform.DOLocalMove(new Vector2(cardsPoint.transform.localPosition.x, cardsPoint.transform.localPosition.y), 0.5f);
        tweener.OnComplete(() =>
        {
            CardNum = CardsList.Count;
            CardsObjectsLists.Add(cardGO);
            HandCardsShuffle();

        });
    }
    private void HandCardsShuffle()
    {
        if (CardsObjectsLists.Count == 0)
        {
            EventCenter.Broadcast(EventDefine.DrawCardFinish);
            return;
        }
        int count = 0;
        for (int i = 0; i < CardsObjectsLists.Count; i++)
        {
            Tweener tweener = CardsObjectsLists[i].transform.DOLocalMove(new Vector2(cardsPoint.transform.localPosition.x , cardsPoint.transform.localPosition.y - i * 300 / CardsObjectsLists.Count), 0.3f);
            tweener.OnComplete(() =>
            {
                ++count;
                if (count == CardsObjectsLists.Count)
                {
                    EventCenter.Broadcast(EventDefine.DrawCardFinish);
                }
            });
        }
    }

    public void HandCardsDis(int id)
    {
        int ii = Id2CardIndex(id);
        Card card = CardsList[ii];
        GameObject go = CardsObjectsLists[ii];
        go.transform.DOLocalMove(new Vector2(go.transform.localPosition.x, go.transform.localPosition.y -500), 0.1f);
        Tweener tweener = go.transform.DOScale(0.2f, 0.1f);
        tweener.OnComplete(() =>
        {
            if (GameManager.isBeforeDiscard)
            {
                Debug.Log("isBeforeDiscard");
                EventCenter.Broadcast(EventDefine.CardDiscardFinish);
            }
            else if (GameManager.isBeforeRound)
            {
                Debug.Log("isBeforeRound");
                EventCenter.Broadcast(EventDefine.Acted);
            }
            else
            {
                Debug.Log("else");
            }
            CardsObjectsLists.Remove(go);
            Destroy(go);
            GameManager.SetDisPile(card);
            CardsList.Remove(card);
            CardNum = CardsList.Count;
        });
    }
    private int Id2CardIndex(int id)
    {
        for (int i = 0; i < CardsList.Count; i++)
        {
            if (CardsList[i].Id == id)
            {
                return i;
            }
        }
        Debug.Log("Can't find this card in hands");
        return -1;
    }
    #endregion
    #region Patents
    private void AddPatent(int id, int subject)
    {
        ++PatentNum;
        Patent patent = new Patent(id, subject, true);
        for (int i = 0; i < CardsList.Count; i++)
        {
            if (CardsList[i].Id == id)
            {

                GameObject go = CardsObjectsLists[i];
                Tweener tweener = go.transform.DOScale(0f, 0.3f);
                Card cd = CardsList[i];
                tweener.OnComplete(() =>
                {
                    CardsList.Remove(cd);
                    CardsObjectsLists.Remove(go);
                    Destroy(go);
                    ShowPatent(id);
                    PatentsList.Add(patent);
                    GameManager.patentAll.Add(patent);
                    CardNum = CardsList.Count;
                    EventCenter.Broadcast(EventDefine.StopCardActing);
                    return;
                });
            }
        }
        return;

    }
    private void ShowPatent(int id)
    {
        EventCenter.Broadcast<string>(EventDefine.Hint, "玩家" + Player + "申请私有专利");
        //GameManager.StopCardActing();
        GameObject patentGO = Instantiate(patentPre, patents.transform, true);
        patentGO.transform.localScale = new Vector2(0, 0);
        patentGO.transform.name = "Patent_" + id;
        patentGO.GetComponent<PatentUI>().GetPatentId(id);
        patentGO.GetComponent<PatentUI>().PatentInit();
        //patentGO.transform.localPosition = patentsPoint;
        patentGO.transform.localPosition = new Vector2(patentsPoint.x, patentsPoint.y - (PatentNum - 1) * 120);
        Tweener tweener = patentGO.transform.DOScale(0.2f, 0.3f);
        tweener.OnComplete(() =>
        {
            PatentsObjectsList.Add(patentGO);
            GameManager.patentObjectAll.Add(patentGO);
            patentGO.GetComponent<PatentUI>().PatentInfoPosition();
            if (!isBeingInf)
            {
                EventCenter.Broadcast(EventDefine.Acted);
            }
            else if (isBeingInf)
            {
                Debug.Log("EventDefine.DefenseFinish " + Player + " " + PlayerInf);
                EventCenter.Broadcast(EventDefine.InfiltrationFinish, PlayerDef, PlayerInf);
            }
        });
    }
    private void Card2Patent(int player, int id, int subject)
    {
        if (player != Player)
        {
            return;
        }
        AddPatent(id, subject);
        if (isBeingInf)
        {
            EventCenter.Broadcast(EventDefine.Defense, Player);
        }
    }
    #endregion
}
