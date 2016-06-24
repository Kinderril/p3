using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class MonsterInfo : MonoBehaviour
{
    private TimeSpan span = TimeSpan.FromMilliseconds(1800);
    private TimerManager.ITimer timer;
    public Animator animator;
    private const string KEY_OPEN = "open";
    private const string KEY_CLOSE = "close";
    private bool isOpen = false;
    public KeyCatcher KeyCatcher;
    public Slider MonsterSliderHP;

    public void Init()
    {
        KeyCatcher.Init(EndAnimationKey, EndStartAnim);
        if (timer != null)
        {
            timer.Stop();
        }
        EndAnimationKey();
        GlobalEventManager.Instance.OnMonsterGetHit += OnMonsterGetHit;
    }

    private void EndStartAnim()
    {
        
    }


    private void OnMonsterGetHit(BaseMonster obj)
    {
        MonsterSliderHP.gameObject.SetActive(true);
        if (animator != null && !isOpen)
        {
            animator.SetTrigger(KEY_OPEN);
//            Debug.Log("SetTrigger(KEY_OPEN)");
            isOpen = true;
        }
        MonsterSliderHP.value = obj.CurHp / obj.Parameters.Parameters[ParamType.Heath];
        if (timer != null)
        {
            timer.Stop();
        }
        timer = MainController.Instance.TimerManager.MakeTimer(span);
        timer.OnTimer += OnTimer;
    }

    private void OnTimer()
    {
        StopAndOff();
    }

    public void EndAnimationKey()
    {
//        Debug.Log("EndAnimationKey");
        isOpen = false;
        MonsterSliderHP.gameObject.SetActive(false);
    }

    private void StopAndOff()
    {
        if (timer != null)
        {
            timer.Stop();
        }
        if (animator == null)
        {
            EndAnimationKey();
        }
        else
        {
            if (isOpen)
            {
                animator.SetTrigger(KEY_CLOSE);
            }
        }
    }

    public void DeInit()
    {
        StopAndOff();
        GlobalEventManager.Instance.OnMonsterGetHit -= OnMonsterGetHit;
    }
}

