using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class SciSelect : MonoBehaviour
{
    public float mTime = 0.5f;
    private static bool isActive = false;
    private Color colorZero = new Color(1, 1, 1, 0);
    public static int sciId;
    private int sciIdL;
    private int sciIdR;
    private Image gameBGMask;
    public GameObject sciL;
    public GameObject sciR;
    private Image sciIconL;
    private Image sciIconR;
    private Image circleL;
    private Image circleR;
    private Text sciNameTextL;
    private Text sciNameTextR;
    private Text sciSkillTextL;
    private Text sciSkillTextR;
    private Image subjectMaskL;
    private Image subjectMaskR;
    private Image subjectL;
    private Image subjectR;
    private Image FrontL;
    private Image FrontR;
    private Image BackL;
    private Image BackR;
    private Button btnL;
    private Button btnR;
    private Text btnTextL;
    private Text btnTextR;
    private void Awake()
    {
        Init();
        
    }
    // Start is called before the first frame update
    void Start()
    {
        StartShow();
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    //IEnumerator ShowCardFrontL()
    //{
    //    yield return new WaitForSeconds(2f);
        
        
    //}
    //IEnumerator ShowCardFrontR()
    //{
    //    yield return new WaitForSeconds(2f);
        
    //}
    
    
    private void Init()
    {
        sciL = transform.Find("SciL").gameObject;
        sciR = transform.Find("SciR").gameObject;
        sciIconL = transform.Find("SciL/SciIcon").GetComponent<Image>();
        sciIconR = transform.Find("SciR/SciIcon").GetComponent<Image>();
        circleL = transform.Find("SciL/Circle").GetComponent<Image>();
        circleR = transform.Find("SciR/Circle").GetComponent<Image>();
        subjectMaskL = transform.Find("SciL/Circle/SubjectMask").GetComponent<Image>();
        subjectMaskR = transform.Find("SciR/Circle/SubjectMask").GetComponent<Image>();
        subjectL = transform.Find("SciL/Circle/SubjectMask/Subject").GetComponent<Image>();
        subjectR = transform.Find("SciR/Circle/SubjectMask/Subject").GetComponent<Image>();
        FrontL= transform.Find("SciL/Front").GetComponent<Image>();
        FrontR = transform.Find("SciR/Front").GetComponent<Image>();
        BackL = transform.Find("SciL/Back").GetComponent<Image>();
        BackR = transform.Find("SciR/Back").GetComponent<Image>();
        sciNameTextL = transform.Find("SciL/SciNameText").GetComponent<Text>();
        sciNameTextR = transform.Find("SciR/SciNameText").GetComponent<Text>();
        sciSkillTextL = transform.Find("SciL/SciSkillText").GetComponent<Text>();
        sciSkillTextR = transform.Find("SciR/SciSkillText").GetComponent<Text>();
        btnL = transform.Find("btn_l").GetComponent<Button>();
        btnR = transform.Find("btn_r").GetComponent<Button>();
        btnTextL = transform.Find("btn_l/Text").GetComponent<Text>();
        btnTextR = transform.Find("btn_r/Text").GetComponent<Text>();
        gameBGMask = transform.Find("GameBGMask").GetComponent<Image>();

        gameBGMask.DOFade(0, 0);
        gameBGMask.DOFade((float)1, 1);

        sciIconL.DOFade(0, 0);
        sciIconR.DOFade(0, 0);
        circleL.DOFade(0, 0);
        circleR.DOFade(0, 0);
        subjectMaskL.DOFade(0, 0);
        subjectMaskR.DOFade(0,0);
        subjectL.DOFade(0,0);
        subjectR.DOFade(0,0);
        sciNameTextL.DOFade(0,0);
        sciNameTextR.DOFade(0,0);
        sciSkillTextL.DOFade(0,0);
        sciSkillTextR.DOFade(0,0);
        FrontL.DOFade(0,0);
        FrontR.DOFade(0,0);
        BackL.DOFade(0,0);
        BackR.DOFade(0,0);
        btnL.image.DOFade(0, 0);
        btnR.image.DOFade(0, 0);
        btnTextL.DOFade(0, 0);
        btnTextR.DOFade(0, 0);
        btnL.gameObject.SetActive(false);
        btnR.gameObject.SetActive(false);

        sciIdL = Random.Range(0, 6);
        do
        {
            sciIdR = Random.Range(0, 6);
        }
        while (sciIdL == sciIdR);
        ScientistManager.GetSciData();
        sciIconL.sprite = ResourcesManager.GetSciSprite(ScientistManager.GetSciIcon(sciIdL));
        sciIconR.sprite = ResourcesManager.GetSciSprite(ScientistManager.GetSciIcon(sciIdR));
        subjectL.sprite = ResourcesManager.GetSubjectSprite("Subject_" + ScientistManager.GetSciSubject(sciIdL));
        subjectR.sprite = ResourcesManager.GetSubjectSprite("Subject_" + ScientistManager.GetSciSubject(sciIdR));
        FrontL.sprite = ResourcesManager.GetPanelSprite("Panel_" + ScientistManager.GetSciSubject(sciIdL));
        FrontR.sprite = ResourcesManager.GetPanelSprite("Panel_" + ScientistManager.GetSciSubject(sciIdR));
        sciNameTextL.text = ScientistManager.GetSciName(sciIdL);
        sciNameTextR.text = ScientistManager.GetSciName(sciIdR);
        sciSkillTextL.text = ScientistManager.GetSciSkill(sciIdL);
        sciSkillTextR.text = ScientistManager.GetSciSkill(sciIdR);
        subjectMaskL.color = GameModel.Subject2Color(ScientistManager.GetSciSubject(sciIdL));
        subjectMaskR.color = GameModel.Subject2Color(ScientistManager.GetSciSubject(sciIdR));
    }
    private void StartShow()
    {
        EventCenter.Broadcast<string>(EventDefine.Hint, "请选择你的英雄");
        Tweener tweener = BackL.DOFade(1, 1);
        BackR.DOFade(1, 1);
        FrontL.DOFade(1, 1);
        FrontR.DOFade(1, 1);
        
        tweener.OnComplete(() =>
        {
            Card2Front();
            ShowCard();
        });
    }
    private void BtnLClick()
    {
        sciId = sciIdL;
        StartCoroutine(ToBack(FrontR.gameObject, BackR.gameObject));
        Tweener tweener = sciIconR.DOFade(0, 1);
        circleR.DOFade(0, 1);
        subjectMaskR.DOFade(0, 1);
        subjectR.DOFade(0, 1);
        sciNameTextR.DOFade(0, 1);
        sciSkillTextR.DOFade(0, 1);
        btnR.image.DOFade(0, 1);
        btnR.gameObject.SetActive(false);
        tweener.OnComplete(() =>
        {
            EndShow();
        });
        
    }
    private void BtnRClick()
    {
        sciId = sciIdR;
        StartCoroutine(ToBack(FrontL.gameObject, BackL.gameObject));
        Tweener tweener = sciIconL.DOFade(0, 1);
        circleL.DOFade(0, 1);
        subjectMaskL.DOFade(0, 1);
        subjectL.DOFade(0, 1);
        sciNameTextL.DOFade(0, 1);
        sciSkillTextL.DOFade(0, 1);
        btnL.image.DOFade(0, 1);
        btnL.gameObject.SetActive(false);
        tweener.OnComplete(() =>
        {
            EndShow();
        });
    }
    private void ShowCard()
    {
        Tweener tweener = sciIconL.DOFade(1, 2);
        sciNameTextL.DOFade(1, 2);
        sciSkillTextL.DOFade(1, 2);
        sciIconR.DOFade(1, 2);
        sciNameTextR.DOFade(1, 2);
        sciSkillTextR.DOFade(1, 2);
        subjectL.DOFade(1, 2);
        subjectR.DOFade(1, 2);
        circleL.DOFade(1, 2);
        circleR.DOFade(1, 2);
        subjectMaskL.DOFade(1, 2);
        subjectMaskR.DOFade(1, 2);
        btnL.gameObject.SetActive(true);
        btnR.gameObject.SetActive(true);

        //circleL.transform.DOLocalRotate(new Vector3(0, 360, 0), 20f, RotateMode.WorldAxisAdd).SetLoops(10);
        //circleR.transform.DOLocalRotate(new Vector3(0, 360, 0), 20f, RotateMode.WorldAxisAdd).SetLoops(10);
        tweener.OnComplete(() =>
        {
            btnL.image.DOFade(1, 0.05f);
            btnR.image.DOFade(1, 0.05f);
            btnTextL.DOFade(1, 0.05f);
            btnTextR.DOFade(1, 0.05f);
            btnL.onClick.AddListener(BtnLClick);
            btnR.onClick.AddListener(BtnRClick);
        });
    }
    private void EndShow()
    {
        Tweener tweener = gameObject.transform.DOScale(new Vector3(2, 2, 2), 2);
        gameBGMask.DOFade(0, 1);
        sciIconL.DOFade(0, 2);
        sciIconR.DOFade(0, 2);
        circleL.DOFade(0, 2);
        circleR.DOFade(0, 2);
        subjectMaskL.DOFade(0, 2);
        subjectMaskR.DOFade(0, 2);
        subjectL.DOFade(0, 2);
        subjectR.DOFade(0, 2);
        sciNameTextL.DOFade(0, 2);
        sciNameTextR.DOFade(0, 2);
        sciSkillTextL.DOFade(0, 2);
        sciSkillTextR.DOFade(0, 2);
        FrontL.DOFade(0, 2);
        FrontR.DOFade(0, 2);
        BackL.DOFade(0, 2);
        BackR.DOFade(0, 2);
        btnL.gameObject.SetActive(false);
        btnR.gameObject.SetActive(false);
        tweener.OnComplete(() =>
        {
            EventCenter.Broadcast(EventDefine.SciSelected, sciId);
            gameObject.transform.parent.gameObject.SetActive(false);
            gameObject.SetActive(false);
        });

    }
    private void Card2Front()
    {
        StartCoroutine(ToFront(FrontL.gameObject, BackL.gameObject));
        StartCoroutine(ToFront(FrontR.gameObject, BackR.gameObject));
        //Tweener tweenerL = BackL.DOFade(1, 0.1f);
        //Tweener tweenerR = BackR.DOFade(1, 0.15f);
        //tweenerL.OnComplete(() =>
        //{
        //    StartCoroutine(ToFront(FrontL.gameObject,BackL.gameObject));
        //});
        //tweenerR.OnComplete(() =>
        //{
        //    StartCoroutine(ToFront(FrontR.gameObject, BackR.gameObject));
        //});
    }
    IEnumerator ToBack(GameObject mFront,GameObject mBack)
    {
        mFront.transform.DORotate(new Vector3(0, 90, 0), mTime);
        for (float i = mTime; i >= 0; i -= Time.deltaTime)
            yield return 0;
        mBack.transform.DORotate(new Vector3(0, 0, 0), mTime);

    }
    /// <summary>
    /// 翻转到正面
    /// </summary>
    IEnumerator ToFront(GameObject mFront, GameObject mBack)
    {
        mBack.transform.DORotate(new Vector3(0, 90, 0), mTime);
        for (float i = mTime; i >= 0; i -= Time.deltaTime)
            yield return 0;
        mFront.transform.DORotate(new Vector3(0, 0, 0), mTime);
    }

}
