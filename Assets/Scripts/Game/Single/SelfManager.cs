using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SelfManager : MonoBehaviour
{
    //public
    public int PatentNum;
    public int CardNum;
    //Int
    private int sciId;
    private int sciLimit;
    private int sciSubject;
    private int discardCount = 0;
    private int discardNum = 0;
    private int showcardCount = 0;
    private int showcardNum = 0;


    //Bool
    private bool isIniting = true;
    private bool isDiscarding = false;

    //GameObject
    private GameObject Sci;
    private GameObject userInfoBox;
    public GameObject cardPre;
    private GameObject cards;
    private GameObject cardsPoint;
    private GameObject patents;
    public GameObject patentPre;
    

    //Scientist Box
    private Image sciIcon;
    private Text sciNameText;
    private Text userNameText;
    private Text sciSkillText;
    private Image sciPanel;
    private Image circle;
    private Image subject;
    private Image subjectMask;
    private Text limitText;

    //Text
    private Text rankText;
    private Text vicRateText;
    private Text vicNumText;
    private Text rankNumText;

    //List
    private List<Card> CardsList = new List<Card>();
    private List<GameObject> CardsObjectsLists = new List<GameObject>();
    private List<Patent> PatentsList = new List<Patent>();
    private List<GameObject> PatentsObjectsList = new List<GameObject>();


    private Vector3 patentsPoint;

    //Button

    private Button btn_test;

    private void Awake()
    {
        Init();
        
    }
    private void Start()
    {
        //SciIcon.DOFade(1, 2);
    }
    private void Update()
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
        sciPanel = transform.Find("Sci/SciPanel").GetComponent<Image>();
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
        EventCenter.AddListener<int>(EventDefine.SciSelected, ShowSelf);
        EventCenter.AddListener<int>(EventDefine.CardDiscard, HandCardsDis);
        EventCenter.AddListener(EventDefine.InitHandCard,InitHandCards);
        //Debug.Log(GetSciName(Random.Range(0, 6)));

        EventCenter.AddListener<GameObject>(EventDefine.CardShowing, HandCardsShow);
        EventCenter.AddListener<GameObject>(EventDefine.CardEndShowing, HandCardsEndShow);
        EventCenter.AddListener<int, int>(EventDefine.Card2Patent, Card2Patent);

        //GameProcess
        EventCenter.AddListener<int>(EventDefine.DrawCard0, DrawCards);
        EventCenter.AddListener(EventDefine.BeforeMaintenance, Maintenance);
        EventCenter.AddListener(EventDefine.BeforeDiscard, ResearchDiscard);
        EventCenter.AddListener<int>(EventDefine.CardShow, HandCardsShow);
        EventCenter.AddListener(EventDefine.CardDiscardFinish, () =>
        {
            ++discardCount;
            EventCenter.Broadcast<string>(EventDefine.Hint, "弃置" + (discardNum- discardCount).ToString() + "张牌");
            //isDiscarding = true;
        });
        EventCenter.AddListener(EventDefine.CardShowFinish, () =>
        {
            ++showcardCount;
            EventCenter.Broadcast<string>(EventDefine.Hint, "公开" + (showcardNum - showcardCount).ToString() + "张牌");
            //isDiscarding = true;
        });


        btn_test = transform.Find("btn_test").GetComponent<Button>();
        btn_test.onClick.AddListener(() =>
        {
            for (int i = 0; i < CardsList.Count; i++)
            {
                Debug.Log("handCards: " + CardsObjectsLists[i].GetComponent<CardUI>().CardID);
            }
        });
    }
    private void ShowSelf(int id)
    {
        ShowSci(id);

        //InitHandCards();
    }
    #endregion
    #region Scientist
    private void ShowSci(int id)
    {
        sciId = id;
        sciSubject = ScientistManager.GetSciSubject(sciId);
        userNameText.text = Models.GameModel.userDto.UserName;
        sciIcon.gameObject.SetActive(true);
        sciIcon.sprite = ResourcesManager.GetSciSprite(ScientistManager.GetSciIcon(sciId));
        sciPanel.sprite = ResourcesManager.GetPanelSprite("Panel_" + sciSubject);
        subject.sprite = ResourcesManager.GetSubjectSprite("Subject_" + sciSubject);
        subjectMask.color = GameModel.Subject2Color(sciSubject);
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
        //circle.transform.DOLocalRotate(new Vector3(0, 360, 0), 20f, RotateMode.WorldAxisAdd).SetLoops(10);
        tweener.OnComplete(() =>
        {
            isIniting = false;
        });
    }
    #endregion
    #region Hand Cards
    private void InitHandCards()
    {
        discardCount = 0;
        showcardCount = 0;
        for(int i = CardsObjectsLists.Count; i < CardsList.Count; i++)
        {
            //TODO
            //if (CardsObjectsLists.Count != 0)
            //{
            //    if (CardsObjectsLists[i].GetComponent<CardUI>().isShowing == true)
            //    {
            //        CardsObjectsLists[i].GetComponent<CardUI>().EndShowEffect(0.02f);
            //        CardsObjectsLists[i].GetComponent<CardUI>().isShowing = false;
            //    }
            //}
            DrawHandCards(CardsList[i].Id);
        }
    }
    private void DrawCards(int num)
    {
        for(int i = 0; i < num; i++)
        {
            Card card = GameManager.cardPile[0];
            CardsList.Add(card);
            CardNum = CardsList.Count;
            GameManager.cardPile.RemoveAt(0);
        }
        if (isIniting == false)
        {
            InitHandCards();

        }
    }
    private void DrawHandCards(int id)
    {
        GameObject cardGO = Instantiate(cardPre, cards.transform, true);
        cardGO.transform.localScale = new Vector2(0.85f, 0.85f);
        cardGO.transform.name = "Card_" + id;
        cardGO.GetComponent<CardUI>().GetCardId(id);
        cardGO.GetComponent<CardUI>().CardInit();
        cardGO.transform.localPosition = cardsPoint.transform.localPosition;
        Tweener tweener = cardGO.transform.DOLocalMove(new Vector2(cardsPoint.transform.localPosition.x, cardsPoint.transform.localPosition.y-350), 0.5f);
        tweener.OnComplete(() =>
        {
            CardNum = CardsList.Count;
            CardsObjectsLists.Add(cardGO);
            HandCardsShuffle();
        });
    }
    private void HandCardsShuffle()
    {
        int count = 0;
        for (int i = 0; i < CardsObjectsLists.Count; i++)
        {
            Tweener tweener = CardsObjectsLists[i].transform.DOLocalMove(new Vector2(cardsPoint.transform.localPosition.x + i * 1000 / CardsObjectsLists.Count, cardsPoint.transform.localPosition.y - 350), 0.3f);
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
    private void HandCardsShow(GameObject go)
    {
        //for(int i = 0; i < handCards.Count; i++)
        //{
        //    if (go != handCardsObjects[i].gameObject)
        //    {
        //        Debug.Log("Interact invalid");
        //    }
        //}
        int count=go.transform.parent.childCount - 1;
        go.transform.SetSiblingIndex(count);
        go.transform.DOLocalMoveY(-150, 0.02f);
        go.GetComponent<CardUI>().ShowEffect(0.02f);
    }
    private void HandCardsEndShow(GameObject go)
    {
        int count = 0;
        for (int i = 0; i < CardsObjectsLists.Count; i++)
        {
            if (go == CardsObjectsLists[i].gameObject)
            {
                count = i;
            }
        }
        go.transform.DOLocalMoveY(-350, 0.02f);
        go.transform.SetSiblingIndex(count);
        go.GetComponent<CardUI>().EndShowEffect(0.02f);
    }
    public void HandCardsDis(int id)
    {
        int ii = Id2CardIndex(id);
        GameObject go = CardsObjectsLists[ii];
        go.transform.DOLocalMove(new Vector2(go.transform.localPosition.x - 800, go.transform.localPosition.y + 500),0.1f);
        Tweener tweener = go.transform.DOScale(0.2f, 0.1f);
        tweener.OnComplete(() =>
        {
            EventCenter.Broadcast(EventDefine.CardDiscardFinish);
            Destroy(go);
            CardsObjectsLists.RemoveAt(ii);
            CardsList.RemoveAt(ii);
            CardNum = CardsList.Count;
        });
    }
    public void HandCardsShow(int id)
    {
        int ii = Id2CardIndex(id);
        GameObject go = CardsObjectsLists[ii];
        go.GetComponent<CardUI>().ShowEffect(0.02f);
        go.GetComponent<CardUI>().isShowing = true;
        Tweener tweener = go.transform.DOLocalMoveY(-250, 0.02f);
        tweener.OnComplete(() =>
        {
            EventCenter.Broadcast(EventDefine.CardShowFinish);
        });

    }
    private int Id2CardIndex(int id)
    {
        for (int i = 0; i < CardsObjectsLists.Count; i++)
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
    private void AddPatent(int id,int subject)
    {
        ++PatentNum;
        Patent patent = new Patent(id, subject, false);
        for(int i = 0; i < CardsList.Count; i++)
        {
            if (CardsList[i].Id == id)
            {
                
                GameObject go = CardsObjectsLists[i];
                Tweener tweener = go.transform.DOScale(0f, 0.3f);
                Card cd = CardsList[i];
                tweener.OnComplete(() =>
                {
                    EventCenter.Broadcast(EventDefine.StopCardActing);
                    CardsList.Remove(cd);
                    CardsObjectsLists.Remove(go);
                    Destroy(go);
                    ShowPatent(id);
                    PatentsList.Add(patent);
                    GameManager.patentAll.Add(patent);
                    CardNum = CardsList.Count;
                    return;
                });
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
            HandCardsShuffle();
        });
    }
    private void Card2Patent(int id,int subject)
    {
        AddPatent(id, subject); 
    }
    #endregion
    #region Game Process
    private void Maintenance()
    {
        showcardNum = PatentNum;
        if (showcardNum == 0)
        {
            EventCenter.Broadcast<string>(EventDefine.Hint, "维护阶段结束");
            EventCenter.Broadcast(EventDefine.MaintenanceFinish);
        }
        else
        {
            StartCoroutine(WaitForShow());
        }
        Debug.Log("Patents num"+ showcardNum);
        
    }
    private IEnumerator WaitForShow()
    {
        EventCenter.Broadcast<string>(EventDefine.Hint, "公开" + showcardNum.ToString() + "张牌");
        while (showcardCount != showcardNum)
        {
            Debug.Log("is Waiting for show card");
            yield return null;
        }
        showcardCount = 0;
        showcardNum = 0;
        EventCenter.Broadcast(EventDefine.MaintenanceFinish);
        EventCenter.Broadcast<string>(EventDefine.Hint, "维护阶段结束");
        yield return null;
    }
    private void ResearchDiscard()
    {
        discardNum = CardNum - sciLimit;
        if(sciLimit - CardNum >=0)
        {
            discardNum = 0;
            EventCenter.Broadcast(EventDefine.DiscardFinish);
            EventCenter.Broadcast<string>(EventDefine.Hint,"研究阶段结束");
        }
        else
        {       
            StartCoroutine(WaitForDiscard());
        }
        //Debug.Log("disCardNum = " + discardNum.ToString());
        //Debug.Log("CardNum = " + CardNum.ToString());
    }
    private IEnumerator WaitForDiscard()
    {
        EventCenter.Broadcast<string>(EventDefine.Hint, "弃置" + (discardNum).ToString() + "张牌");
        while (discardCount!=discardNum){
            Debug.Log("is Waiting for discard");
            yield return null;
        }
        discardCount = 0;
        discardNum = 0;
        EventCenter.Broadcast(EventDefine.DiscardFinish);
        EventCenter.Broadcast<string>(EventDefine.Hint, "研究阶段结束");
        yield return null;
    }
    #endregion
}

