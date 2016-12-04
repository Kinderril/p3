using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public enum CounterSide
{
    up,
    down,
}

public class ChangingCounter : MonoBehaviour
{
    private const int MS = 1000;
    private int TOTAL_CHANGE_TIME_MS = 1 * MS;//2sec
    private int FPS = 30;


    private Text label;
    private int targetValue;
    private int curValue = 0;
    private bool shallCalc;
    private CounterSide counterSide;
    public const float SHAKE_TIME = 1;
    public const float SHAKE_STR = 0.3f;
    private int MAX_CHANGES;// = FPS * TOTAL_CHANGE_TIME_MS / MS;
    private int MIN_STEP_TIME;// = MS / FPS;
    private int curChangeTime;
    private bool isUpdating = false;
    private float timeLast;

    private int totalChanges = 0;
    private int[] changesCounts;
    private int[] currentValues;
    private int[] targetValues;
    private float[] startTimes;
    private float[] deltaChangeTime;
    private Action<int> workAction;

    public void Init(int value = 0, int TOTAL_CHANGE_TIME_MS = 1000, int FPS = 30)
    {
        this.TOTAL_CHANGE_TIME_MS = TOTAL_CHANGE_TIME_MS;
        this.FPS = FPS;
        MAX_CHANGES = FPS * TOTAL_CHANGE_TIME_MS / MS;
        MIN_STEP_TIME = MS / FPS;
        
        label = GetComponent<Text>();
        label.text = value.ToString();
        curValue = value;
    }

    private IEnumerator STest()//TODO test function
    {
        yield return new WaitForSeconds(2f);
        Init();
        curValue = 382;
        ChangeTo(429);
    }

    public static int[] DigitArr(string n)
    {
        var chars = n.ToCharArray();
        int[] arr = new int[chars.Length];
        for (int i = 0; i < chars.Length; i++)
        {
            arr[i] = Convert.ToInt32(chars[i].ToString());
        }
        return arr;
    }

    public void ChangeTo(int val)
    {
        targetValue = val;
        shallCalc = targetValue != curValue;

        if (shallCalc)
        {
            if (targetValue > curValue)
            {
                counterSide = CounterSide.up;
                workAction = workActionUp;
            }
            else
            {
                counterSide = CounterSide.down;
                workAction = workActionDown;
            }

            var curStr = curValue.ToString();
            var targetStr = targetValue.ToString();

            var targetDigitsCount = targetStr.Length;
            var curDigitsCount = curStr.Length;

            if (targetDigitsCount > curDigitsCount)
            {
                var diff = targetDigitsCount - curDigitsCount;
                for (int i = 0; i < diff; i++)
                {
                    curStr = "0" + curStr;
                }
            }
            else if (targetDigitsCount < curDigitsCount)
            {
                var diff = curDigitsCount - targetDigitsCount;
                for (int i = 0; i < diff; i++)
                {
                    targetStr = "0" + targetStr;
                }
            }

            currentValues = DigitArr(curStr);
            targetValues = DigitArr(targetStr);

            changesCounts = new int[Mathf.Max(targetDigitsCount, curDigitsCount)];
            startTimes = new float[changesCounts.Length];
            deltaChangeTime = new float[changesCounts.Length];
            for (int i = changesCounts.Length - 1; i >= 0; i--)
            {
                changesCounts[i] = CalcStepsCount(currentValues[i], targetValues[i]);
            }
            totalChanges = 0;
            int[] temp = new int[changesCounts.Length];
            for (int i = 0; i < changesCounts.Length; i++)
            {
                var c = changesCounts[i];

                for (int j = 0; j < i; j++)
                {
                    int t = 0;
                    bool upstage = changesCounts[j + 1] + currentValues[j + 1] >= 10;
                    int offset = 0;
                    if (upstage)
                    {
                        offset = -1;
                    }
                    t = Mathf.Abs(changesCounts[j] + offset) * (int)Math.Pow(10, (i - j));
                    c += t;

                }
                if (c > MAX_CHANGES)
                {
                    startTimes[i] = 0;
                    c = MAX_CHANGES + changesCounts[i];
                    deltaChangeTime[i] = (float)MIN_STEP_TIME / (float)MS;
                }
                else
                {
                    if (c > 0 && counterSide == CounterSide.up)
                    {
                        var div = (float)TOTAL_CHANGE_TIME_MS / (float)c;
                        deltaChangeTime[i] = div / (float)MS;
                        startTimes[i] = deltaChangeTime[i];
                    }
                    else
                    {
                        deltaChangeTime[i] = 0;
                        startTimes[i] = 0;
                    }
                }
                totalChanges += c;
                temp[i] = c;
            }
            float prevZero = 0f;
            for (int i = changesCounts.Length - 1; i >= 0; i--)
            {
                var c = temp[i];
                float nextZero = 0;
                if (c <= MAX_CHANGES && c > 0)
                {
                    if (c > 0 && counterSide == CounterSide.up)
                    {
                        int stepsToZero = 9 - currentValues[i];
                        nextZero = stepsToZero * (deltaChangeTime[i]);
                        startTimes[i] = prevZero;
                    }
                }
                prevZero = prevZero + nextZero;
            }
            changesCounts = temp;
            isUpdating = true;
            timeLast = 0;
        }
    }
    

    void Update()
    {
        if (isUpdating)
        {
            bool haveChanges = false;
            timeLast += Time.deltaTime;
            for (int i = 0; i < startTimes.Length; i++)
            {
                if (startTimes[i] < timeLast)
                {
                    if (changesCounts[i] > 0)
                    {
                        haveChanges = true;
                        changesCounts[i]--;
                        workAction(i);
                        startTimes[i] += deltaChangeTime[i];
                        totalChanges--;
                        if (totalChanges <= 0)
                        {
                            isUpdating = false;
                        }
                    }
                }
            }
            if (haveChanges)
            {
                UpdateToField();
            }
        }
    }

    private int CalcStepsCount(int cur, int trg)
    {
        int c = 0;
        int diff = Mathf.Abs(cur - trg);
        if (diff > 0)
        {
            switch (counterSide)
            {
                case CounterSide.up:

                    if (trg > cur)
                    {
                        c = diff;
                    }
                    else
                    {
                        c = 10 - diff;
                    }
                    break;
                case CounterSide.down:
                    if (cur > trg)
                    {
                        c = diff;
                    }
                    else
                    {
                        c = 10 - diff;
                    }
                    break;
            }
        }
        return c;
    }

    private void UpdateToField()
    {
        string ss = "";
        for (int i = 0; i < currentValues.Length; i++)
        {
            ss += currentValues[i];
        }

        label.text = ss;
        if (label.text.Length > 1)
        {
            label.text = label.text.TrimStart('0');
        }
        curValue = Convert.ToInt32(ss);
    }

    private void workActionUp(int index)
    {
        var val = currentValues[index];
        val++;
        if (val > 9)
        {
            val = 0;
        }
        currentValues[index] = val;

    }
    private void workActionDown(int index)
    {
        var val = currentValues[index];
        val--;
        if (val < 0)
        {
            val = 9;
        }
        currentValues[index] = val;

    }

    private void DebugLog()
    {
        string ss = "";
        for (int i = 0; i < currentValues.Length; i++)
        {
            ss += currentValues[i];
        }
//        Logger.Log(SuperCity.LogType.Credits, ss);
        ss = "";
        for (int i = 0; i < targetValues.Length; i++)
        {
            ss += targetValues[i];
        }
//        Logger.Log(SuperCity.LogType.Credits, ss);
        ss = "";
        for (int i = 0; i < changesCounts.Length; i++)
        {
            ss += " " + changesCounts[i];
        }

//        Logger.Log(SuperCity.LogType.Credits, "Changes counts:" + ss);
        //        ss = "";
        //        for (int i = 0; i < temp.Length; i++)
        //        {
        //            ss += " " + temp[i];
        //        }
        //        Logger.Log(SCLogType.Credits, "temp counts:" + ss);
        ss = "";
        for (int i = 0; i < startTimes.Length; i++)
        {
            ss += " " + startTimes[i];
        }
//        Logger.Log(SuperCity.LogType.Credits, "startTimes counts:" + ss);
        ss = "";
        for (int i = 0; i < deltaChangeTime.Length; i++)
        {
            ss += " " + deltaChangeTime[i];
        }
//        Logger.Log(SuperCity.LogType.Credits, "deltaChangeTime counts:" + ss);
//        Logger.Log(SuperCity.LogType.Credits, "total counts:" + totalChanges);
        //        Logger.Log(SCLogType.Credits, changesCounts.Length + "   " + targetValues.Length);
    }
}

