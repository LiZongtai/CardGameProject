using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class SciDataMainJson
{
    /// <summary>
    /// 
    /// </summary>
    public int SciNum;
    /// <summary>
    /// 
    /// </summary>
    public List<SciData> SciData;
}
[Serializable]
public class SciData
{
    /// <summary>
    /// 
    /// </summary>
    public int Id;
    /// <summary>
    /// 艾伦·麦席森·图灵
    /// </summary>
    public string Name;
    /// <summary>
    /// 
    /// </summary>
    public int Subject;
    /// <summary>
    /// 
    /// </summary>
    public int Limit;
    /// <summary>
    /// 你的失败判定改为四项专利，你申请专利时不能使用手牌，必须观看牌堆顶两张牌，从中选择一张作为专利，弃置另外一张
    /// </summary>
    public string Skill;
    /// <summary>
    /// 
    /// </summary>
    public string Img;
}



