using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;


public class ScientistManager
{
    private static int sciNum;
    private static SciDataMainJson sciD;
    private static List<SciData> sciData;
    public static int sciId;
    public static string sciName;
    public static int sciSubject;
    public static int sciLimit;
    public static string sciSkill;
    public static string sciImg;
    public static SciDataMainJson LoadSciDataJson()
    {
        if (!File.Exists(Application.dataPath + "/StreamingAssets/Scientist.json"))
        {
            return null;
        }
        StreamReader sr = new StreamReader(Application.dataPath + "/StreamingAssets/Scientist.json");
        if (sr == null)
        {
            return null;
        }
        string json = sr.ReadToEnd();
        if (json.Length > 0)
        {
            return JsonUtility.FromJson<SciDataMainJson>(json);
        }
        return null;
    }
    public static void GetSciData()
    {
        sciD = LoadSciDataJson();
        sciNum = sciD.SciNum;
        sciData = sciD.SciData;
    }
    public static string GetSciName(int id)
    {
        return sciData[id].Name;
    }
    public static string GetSciIcon(int id)
    {
        return sciData[id].Img;
    }
    public static string GetSciSkill(int id)
    {
        return sciData[id].Skill;
    }
    public static int GetSciSubject(int id)
    {
        return sciData[id].Subject;
    }
    public static int GetSciLimit(int id)
    {
        return sciData[id].Limit;
    }
}
