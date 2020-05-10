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

    //Bool
    private bool isIniting = true;
    private bool isBeingInf = false;

    //GameObject
    private GameObject Sci;
    private GameObject userInfoBox;
    //public GameObject cardPre;
    //private GameObject cards;
    private GameObject patents;
    


    //Scientist Box
    private Image SciIcon;
    private Text SciNameText;
    private Text userNameText;
    private Text SciSkillText;
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
    private List<Card> CardsList = new List<Card>();
    //private List<GameObject> CardsObjectsLists = new List<GameObject>();
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
        SciNameText = transform.Find("Sci/SciNameText").GetComponent<Text>();
        SciIcon = transform.Find("Sci/SciIcon").GetComponent<Image>();
        SciPanel = transform.Find("Sci/SciPanel").GetComponent<Image>();
        userNameText = transform.Find("Sci/UserNameText").GetComponent<Text>();
        SciSkillText = transform.Find("Sci/SciSkillText").GetComponent<Text>();
        circle = transform.Find("Sci/Circle").GetComponent<Image>();
        subjectMask = transform.Find("Sci/Circle/SubjectMask").GetComponent<Image>();
        subject = transform.Find("Sci/Circle/SubjectMask/Subject").GetComponent<Image>();
        userInfoBox = transform.Find("Sci/UserInfoBox").gameObject;
        rankText = transform.Find("Sci/UserInfoBox/Rank/RankText").GetComponent<Text>();
        vicRateText = transform.Find("Sci/UserInfoBox/VicRate/VicRateText").GetComponent<Text>();
        vicNumText = transform.Find("Sci/UserInfoBox/VicNum/VicNumText").GetComponent<Text>();
        rankNumText = transform.Find("Sci/UserInfoBox/RankNum/RankNumText").GetComponent<Text>();
        limitText = transform.Find("Sci/LimitText").GetComponent<Text>();
        //cards = transform.Find("Cards").gameObject;
        //cardsPoint = transform.Find("Cards/CardsPoint").localPosition;
        patents = transform.Find("Patents").gameObject;
        patentsPoint = transform.Find("Patents/PatentsPoint").localPosition;
        lockon = transform.Find("LockOn").GetComponent<Button>();
        lockon.gameObject.SetActive(false);

        userInfoBox.SetActive(false);
        SciIcon.DOFade(0, 0);
        SciNameText.DOFade(0, 0);
        SciSkillText.DOFade(0, 0);
        userNameText.DOFade(0, 0);
        subject.DOFade(0, 0);
        subjectMask.DOFade(0, 0);
        circle.DOFade(0, 0);
        SciIcon.gameObject.SetActive(false);
        ScientistManager.GetSciData();
        //EventCenter.AddListener<int>(EventDefine.SciSelected, ShowSelf);
        //EventCenter.AddListener<int>(EventDefine.CardDiscard, HandCardsDis);
        //Debug.Log(GetSciName(Random.Range(0, 6)));
        //EventCenter.AddListener<GameObject>(EventDefine.CardShowing, HandCardsShow);
        //EventCenter.AddListener<GameObject>(EventDefine.CardEndShowing, HandCardsEndShow);
        //EventCenter.AddListener<int, int>(EventDefine.Card2Patent, Card2Patent);
        sciId = UnityEngine.Random.Range(0, 6);
        ShowSelf(sciId);
    }
    private void ListenerInit()
    {
        EventCenter.AddListener<int, int>(EventDefine.CardDraw, DrawCards);

        //Button
        lockon.onClick.AddListener(() =>
        {
            EventCenter.Broadcast(EventDefine.InfiltrationRes, Player);
        });

        //GameProcess
        EventCenter.AddListener<int>(EventDefine.s2_BeforeRound, BeforeRound);
        EventCenter.AddListener<int>(EventDefine.s3_BeforeAction, BeforeAction);
        EventCenter.AddListener<int>(EventDefine.s4_AfterAction, AfterAction);
        EventCenter.AddListener<int>(EventDefine.s5_BeforeDraw, BeforeDraw);
        EventCenter.AddListener<int>(EventDefine.s6_BeforeDiscard, ResearchDiscard);
        EventCenter.AddListener<int>(EventDefine.s7_AfterDiscard, AfterDiscard);
        EventCenter.AddListener<int>(EventDefine.s8_BeforeMaintenance, Maintenance);

        //CardAction
        EventCenter.AddListener(EventDefine.Infiltration, Infiltration);
        EventCenter.AddListener<int>(EventDefine.InfiltrationRes, InfiltrationRes);
    }
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
        int randDis = Random.Range(0, CardNum);
        Card card = CardsList[randDis];
        Debug.Log("Player " + Player + " discard " + card.Id);
        CardsList.Remove(card);
        GameManager.SetDisPile(card);
        EventCenter.Broadcast(EventDefine.Acted);
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
            GameManager.SetDrawNum(0, 0);
            EventCenter.Broadcast(EventDefine.DrawCardFinish);
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
            int randDis = Random.Range(0, CardNum);
            Card card = CardsList[randDis];
            Debug.Log("Player " + Player + " discard " + card.Id);
            CardsList.Remove(card);
            GameManager.SetDisPile(card);
            EventCenter.Broadcast(EventDefine.DiscardFinish);
        }
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
    private void Infiltration()
    {
        for (int i = 0; i < CardNum; i++)
        {
            Debug.Log("Player " + Player + " Card Type: " + CardsList[i].Type);
        }
        lockon.gameObject.SetActive(true);
        lockon.interactable = false;
        Tweener tweener = lockon.transform.DOScale(0.15f, 0.2f);
        tweener.OnComplete(() =>
        {
            lockon.interactable = true;
        });
    }
    private void InfiltrationRes(int player)
    {
        if (player != Player)
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
            isBeingInf = true;
            lockon.interactable = false;
            Card defCard = SearchDefenseCard();
            //Can't Defence
            if (defCard == null)
            {
                Debug.Log("Can't Defence");
                Card patentCard = SearchRandomCard();
                AddPatent(patentCard.Id, patentCard.Subject);
            }
            //Have Defence Card
            else
            {
                Debug.Log("Have Defence Card");
                CardShow(defCard.Id);
            }

        }
    }
    private void InfiltrationFinish()
    {

        Tweener tweener = lockon.transform.DOScale(0, 0.2f);
        tweener.OnComplete(() =>
        {
            isBeingInf = false;
            lockon.gameObject.SetActive(false);
            EventCenter.Broadcast(EventDefine.InfiltrationFinish);
        });
    }
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
    private void ShowSelf(int id)
    {
        ShowSci(id);
        //InitHandCards();
    }
    #endregion
    #region Scientist
    private void ShowSci(int id)
    {
        userNameText.text = "Player " + Player;
        sciId = id;
        sciSubject = ScientistManager.GetSciSubject(sciId);
        userNameText.text = Models.GameModel.userDto.UserName;
        SciIcon.gameObject.SetActive(true);
        SciIcon.sprite = ResourcesManager.GetSciSprite(ScientistManager.GetSciIcon(sciId));
        SciPanel.sprite = ResourcesManager.GetPanelSprite("Panel_" + ScientistManager.GetSciSubject(sciId));
        subject.sprite = ResourcesManager.GetSubjectSprite("Subject_" + ScientistManager.GetSciSubject(sciId));
        subjectMask.color = GameModel.Subject2Color(ScientistManager.GetSciSubject(sciId));
        SciNameText.text = ScientistManager.GetSciName(sciId);
        SciSkillText.text = ScientistManager.GetSciSkill(sciId);
        sciLimit = ScientistManager.GetSciLimit(sciId);
        limitText.text = sciLimit.ToString();
        Tweener tweener = SciIcon.DOFade(1, 3);
        SciNameText.DOFade(1, 3);
        SciSkillText.DOFade(1, 3);
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
    //    for (int i = CardsObjectsLists.Count; i < CardsList.Count; i++)
    //    {
    //        DrawHandCards(CardsList[i].Id);
    //    }
    //}
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
            GameManager.cardPile.RemoveAt(0);
            CardNum = CardsList.Count;
            
        }
        EventCenter.Broadcast(EventDefine.DrawCardFinish);

    }
    private void CardShow(int id)
    {
        int ii = Id2CardIndex(id);
        Card card = CardsList[ii];
        GameObject cardGO = Instantiate(cardPre, gameObject.transform, true);
        cardGO.transform.localScale = new Vector2(0, 0);
        cardGO.transform.name = "Card_" + id;
        cardGO.GetComponent<CardUI>().GetCardId(id);
        cardGO.GetComponent<CardUI>().CardInit();
        cardGO.transform.localPosition = new Vector2(0, 0);
        Tweener tweener = cardGO.transform.DOLocalMove(new Vector2(300, -100), 0.3f);
        cardGO.transform.DOScale(0.7f, 0.3f);
        cardGO.GetComponent<CardUI>().isShowing = true;
        cardGO.GetComponent<CardUI>().ShowEffect(0.3f);
        tweener.OnComplete(() =>
        {
            if (isBeingInf)
            {
                CardsList.Remove(card);
                InfiltrationFinish();
                StartCoroutine(CardShowForDefence(id));
            }
        });
    }
    private IEnumerator CardShowForDefence(int id)
    {
        yield return new WaitForSeconds(2f);
        GameObject cardGO = transform.Find("Card_" + id).gameObject;
        cardGO.GetComponent<CardUI>().EndShowEffect(0.2f);
        cardGO.transform.DOLocalMove(new Vector2(300, -400), 0.2f);
        Tweener tweener = cardGO.transform.DOScale(0, 0.2f);
        tweener.OnComplete(() =>
        {
            Destroy(cardGO);
            CardNum = CardsList.Count;
        });
    }
    public void HandCardsDis(int id)
    {
        CardsList.RemoveAt(Id2CardIndex(id));
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
        PatentNum++;
        Patent patent = new Patent(id, subject, false);
        for (int i = 0; i < CardsList.Count; i++)
        {
            if (CardsList[i].Id == id)
            {

                //GameObject go = CardsObjectsLists[i];
                //Tweener tweener = go.transform.DOScale(0f, 0.3f);
                Card cd = CardsList[i];
                //EventCenter.Broadcast(EventDefine.StopCardActing);
                CardsList.Remove(cd);
                //CardsObjectsLists.Remove(go);
                //Destroy(go);
                ShowPatent(id);
                PatentsList.Add(patent);
                GameManager.patentAll.Add(patent);
            }
        }
        return;

    }
    private void ShowPatent(int id)
    {
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
            if (isBeingInf)
            {
                InfiltrationFinish();
            }
        });
    }
    private void Card2Patent(int id, int subject)
    {
        AddPatent(id, subject);
    }
    #endregion
}
