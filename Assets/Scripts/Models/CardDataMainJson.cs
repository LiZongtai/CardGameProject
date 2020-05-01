using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CardDataMainJson
{
    /// <summary>
    /// 
    /// </summary>
    public int CardNum;
    /// <summary>
    /// 
    /// </summary>
    public List<CardData> CardData;
}
[Serializable]
public class CardData
{
    /// <summary>
    /// 
    /// </summary>
    public int Id;
    /// <summary>
    /// 多路复用
    /// </summary>
    public string Name;
    /// <summary>
    /// 展示牌堆顶的四张牌（特殊影响触发时为两张），拿走其中与此牌花色相同的牌
    /// </summary>
    public string IE;
    /// <summary>
    /// 一次可打出两张渗透/缓冲，渗透效果不叠加，特殊影响和花色叠加，若如此做，立即发动弱化的即时效果
    /// </summary>
    public string SE;
    /// <summary>
    /// 
    /// </summary>
    public int Subject;
    /// <summary>
    /// 
    /// </summary>
    public int Type;
}


