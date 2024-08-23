using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TimeCounter : MonoBehaviour
{
    public Text timeText; //用时文本
    bool isCounting = false; //是否计时
    //private bool isCounting = false;
    private float countTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isCounting)
        {
            countTime += Time.deltaTime;
            timeText.text = TimeFormatter(countTime);
        }
    }

    public void StartTime()
    {
        isCounting = true;
    }

    public void StopTime()
    {
        isCounting = false;
    }

    public void Restart()
    {
        isCounting = true;
        countTime = 0;
    }
    string TimeFormatter(float time)
    {
        int hour = (int)time / 3600;
        int min = (int)(time - hour * 3600) / 60;
        int sec = (int)(time - hour * 3600 - min * 60);
        return hour.ToString("D2") + ":" + min.ToString("D2") + ":" + sec.ToString("D2");
    }
}
