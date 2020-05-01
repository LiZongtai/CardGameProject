using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModel
{
    // user info
    public UserDto userDto { get; set; }
    public int playNum { get; set; }
    public static Color colorZero = new Color(1, 1, 1, 0);
    public static Color color0 = new Color((float)0.9215686, (float)0.3803922, (float)0, 1);
    public static Color color1 = new Color((float)0.2470588, (float)0.282353, (float)0.8, 1);
    public static Color color2 = new Color((float)0.7215686, (float)0.2392157, (float)0.7294118, 1);
    public static Color color3 = new Color((float)0.1137255, (float)0.4196078, (float)0.2, 1);
    public static Color Subject2Color(int subject)
    {
        switch (subject)
        {
            case 0:
                return color0;
            case 1:
                return color1;
            case 2:
                return color2;
            case 3:
                return color3;
        }
        return colorZero;
    }
}
