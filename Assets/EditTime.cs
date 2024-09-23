using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class EditTime : MonoBehaviour
{
    [SerializeField] private Clocks clocks;
    [SerializeField] private GameObject[] arrows;
    [SerializeField] private Text[] timeTexts;
    [SerializeField] private InputField[] fields;
    [SerializeField] private Text buttonName;
    private bool editing;

    public void Edit()
    {
        if (!editing)
        {
            Set(true, "Apply changes");
        }
        else
        {
            Set(false, "Edit time");
        }
    }

    public void ShowTime(ClockTime time)
    {
        SetField(0, time.hours);
        SetField(1, time.minutes);
        SetField(2, time.seconds);
    }

    private void SetField(int index, int time)
    {
        if(time < 10)
            fields[index].SetTextWithoutNotify("0" + time.ToString());
        else
            fields[index].SetTextWithoutNotify(time.ToString());
    }

    public void SetSeconds(string time)
    {
        clocks.ChangeTimeByInput(int.Parse(time), 2);
    }
    public void SetMinutes(string time)
    {
        clocks.ChangeTimeByInput(int.Parse(time), 1);
    }
    public void SetHours(string time)
    {
        clocks.ChangeTimeByInput(int.Parse(time), 0);
    }

    private void Set(bool state, string text)
    {
        editing = state;
        clocks.CountingTime = !state;
        foreach (var a in arrows)
            a.SetActive(state);
        buttonName.text = text;

        for(int i = 0; i < fields.Length; i++)
        {
            fields[i].gameObject.SetActive(state);
            timeTexts[i].gameObject.SetActive(!state);
            fields[i].SetTextWithoutNotify(timeTexts[i].text);
        }
    }
}
