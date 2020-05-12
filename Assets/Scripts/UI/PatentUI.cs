using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class PatentUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //public
    public bool isShowingPatent = false;
    public bool isEndShowingPatent = false;
    public bool isFront;
    public bool isPrivate;
    public int PatentID = -1;
    public int PatentSubject = -1;

    //Bool
    private bool isRotating = false;
    private bool isShowing = false;

    //Image
    private Image patBG;
    private Image patIcon;
    private Image circle;

    //Patent Info
    private GameObject patentInfo;
    private Image patentInfoBG;
    private Image patentSubjectMask;
    private Image patentSubjectIcon;
    private Text patentName;
    private Text patentState;
    private Text patentIE;
    private Text patentSE;


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
        if (isRotating == false)
        {
            isRotating = true;
            Tweener tweener = circle.transform.DOLocalRotate(new Vector3(0, 0, 360), 2f, RotateMode.WorldAxisAdd);
            tweener.OnComplete(() =>
            {
                isRotating = false;
            });
        }
    }
    private void Init()
    {
        patBG = transform.Find("PatBG").GetComponent<Image>();
        patIcon = transform.Find("Mask/PatIcon").GetComponent<Image>();
        circle = transform.Find("Circle").GetComponent<Image>();
        patentInfo = transform.Find("PatentInfo").gameObject;
        patentSubjectIcon = transform.Find("PatentInfo/Mask/Icon").GetComponent<Image>();
        patentSubjectMask = transform.Find("PatentInfo/Mask").GetComponent<Image>();
        patentState = transform.Find("PatentInfo/State").GetComponent<Text>();
        patentName = transform.Find("PatentInfo/Name").GetComponent<Text>();
        patentIE = transform.Find("PatentInfo/IE").GetComponent<Text>();
        patentSE = transform.Find("PatentInfo/SE").GetComponent<Text>();
        patentInfo.SetActive(false);

    }
    public void GetPatentId(int id)
    {
        this.PatentID = id;
    }
    public void PatentInit()
    {
        CardDataManager.GetCardData();
        PatentSubject = CardDataManager.GetCardSubject(PatentID);
        patBG.sprite = ResourcesManager.GetCardIconBgSprite("IconBox_" + PatentSubject);
        patIcon.sprite = ResourcesManager.GetCardIconSprite(CardDataManager.GetCardIcon(PatentID));
        circle.color= GameModel.Subject2Color(PatentSubject);
        isRotating = false;
        patentInfo.transform.localScale = new Vector3(5, 5, 0);
        patentInfo.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        //circle.gameObject.SetActive(false);

    }
    private void PatentShow()
    {
        Tweener tweener= circle.DOFade(1, 0.3f);
        patBG.DOFade(1, 0.3f);
        patIcon.DOFade(1, 0.3f);
    }
    public void PatentInfoShow()
    {
        patentInfo.SetActive(true);
        
        //Debug.Log("Patent Show");
        isShowingPatent = true;
        //Debug.Log(patentInfo.transform.position.x + " " + patentInfo.transform.position.y);
        patentSubjectMask.color = GameModel.Subject2Color(PatentSubject);
        patentSubjectIcon.sprite = ResourcesManager.GetSubjectSprite("Subject_" + PatentSubject);
        patentName.text = CardDataManager.GetCardName(PatentID);
        patentIE.text = CardDataManager.GetCardIE(PatentID);
        patentSE.text = CardDataManager.GetCardSE(PatentID);
        
        if (patentIE.text == "渗透" || patentIE.text == "缓冲")
        {
            //patentIE.fontSize = 40;
            patentIE.alignment = (TextAnchor)TextAlignment.Center;
            patentIE.resizeTextForBestFit = true;
        }
        else
        {
            patentIE.fontSize = 20;
            patentIE.resizeTextForBestFit = false;
            patentIE.alignment = (TextAnchor)TextAlignment.Left;
        }
        if (isPrivate)
        {
            patentState.text = "状态：公有";
        }
        else
        {
            patentState.text = "状态：私有";
        }
        Tweener tweener = patentInfo.transform.DOScale(5, 0.3f);
        //Tweener tweener = patentInfo.transform.DOScale(1.5f, 0.3f);
        tweener.OnComplete(() =>
        {
            isShowingPatent = false;
        });
    }
    public void EndPatentInfoShow()
    {
        //Debug.Log("End Patent Show");
        isEndShowingPatent = true;
        Tweener tweener = patentInfo.transform.DOScale(0, 0.3f);
        tweener.OnComplete(() =>
        {
            patentInfo.SetActive(false);
            isEndShowingPatent = false;
        });
    }
    private GameObject SearchPatent(int id)
    {
        for (int i = 0; i < GameManager.patentAll.Count; i++)
        {
            if (GameManager.patentAll[i].Id == id)
            {
                return GameManager.patentObjectAll[i];
            }
        }
        return null;
    }
    public void PatentInfoPosition()
    {
        //set position
        Vector3 extV = CheckRect(patentInfo);
        //Debug.Log(extV.x + " " + extV.y);
        patentInfo.transform.position = new Vector2(patentInfo.transform.position.x + 225 - extV.x, patentInfo.transform.position.y - extV.y);
        //patentInfo.transform.localScale = new Vector3(0, 0, 0);
        //
    }
    private Vector3 CheckRect(GameObject go)
    {
        float extX;
        float extY;
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        Rect screenRect = new Rect(0, 0, screenWidth, screenHeight);
        Rect goRect = go.GetComponent<RectTransform>().rect;
        //Debug.Log("width_" + goRect.width + " height_" + goRect.height + " x_" + go.transform.position.x + " y_" + go.transform.position.y);
        //Debug.Log(screenRect.xMin + " " + screenRect.yMin + " " + screenRect.xMax + " " + screenRect.yMax);
        if (go.transform.position.x - goRect.width / 2 < screenRect.xMin)
        {
            extX = (go.transform.position.x - goRect.width / 2) - screenRect.xMin;
        }
        else if (go.transform.position.x + goRect.width / 2 > screenRect.xMax)
        {
            extX = (go.transform.position.x + goRect.width / 2) - screenRect.xMax;
        }
        else
        {
            extX = 0;
        }
        if (go.transform.position.y - goRect.height / 2 < screenRect.yMin)
        {
            extY = (go.transform.position.y - goRect.height / 2) - screenRect.yMin;
        }
        else if (go.transform.position.y + goRect.height / 2 > screenRect.yMax)
        {
            extY = (go.transform.position.y + goRect.height / 2) - screenRect.yMax;
        }
        else
        {
            extY = 0;
        }
        Vector3 v = new Vector3(extX, extY, 0);
        return v;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isShowingPatent == false)
        {
            EventCenter.Broadcast(EventDefine.PatentShowing, gameObject);
        }   
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isEndShowingPatent == false)
        {
            EventCenter.Broadcast(EventDefine.PatentEndShowing, gameObject);
        } 
    }
}
