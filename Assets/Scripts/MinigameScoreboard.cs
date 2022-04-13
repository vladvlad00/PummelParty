using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class MinigameScoreboard{
    private TextMeshProUGUI[] text_array;
    private int[] scores;
    private Action finish;

    public MinigameScoreboard(TextMeshProUGUI[] text_array, int[] scores, Action finish){
        this.text_array = text_array;
        this.scores = scores;
        this.finish = finish;
    }
    public void Update(int[] scores,bool done){
        if (done){
            finish();
            return;
        }
        for (int i=0; i<text_array.Length; i++){
            text_array[i].text = scores[i].ToString();
        }

    }
}