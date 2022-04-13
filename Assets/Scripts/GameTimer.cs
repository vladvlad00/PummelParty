using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameTimer
{
    private TextMeshProUGUI text;
    private float remainingTime;
    private Action finish;
    private bool done = false;

    public GameTimer(float remainingTime, TextMeshProUGUI text, Action finish)
    {
        this.remainingTime = remainingTime;
        this.text = text;
        this.finish = finish;
    }
    public void Update()
    {
        if (done)
            return;
        remainingTime -= Time.deltaTime;
        text.text = remainingTime.ToString("0.00") + "s";
        if (remainingTime < 0)
        {
            done = true;
            finish();
        }
    }
}
