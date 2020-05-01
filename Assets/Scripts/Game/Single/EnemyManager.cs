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
    public int SciId;

    //Bool
    private bool isIniting = true;

    //GameObject
    private GameObject Sci;
    private GameObject userInfoBox;
    //public GameObject cardPre;
    //private GameObject cards;
    private GameObject patents;
    public GameObject patentPre;


    //Scientist Box
    private Image SciIcon;
    private Text SciNameText;
    private Text userNameText;
    private Text SciSkillText;
    private Image SciPanel;
    private Image circle;
    private Image subject;
    private Image subjectMask;

    //Text
    private Text rankText;
    private Text vicRateText;
    private Text vicNumText;
    private Text rankNumText;

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

        //cards = transform.Find("Cards").gameObject;
        //cardsPoint = transform.Find("Cards/CardsPoint").localPosition;
        patents = transform.Find("Patents").gameObject;
        patentsPoint = transform.Find("Patents/PatentsPoint").localPosition;


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
        switch (Player) 
        {
            case 1:
                EventCenter.AddListener<int>(EventDefine.DrawCard1, DrawCards);
                break;
            case 2:
                EventCenter.AddListener<int>(EventDefine.DrawCard2, DrawCards);
                break;
            case 3:
                EventCenter.AddListener<int>(EventDefine.DrawCard3, DrawCards);
                break;
        }
        //EventCenter.AddListener<int, int>(EventDefine.Card2Patent, Card2Patent);
        SciId = Random.Range(0, 6);
        ShowSelf(SciId);
    }
    private void ShowSelf(int id)
    {
        ShowSci(id);
        //InitHandCards();
    }
    #endregion
    #region Scientist
    private void ShowSci(int sciId)
    {
        userNameText.text = "Player " + Player;
        SciIcon.gameObject.SetActive(true);
        SciIcon.sprite = ResourcesManager.GetSciSprite(ScientistManager.GetSciIcon(sciId));
        SciPanel.sprite = ResourcesManager.GetPanelSprite("Panel_" + ScientistManager.GetSciSubject(sciId));
        subject.sprite = ResourcesManager.GetSubjectSprite("Subject_" + ScientistManager.GetSciSubject(sciId));
        subjectMask.color = GameModel.Subject2Color(ScientistManager.GetSciSubject(sciId));
        SciNameText.text = ScientistManager.GetSciName(sciId);
        SciSkillText.text = ScientistManager.GetSciSkill(sciId);
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
    private void DrawCards(int num)
    {
        for (int i = 0; i < num; i++)
        {
            Card card = GameManager.cardPile[0];
            CardsList.Add(card);
            GameManager.cardPile.RemoveAt(0);
        }
        if (isIniting == false)
        {
            //InitHandCards();
        }
    }
    //private void DrawHandCards(int id)
    //{
    //    GameObject cardGO = Instantiate(cardPre, cards.transform, true);
    //    cardGO.transform.localScale = new Vector2(0.85f, 0.85f);
    //    cardGO.transform.name = "Card_" + id;
    //    cardGO.GetComponent<CardUI>().GetCardId(id);
    //    cardGO.GetComponent<CardUI>().CardInit();
    //    cardGO.transform.localPosition = cards.transform.localPosition;
    //    Tweener tweener = cardGO.transform.DOLocalMove(new Vector2(cards.transform.localPosition.x, cards.transform.localPosition.y - 350), 0.5f);
    //    tweener.OnComplete(() =>
    //    {
    //        CardsObjectsLists.Add(cardGO);
    //        HandCardsShuffle();
    //    });
    //}
    //private void HandCardsShuffle()
    //{
    //    for (int i = 0; i < CardsObjectsLists.Count; i++)
    //    {
    //        CardsObjectsLists[i].transform.DOLocalMove(new Vector2(cards.transform.localPosition.x + i * 600 / CardsObjectsLists.Count, cards.transform.localPosition.y - 350), 0.3f);
    //    }
    //}
    //private void HandCardsShow(GameObject go)
    //{
    //    //for(int i = 0; i < handCards.Count; i++)
    //    //{
    //    //    if (go != handCardsObjects[i].gameObject)
    //    //    {
    //    //        Debug.Log("Interact invalid");
    //    //    }
    //    //}
    //    int count = go.transform.parent.childCount - 1;
    //    go.transform.SetSiblingIndex(count);
    //    go.transform.DOLocalMoveY(-150, 0.02f);
    //    go.GetComponent<CardUI>().ShowEffect(0.02f);
    //}
    //private void HandCardsEndShow(GameObject go)
    //{
    //    int count = 0;
    //    for (int i = 0; i < CardsObjectsLists.Count; i++)
    //    {
    //        if (go == CardsObjectsLists[i].gameObject)
    //        {
    //            count = i;
    //        }
    //    }
    //    go.transform.DOLocalMoveY(-350, 0.02f);
    //    go.transform.SetSiblingIndex(count);
    //    go.GetComponent<CardUI>().EndShowEffect(0.02f);
    //}
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
        });
    }
    private void Card2Patent(int id, int subject)
    {
        AddPatent(id, subject);
    }
    #endregion
}
