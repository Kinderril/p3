using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class SummonnerBehaviour : MonoBehaviour
{
    private float nextActivateTime = 0;
    private float destroyTime = 0;
    private int chargesRemain = 0;
    private BaseSummon BaseSummon;
    private SpellInGame SpellInGame;
    private Unit Owner;

    public void Init(Unit owner, SpellInGame spellInGame)
    {
        Owner = owner;
        SpellInGame = spellInGame;
        BaseSummon = SpellInGame.sourseItem.SpellData.BaseSummon;
        chargesRemain = BaseSummon.ShootCount;
        destroyTime = BaseSummon.LIFE_TIME_SEC + Time.time;


    }

    void Update()
    {
        if (!CheckDestroy())
        {
            TryActivate();
        }
    }

    private bool CheckDestroy()
    {
        if (destroyTime < Time.time)
        {
            Dispose();
            return true;
        }
        return false;
    }

    private void TryActivate()
    {
        if (chargesRemain > 0)
        {
            if (nextActivateTime < Time.time)
            {
                Activate();
            }
        }
    }

    private void Activate()
    {
        nextActivateTime = BaseSummon.DelayShoot;
        SpellInGame.ActivateBySummon(this);
    }

    public void Dispose()
    {
        Destroy(gameObject);
    }

    public void ActivationFine()
    {
        chargesRemain--;
        if (chargesRemain <= 0)
        {
            Dispose();
        }
    }
}

