using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class UIDamageNumber : MonoBehaviour
{
    public TMPro.TextMeshProUGUI damageText;
    public Color damageColor;
    public Color healingColor;
    public void SetNumber(float number)
    {
        string numberString = number.ToString();
        if ((int)(Math.Round(number, 1) * 10.0) % 10 == 0)
        {
            numberString = Math.Round(number, 0).ToString("f0");
        }
        else
        {
            numberString = Math.Round(number, 1).ToString("f1");
        }

        damageText.text = "+" + numberString;
        damageText.color = healingColor;

        if (number < 0)
        {
            damageText.text = numberString;
            damageText.color = damageColor;
        }
    }
}
