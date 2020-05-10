using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using DG.Tweening;

public class CardManager : MonoBehaviour
{
    //public 
    //public int Player;

    //Int
    private int actingCardId;
    private int actingCardSub;
    private int actingCardType;
    private int CardNum;

    //Bool
    private bool isInfiltration=false;

    //List
    public List<Card> CardsList = new List<Card>();
    public List<GameObject> CardsObjectsLists = new List<GameObject>();

    // Start is called before the first frame update
    private void Awake()
    {
        EventListener();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void EventListener()
    {
        //Card Action
        EventCenter.AddListener(EventDefine.InfiltrationFinish, InfiltrationFinish);

        EventCenter.AddListener<int, int, int>(EventDefine.IEaction, CardIE_respose);
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
    private void CardIE_respose(int id,int sub,int type)
    {
        actingCardId = id;
        actingCardSub = sub;
        actingCardType = type;
        switch (type)
        {
            case 0:
                Infiltration();
                break;
            case 1:
                Defense();
                break;
        }
    }
    private void Infiltration()
    {
        EventCenter.Broadcast(EventDefine.Infiltration);
        StartCoroutine(WaitForInfiltration());
    }
    private void InfiltrationFinish()
    {
        int ii = Id2CardIndex(actingCardId);
        GameObject go = CardsObjectsLists[ii];
        go.transform.DOLocalMove(new Vector2(go.transform.localPosition.x - 800, go.transform.localPosition.y + 500), 0.1f);
        Tweener tweener = go.transform.DOScale(0.2f, 0.1f);
        tweener.OnComplete(() =>
        {
            Destroy(go);
            CardsObjectsLists.RemoveAt(ii);
            CardsList.RemoveAt(ii);
            CardNum = CardsList.Count;
            isInfiltration = false;
            EventCenter.Broadcast(EventDefine.Acted);
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
    private IEnumerator WaitForInfiltration()
    {
        isInfiltration = true;
        while (isInfiltration)
        {
            Debug.Log("is Infiltration");
            yield return null;
        }
    }
    private void Defense()
    {

    }
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
        Debug.Log("CardList[0]: "+SelfManager.CardsList[0].Id);
    }
}

