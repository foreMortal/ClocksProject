using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class Clocks : MonoBehaviour
{
    [SerializeField] private Transform[] arrows;
    [SerializeField] private Text[] timeTexts;

    private float[] timeVals = new float[4] { 0f, 0f, 0f, 0f};
    private float[] maxValues = new float[] { 24f, 60f, 60f, 1f};
    public bool CountingTime { get; set; }


    public void ReceiveTime(ClockTime time)
    {
        timeVals[0] = time.hours;
        timeVals[1] = time.minutes;
        timeVals[2] = time.seconds;
        timeVals[3] = 0f;

        UpdateClocks();
        MoveArrows();

        CountingTime = true;
    }

    public void ChangeTimeByInput(int value, int index)
    {
        timeVals[index] = value;
        UpdateClocks();
        MoveArrows();
    }

    private void Update()
    {
        if (CountingTime)
        {
            IncreaseTime(3, Time.deltaTime);
            MoveArrows();
            UpdateClocks();
        }
    }

    public ClockTime ChangeTimeManually(float time)
    {
        ClockTime t;
        if (time > 0)
            t = IncreaseTime(3, time);
        else
            t = DecreaseTime(3, time);

        MoveArrows();
        UpdateClocks();

        return t;
    }

    private void MoveArrows()
    {
        for(int i = 0; i < arrows.Length; i++)
        {
            float max = 0;
            if (i == 0)
                max = 60 * 60 * 24;
            else if (i == 1)
                max = 60 * 60;
            else
                max = 60;

            max *= 1000f;

            float delta = 360f / max;

            float nextPos = AnythyngToMilliSeconds(i) * delta;

            arrows[i].rotation = Quaternion.Euler(0f, 0f, -nextPos);

            //arrows[i].DORotate(new Vector3(0f, 0f, -nextPos), 0.1f);
        }
    }

    private ClockTime IncreaseTime(int index, float delta)
    {
        timeVals[index] += delta;

        while (timeVals[index] >= maxValues[index])
        {
            timeVals[index] -= maxValues[index];

            if(index != 0)
                IncreaseTime(index - 1, 1);
        }

        
        ClockTime t = new ClockTime
        {
            hours = GetIntFromFloat(timeVals[0]),
            minutes = GetIntFromFloat(timeVals[1]),
            seconds = GetIntFromFloat(timeVals[2]),
        };
        return t;
    }

    private ClockTime DecreaseTime(int index, float delta)
    {
        timeVals[index] += delta;

        while (timeVals[index] < 0f)
        {
            timeVals[index] += maxValues[index];

            if (index != 0)
                DecreaseTime(index - 1, -1);
        }

        ClockTime t = new ClockTime
        {
            hours = GetIntFromFloat(timeVals[0]),
            minutes = GetIntFromFloat(timeVals[1]),
            seconds = GetIntFromFloat(timeVals[2]),
        };
        return t;
    }

    private void UpdateClocks()
    {
        for(int i = 0; i < timeTexts.Length; i++)
        {
            if (timeVals[i] < 10)
                timeTexts[i].text = "0" + GetIntFromFloat(timeVals[i]);
            else
                timeTexts[i].text = GetIntFromFloat(timeVals[i]).ToString();
        }
    }

    private int GetIntFromFloat(float num)
    {
        float t = num % 1;

        return (int)(num - t);
    }

    private float AnythyngToMilliSeconds(int index)
    {
        float res = 0f;

        for(int i = index; i < timeVals.Length - 1; i++)
        {
            res += timeVals[i] * Mathf.Pow(60f, 2 - i);
        }

        res *= 1000f;
        res += timeVals[^1] * 1000f;

        return res;
    }

}   

public struct ClockTime
{
    public int hours;
    public int minutes;
    public int seconds;
}
