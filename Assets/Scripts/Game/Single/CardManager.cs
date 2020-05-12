using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CardManager : MonoBehaviour
{
    //public 
    public int Player;

    //Int
    private int infCardId=-1;
    private int defCardId=-1;
    private int actingCardId=-1;
    private int actingCardSub;
    private int actingCardType;
    private int CardNum;
    private int InfiltrationPlayer = -1;
    private int DefensePlayer = -1;

    //Bool
    private bool isInfiltration=false;
    private bool isDefense = false;
    private bool isInfiltrationEnd=false;

    //GameObject
    private GameObject canvas;
    private Image cardShowMask;

    //List
    public List<Card> CardsList = new List<Card>();
    public List<GameObject> CardsObjectsLists = new List<GameObject>();

    // Start is called before the first frame update
    private void Awake()
    {
        Init();
        EventListener();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void Init()
    {
        canvas = GameObject.Find("Canvas");
        cardShowMask = canvas.transform.Find("CardShowMask").GetComponent<Image>();
    }
    private void EventListener()
    {
        //Card Action
        EventCenter.AddListener<int>(EventDefine.Infiltration, InfiltrationSet);
        EventCenter.AddListener<int,int>(EventDefine.InfiltrationFinish, InfiltrationFinish);
        EventCenter.AddListener<int>(EventDefine.Defense, DefenseSet);

        EventCenter.AddListener<int, int, int, int>(EventDefine.IEaction, CardIE_respose);
        //EventCenter.AddListener<int>(EventDefine.s0_RoundStart,s0_response);
        //EventCenter.AddListener<int>(EventDefine.s1_PlayRoundStart, s1_response);
        //EventCenter.AddListener<int>(EventDefine.s2_BeforeRound, s2_response);
        //EventCenter.AddListener<int>(EventDefine.s3_BeforeAction, s3_response);
        //EventCenter.AddListener<int>(EventDefine.s4_AfterAction, s4_response);
        //EventCenter.AddListener<int>(EventDefine.s5_BeforeDraw, s5_response);
        //EventCenter.AddListener<int>(EventDefine.s6_BeforeDiscard, s6_response);
        //EventCenter.AddListener<int>(EventDefine.s7_AfterDiscard, s7_response);
        //EventCenter.AddListener<int>(EventDefine.s8_BeforeMaintenance, s8_response);
        //EventCenter.AddListener<int>(EventDefine.s9_PlayerRoundEnd, s9_response);
        //EventCenter.AddListener<int>(EventDefine.s10_RoundEnd, s10_response);
        //EventCenter.AddListener<int>(EventDefine.r0_attack, r0_response);
        //EventCenter.AddListener<int>(EventDefine.r1_attacked, r1_response);
        //EventCenter.AddListener<int>(EventDefine.r2_defend, r2_response);
        //EventCenter.AddListener<int>(EventDefine.r3_defended, r3_response);
        //EventCenter.AddListener<int>(EventDefine.CardSE, CardSE_respose);
    }
    private void IECardResponse(Card card)
    {
        int cardIndex = Id2CardIndex(card.Id);
        GameObject cardGO = CardsObjectsLists[cardIndex];
        cardGO.transform.SetParent(canvas.transform);
        cardGO.GetComponent<CardUI>().isShowing = true;
        //cardGO.GetComponent<CardUI>().isBack = false;
        cardGO.GetComponent<CardUI>().ShowEffect(0.3f);
        cardShowMask.gameObject.SetActive(true);
        cardShowMask.DOFade(1, 0.3f);
        cardGO.transform.SetAsLastSibling();
        if (cardGO.GetComponent<CardUI>().isBack)
        {
            cardGO.transform.DORotate(new Vector3(0, 0, 0), 0.2f);
        }
        cardGO.transform.DOScale(0.7f, 0.3f);
        Tweener tweener = cardGO.transform.DOLocalMove(new Vector2(0, 0), 0.3f);
        //Tweener tweener = cardGO.transform.DOLocalMoveX(cardGO.transform.localPosition.x + 300, 0.3f);
        tweener.OnComplete(() =>
        {
            CardsObjectsLists.Remove(cardGO);
            CardsList.Remove(card);
            CardNum = CardsList.Count;
            GameManager.SetDisPile(card);
            StartCoroutine(CardShowDelay(cardGO));
        });
    }
    private IEnumerator CardShowDelay(GameObject cardGO)
    {
        yield return new WaitForSeconds(2.2f);
        cardGO.GetComponent<CardUI>().EndShowEffect(0.3f);
        //cardGO.transform.DOLocalMove(new Vector2(300, -400), 0.2f);
        Tweener tweener = cardGO.transform.DOScale(0, 0.3f);
        cardShowMask.DOFade(0, 0.3f);
        tweener.OnComplete(() =>
        {
            cardShowMask.gameObject.SetActive(false);
            Destroy(cardGO);
        });
    }
    private void CardIE_respose(int player, int id,int sub,int type)
    {
        if (player != Player)
        {
            return;
        }
        actingCardId = id;
        actingCardSub = sub;
        actingCardType = type;
        int index = Id2CardIndex(id);
        Card actingCard = CardsList[index];
        IECardResponse(actingCard);
        switch (type)
        {
            case 0:
                infCardId = actingCardId;
                Infiltration();
                break;
            case 1:
                defCardId = actingCardId;
                Defense();
                break;
        }
    }
    /// <summary>
    /// 渗透
    /// </summary>
    private void Infiltration()
    {
        //StartCoroutine(WaitForDefense());
        //Player : infiltrator
        EventCenter.Broadcast(EventDefine.Infiltration,Player);
        StartCoroutine(WaitForInfiltration());
    }
    private IEnumerator WaitForInfiltration()
    {
        isInfiltration = true;
        while (isInfiltration)
        {
            Debug.Log("is Infiltration Acting " + isInfiltration);
            yield return null;
        }
        EventCenter.Broadcast(EventDefine.Acted);
    }
    /// <summary>
    /// 缓冲
    /// </summary>
    private void Defense()
    {
        isDefense = true;
        StartCoroutine(WaitForDefense());
        EventCenter.Broadcast(EventDefine.Defense, Player);
        //StartCoroutine(WaitForDefense());
    }
    private IEnumerator WaitForDefense()
    {
        yield return new WaitForSeconds(2f);
        EventCenter.Broadcast(EventDefine.InfiltrationFinish, DefensePlayer, InfiltrationPlayer);
        Debug.Log("Player " + Player + " is Defensing");
    }
    private void DefenseSet(int defPlayer)
    {
        DefensePlayer = defPlayer;
    }
    private void InfiltrationSet(int infPlayer)
    {
        InfiltrationPlayer = infPlayer;
    }
    private void InfiltrationFinish(int defPlayer, int infPlayer)
    {
        if (infPlayer==Player)
        {
            isInfiltration = false;
        }
        if (defPlayer == Player)
        {
            isDefense = false;
        }
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
        Debug.Log("Player " + Player + " Can't find this card in hands. Id=" + id);
        return -1;
    }
    //private void InfiltrationFinish(int playerDef,int playerInf)
    //{
    //    if (playerInf != Player)
    //    {
    //        return;
    //    }
    //    if (infCardId == -1)
    //    {
    //        isInfiltration = false;
    //        return;
    //    }
    //    Debug.Log("Infiltration Finish. InfCardId="+ infCardId);
    //    int ii = Id2CardIndex(infCardId);
    //    GameObject go = CardsObjectsLists[ii];
    //    go.transform.DOLocalMove(new Vector2(go.transform.localPosition.x - 800, go.transform.localPosition.y + 500), 0.1f);
    //    Tweener tweener = go.transform.DOScale(0.2f, 0.1f);
    //    tweener.OnComplete(() =>
    //    {
    //        //Destroy(go);
    //        //CardsObjectsLists.RemoveAt(ii);
    //        //CardsList.RemoveAt(ii);
    //        //CardNum = CardsList.Count;
    //        isInfiltration = false;
    //        infCardId = -1;
    //    });
    //}
    //private void DefenseFinish(int playerDef,int playerInf)
    //{
    //    //Debug.Log("Defense Finish and actingCardId " + defCardId);
    //    if (playerDef != Player)
    //    {
    //        return;
    //    }
    //    if(defCardId == -1)
    //    {
    //        isDefense = false;
    //        return;
    //    }
    //    Debug.Log("Defense Finish. DefCardId=" + defCardId);
    //    int ii = Id2CardIndex(defCardId);
    //    GameObject go = CardsObjectsLists[ii];
    //    go.transform.DOLocalMove(new Vector2(go.transform.localPosition.x - 800, go.transform.localPosition.y + 500), 0.1f);
    //    Tweener tweener = go.transform.DOScale(0.2f, 0.1f);
    //    tweener.OnComplete(() =>
    //    {
    //        //Destroy(go);
    //        //CardsObjectsLists.RemoveAt(ii);
    //        //CardsList.RemoveAt(ii);
    //        //CardNum = CardsList.Count;
    //        isDefense = false;
    //        defCardId = -1;
    //        //EventCenter.Broadcast(EventDefine.Acted);
    //    });
    //}
    private void CardSE_respose(int id)
    {
        Debug.Log("Card " + id + " Response");
        Type t = typeof(CardManager);
        MethodInfo mt = t.GetMethod("CardSE_" + id, BindingFlags.Public);
        if (mt == null)
        {

            Debug.Log("don't find function !");

        }
        else
        {
            Debug.Log("find function !");
            string str = (string)mt.Invoke(this, new object[] { });

        }
    }
    #region response
     private void s0_response(int player)
    {
        
    }
    private void s1_response(int player)
    {
        
    }
    private void s2_response(int player)
    {

    }
    private void s3_response(int player)
    {

    }
    private void s4_response(int player)
    {

    }
    private void s5_response(int player)
    {

    }
    private void s6_response(int player)
    {

    }
    private void s7_response(int player)
    {

    }
    private void s8_response(int player)
    {

    }
    private void s9_response(int player)
    {

    }
    private void s10_response(int player)
    {

    }
    private void r0_response(int player)
    {

    }
    private void r1_response(int player)
    {

    }
    private void r2_response(int player)
    {

    }
    private void r3_response(int player)
    {

    }
    #endregion

}
public class CardSE 
{
    public void CardSE_51()
    {
        Debug.Log("Card 51 !!!!!!!!!!!!!!!!!!!1");
    }
}