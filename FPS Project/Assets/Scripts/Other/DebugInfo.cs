using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugInfo : MonoBehaviour
{
    public TextMeshProUGUI textMesh;

    float average;
    float current;
    float min;
    float max;

    int total5;
    float min5;
    float max5;
    float secondsLeft = 5f;



    void UpdateText()
    {
        textMesh.text = $"<mspace=25>FPS: {current}\nAvg: {average}\nMin: {min}\nMax: {max}";
    }



    private void Update()
    {
        if (secondsLeft < 0f)
        {
            secondsLeft = 2f;
            average = (float) Math.Round(total5 / 2f, 4);
            min = (float)Math.Round(min5, 2);
            max = (float)Math.Round(max5, 2);

            total5 = 0;
            min5 = 1000f;
            max5 = 0f;
        }

        float frameTime = 1 / Time.unscaledDeltaTime;
        current = (float)Math.Round(frameTime, 4); ;

        secondsLeft -= Time.unscaledDeltaTime;

        total5 ++;

        if (max5 < frameTime)
        {
            max5 = frameTime;
        }

        if (min5 > frameTime)
        {
            min5 = frameTime;
        }

        UpdateText();
    }
}
