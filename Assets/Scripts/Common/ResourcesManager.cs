using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesManager
{
    private static Dictionary<string, Sprite> nameSpriteDic = new Dictionary<string, Sprite>();
    private static Dictionary<string, Sprite> sciSpriteDic = new Dictionary<string, Sprite>();
    private static Dictionary<string, Sprite> subjectSpriteDic = new Dictionary<string, Sprite>();
    private static Dictionary<string, Sprite> panelSpriteDic = new Dictionary<string, Sprite>();
    private static Dictionary<string, Sprite> cardIConSpriteDic = new Dictionary<string, Sprite>();
    private static Dictionary<string, Sprite> cardTypeSpriteDic = new Dictionary<string, Sprite>();
    private static Dictionary<string, Sprite> cardFrontSpriteDic = new Dictionary<string, Sprite>();
    private static Dictionary<string, Sprite> cardIconBgSpriteDic = new Dictionary<string, Sprite>();
    #region Get General Sprite
    public static Sprite GetSprite(string iconName)
    {
        if (nameSpriteDic.ContainsKey(iconName))
        {
            return nameSpriteDic[iconName];
        }
        else
        {
            Sprite[] sprites = Resources.LoadAll<Sprite>("headIcon");
            string[] nameArr = iconName.Split('_');
            Sprite temp = sprites[int.Parse(nameArr[1])];
            nameSpriteDic.Add(iconName, temp);
            return temp;
        }
       
    }
    public static Sprite GetSubjectSprite(string iconName)
    {
        if (subjectSpriteDic.ContainsKey(iconName))
        {
            return subjectSpriteDic[iconName];
        }
        else
        {
            Sprite[] sprites = Resources.LoadAll<Sprite>("Subject");
            string[] nameArr = iconName.Split('_');
            Sprite temp = sprites[int.Parse(nameArr[1])];
            subjectSpriteDic.Add(iconName, temp);
            return temp;
        }
    }
    #endregion
    #region Get Sci Sprite
    public static Sprite GetSciSprite(string iconName)
    {
        if (sciSpriteDic.ContainsKey(iconName))
        {
            return sciSpriteDic[iconName];
        }
        else
        {
            Sprite[] sprites = Resources.LoadAll<Sprite>("Scientist");
            string[] nameArr = iconName.Split('_');
            Sprite temp = sprites[int.Parse(nameArr[1])];
            sciSpriteDic.Add(iconName, temp);
            return temp;
        }
    }

    public static Sprite GetPanelSprite(string iconName)
    {
        if (panelSpriteDic.ContainsKey(iconName))
        {
            return panelSpriteDic[iconName];
        }
        else
        {
            Sprite[] sprites = Resources.LoadAll<Sprite>("Panel");
            string[] nameArr = iconName.Split('_');
            Sprite temp = sprites[int.Parse(nameArr[1])];
            panelSpriteDic.Add(iconName, temp);
            return temp;
        }
    }
    #endregion
    #region Card Sprite
    public static Sprite GetCardIconSprite(string iconName)
    {
        if (cardIConSpriteDic.ContainsKey(iconName))
        {
            return cardIConSpriteDic[iconName];
        }
        else
        {
            Sprite[] sprites = Resources.LoadAll<Sprite>("CardIcon");
            string[] nameArr = iconName.Split('_');
            Sprite temp = sprites[int.Parse(nameArr[1])];
            cardIConSpriteDic.Add(iconName, temp);
            return temp;
        }
    }
    public static Sprite GetTypeSprite(string iconName)
    {
        if (cardTypeSpriteDic.ContainsKey(iconName))
        {
            return cardTypeSpriteDic[iconName];
        }
        else
        {
            Sprite[] sprites = Resources.LoadAll<Sprite>("Type");
            string[] nameArr = iconName.Split('_');
            Sprite temp = sprites[int.Parse(nameArr[1])];
            cardTypeSpriteDic.Add(iconName, temp);
            return temp;
        }
    }
    public static Sprite GetCardFrontSprite(string iconName,string cardFrontName)
    {
        if (cardFrontSpriteDic.ContainsKey(iconName))
        {
            return cardFrontSpriteDic[iconName];
        }
        else
        {
            Sprite[] sprites = Resources.LoadAll<Sprite>(cardFrontName);
            string[] nameArr = iconName.Split('_');
            Sprite temp = sprites[int.Parse(nameArr[1])];
            cardFrontSpriteDic.Add(iconName, temp);
            return temp;
        }
    }
    public static Sprite GetCardIconBgSprite(string iconName)
    {
        if (cardIconBgSpriteDic.ContainsKey(iconName))
        {
            return cardIconBgSpriteDic[iconName];
        }
        else
        {
            Sprite[] sprites = Resources.LoadAll<Sprite>("IconBox");
            string[] nameArr = iconName.Split('_');
            Sprite temp = sprites[int.Parse(nameArr[1])];
            cardIconBgSpriteDic.Add(iconName, temp);
            return temp;
        }
    }
    #endregion

}
