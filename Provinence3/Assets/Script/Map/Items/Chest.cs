using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityStandardAssets.Effects;


public class Chest : MonoBehaviour
{
//    public const int moneyCoef = 80; 
    public Dictionary<ItemId, int> items = new Dictionary<ItemId, int>();
    private bool isOpen = false;
    public ParticleSystemMultiplier SystemMultiplier;
    private Animator animator;

    void OnTriggerEnter(Collider other)
    {
        if (!isOpen)
        {
            var unit = other.GetComponent<Hero>();
            if (unit != null)
            {
                PlayOpen();
                if (SystemMultiplier != null)
                    SystemMultiplier.StartPlay();
            }
        }
    }

    public void Release()//Name from event
    {
        ReleaseItems();
    }
    private void ReleaseItems()
    {
        foreach (var item in items)
        {
            var mo = DataBaseController.GetItem<GoldMapItem>(DataBaseController.Instance.GoldMapItemPrefab,
                transform.position);
            mo.Init(item.Key, item.Value);
            mo.transform.SetParent(Map.Instance.miscContainer,true);
            mo.StartFly(transform);
        }
    }

    private void PlayOpen()
    {
        isOpen = true;
        if (animator != null)
        {
            animator.SetBool("isOpen",true);
        }
    }

    public void Init(bool withCrystal,Level lvl)
    {
		float m_GroundCheckDistance = 9999f;
        animator = GetComponent<Animator>();
        Utils.GroundTransform(transform, m_GroundCheckDistance);
        Utils.SetRandomRotation(transform);
        var p = lvl.Penalty;
        var rnd = (int)(Formuls.GoldInChest(lvl.difficult) * p); 
        items.Add(ItemId.money,GreatRandom.RandomizeValue(rnd));
        if (withCrystal && p > 0.99f)
            items.Add(ItemId.crystal, 1);

    }
}

