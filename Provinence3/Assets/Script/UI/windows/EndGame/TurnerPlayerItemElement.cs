using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class TurnerPlayerItemElement : MonoBehaviour
{
    private bool isTurn = false;
    private Animator animator;
    private string key = "Turn";

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OnClick()
    {
        if (!isTurn)
        {
            DoTurn();
        }
    }

    private void DoTurn()
    {
        isTurn = true;
        animator.SetTrigger(key);
    }

    public void EndAnimation()
    {
        Destroy(gameObject);
    }
}

