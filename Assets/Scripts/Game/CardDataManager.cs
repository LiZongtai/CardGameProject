using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

public class CardDataManager
{
    private static int cardNum;
    private static CardDataMainJson cardD;
    private static List<CardData> cardData;
    public static int cardId;
    public static string name;
    public static string ie;
    public static string se;
    public static int subject;
    public static int type;
    // Start is called before the first frame update
    public static CardDataMainJson LoadCardDataJson()
    {
        if (!File.Exists(Application.dataPath + "/StreamingAssets/Card.json"))
        {
            return null;
        }
        StreamReader sr = new StreamReader(Application.dataPath + "/StreamingAssets/Card.json");
        if (sr == null)
        {
            return null;
        }
        string json = sr.ReadToEnd();
        if (json.Length > 0)
        {
            return JsonUtility.FromJson<CardDataMainJson>(json);
        }
        return null;
    }
    public static void GetCardData()
    {
        cardD = LoadCardDataJson(); 
        cardData = cardD.CardData;
    }
    public static int GetCardNum()
    {
        return cardD.CardNum;
    }
    public static string GetCardName(int id)
    {
        return cardData[id].Name;
    }
    public static string GetCardIcon(int id)
    {
        string cardImgPath = "CardIcon_" + id.ToString();
        return cardImgPath;
    }
    public static string GetCardIE(int id)
    {
        return cardData[id].IE;
    }
    public static string GetCardSE(int id)
    {
        return cardData[id].SE;
    }
    public static int GetCardSubject(int id)
    {
        return cardData[id].Subject;
    }
    public static int GetCardType(int id)
    {
        return cardData[id].Type;
    }
}
