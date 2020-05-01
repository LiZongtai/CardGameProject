using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;


public class CardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,IPointerClickHandler
{
    //public
    public bool isFront;
    public int CardID = -1;
    public int CardType=-1;
    public int CardSubject=-1;
    //public AudioSource openCard;

    //Card Bool
    private bool isShowingIE = false;
    private bool isShowingSE = false;
    private bool isEndShowingIE = false;
    private bool isEndShowingSE = false;

    //Card GameObject
    private int PanelCount;
    private GameObject front;

    //Card Text
    private Text ieText;
    private Text seText;
    private Text nameText;

    // Card Image
    private Image center;
    private Image top;
    private Image buttom;
    private Image type;
    private Image back;
    private Image iconBG;
    private Image icon;
    private Image subjectMask;
    private Image subjectIcon;
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
    #region Card Init
    private void Init()
    {
        //openCard = gameObject.AddComponent<AudioSource>();
        front = transform.Find("Front").gameObject;
        top = transform.Find("Front/Top").GetComponent<Image>();
        center = transform.Find("Front/Center").GetComponent<Image>();
        buttom = transform.Find("Front/Buttom").GetComponent<Image>();
        ieText = transform.Find("Effect/IE").GetComponent<Text>();
        seText = transform.Find("Effect/SE").GetComponent<Text>();
        nameText = transform.Find("Name").GetComponent<Text>();
        icon = transform.Find("IconBox/Mask/Icon").GetComponent<Image>();
        type = transform.Find("Effect/Type").GetComponent<Image>();
        subjectIcon = transform.Find("Subject/SubjectIcon").GetComponent<Image>();
        subjectMask = transform.Find("Subject/SubjectMask").GetComponent<Image>();
        iconBG = transform.Find("IconBg").GetComponent<Image>();
        PanelCount = gameObject.transform.GetSiblingIndex();
        center.transform.DOScaleY((float)0.26, 0);
        seText.DOFade(0, 0);
        ieText.DOFade(0, 0);
        type.DOFade(0, 0);
    }
    public void GetCardId(int id)
    {
        this.CardID = id;
    }
    
    public void CardInit()
    {
        //if (CardID == -1)
        //{
        //    return;
        //}
        CardDataManager.GetCardData();
        CardType = CardDataManager.GetCardType(CardID);
        CardSubject = CardDataManager.GetCardSubject(CardID);
        icon.sprite = ResourcesManager.GetCardIconSprite(CardDataManager.GetCardIcon(CardID));
        subjectIcon.sprite = ResourcesManager.GetSubjectSprite("Subject_" + CardSubject);
        type.sprite = ResourcesManager.GetTypeSprite("Type_" + CardType);
        iconBG.sprite = ResourcesManager.GetCardIconBgSprite("IconBox_" + CardSubject);
        nameText.text = CardDataManager.GetCardName(CardID);
        ieText.text = CardDataManager.GetCardIE(CardID);
        seText.text = CardDataManager.GetCardSE(CardID);
        subjectMask.color = GameModel.Subject2Color(CardSubject);
        string cardFrontPath = "CardFront" + CardSubject;
        top.sprite = ResourcesManager.GetCardFrontSprite(cardFrontPath + "_0", cardFrontPath);
        center.sprite = ResourcesManager.GetCardFrontSprite(cardFrontPath + "_1", cardFrontPath);
        buttom.sprite = ResourcesManager.GetCardFrontSprite(cardFrontPath + "_2", cardFrontPath);

        if(ieText.text=="渗透"|| ieText.text == "缓冲")
        {
            ieText.fontSize = 40;
            ieText.alignment = (TextAnchor)TextAlignment.Center;
            seText.alignment = (TextAnchor)TextAlignment.Left;
        }
        else
        {
            ieText.fontSize = 20;
            ieText.alignment = (TextAnchor)TextAlignment.Left;
            seText.alignment = (TextAnchor)TextAlignment.Left;
        }

    }
    #endregion
    #region ShowEffect
    public void ShowEffect(float time)
    {
        
        isShowingIE = true;
        isShowingSE = true;
        ieText.gameObject.SetActive(true);
        seText.gameObject.SetActive(true);
        type.gameObject.SetActive(true);
        center.transform.DOScaleY((float)1.2, time);
        top.transform.DOLocalMoveY(475, time);
        Tweener tweener = buttom.transform.DOLocalMoveY(-475, time);
        tweener.OnComplete(() => {
            ieText.DOFade(1, time/2);
            seText.DOFade(1, time / 2);
            type.DOFade(1, time / 2);
            isShowingIE = false;
            isShowingSE = false;
        });
    }
    public void EndShowEffect(float time)
    {

        isEndShowingIE = true;
        isEndShowingSE = true;
        Tweener tweener = type.DOFade(0, time);
        ieText.DOFade(0, time);
        seText.DOFade(0, time);
        tweener.OnComplete(()=> {
            center.transform.DOScaleY((float)0.26, time);
            Tweener tweener1 = top.transform.DOLocalMoveY(240, time);
            Tweener tweener2 = buttom.transform.DOLocalMoveY(-240, time);
            ieText.gameObject.SetActive(false);
            seText.gameObject.SetActive(false);
            type.gameObject.SetActive(false);
            tweener1.OnComplete(()=>{
                isEndShowingIE = false;
            });
            tweener2.OnComplete(() => {
                isEndShowingSE = false;
            });
        });
    }
    public void ShowIEFlag()
    {
        isShowingIE = false;
    }
    private void ShowSEFlag()
    {
        isShowingSE = false;
    }
    private void ShowIE()
    {
        if (isShowingIE == true)
        {
            return;
        }
        else if (isShowingSE == true)
        {
            return;
        }
        isShowingIE = true;
        center.transform.DOScaleY((float)0.63, (float)0.5);
        center.transform.DOLocalMoveY((float)92.5, (float)0.5);
        top.transform.DOLocalMoveY(425, (float)0.5);
        ieText.DOFade(1, (float)0.5);
    }

    private void EndShowIE()
    {
        if (isShowingSE == true)
        {
            return;
        }
        Tweener tweener = center.transform.DOScaleY((float)0.26, (float)0.5);
        center.transform.DOLocalMoveY((float)0, (float)0.5);
        top.transform.DOLocalMoveY(240, (float)0.5);
        ieText.DOFade(0, (float)0.5);
        tweener.OnComplete(ShowIEFlag);
    }
    private void ShowSE()
    {
        if (isShowingIE == true)
        {
            return;
        }
        else if(isShowingSE == true){
            return;
        }
        isShowingSE = true;
        center.transform.DOScaleY((float)0.63, (float)0.5);
        center.transform.DOLocalMoveY((float)-92.5, (float)0.5);
        buttom.transform.DOLocalMoveY(-425, (float)0.5);
        seText.DOFade(1, (float)0.5);

    }

    private void EndShowSE()
    {
        if (isShowingIE == true)
        {
            return;
        }
        Tweener tweener = center.transform.DOScaleY((float)0.26, (float)0.5);
        center.transform.DOLocalMoveY(0, (float)0.5);
        buttom.transform.DOLocalMoveY(-240, (float)0.5);
        seText.DOFade(0, (float)0.5);
        tweener.OnComplete(ShowSEFlag);
    }

    #endregion
    #region Dynamic effect
    
    #endregion
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isShowingIE != true && isShowingSE != true)
        {
            EventCenter.Broadcast(EventDefine.CardShowing, gameObject);
            //gameObject.transform.DOLocalMoveY(-150, 0.05f);
            //ShowEffect();
        }       
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isEndShowingIE == false && isEndShowingSE == false)
        {
            EventCenter.Broadcast(EventDefine.CardEndShowing, gameObject);
            //gameObject.transform.DOLocalMoveY(-225, 0.05f);
            //EndShowEffect();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        EventCenter.Broadcast(EventDefine.CardActing, gameObject);
    }
}
