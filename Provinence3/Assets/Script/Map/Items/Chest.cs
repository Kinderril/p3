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

    public void Init(bool withCrystal, Level lvl)
    {
        float m_GroundCheckDistance = 9999f;
        animator = GetComponent<Animator>();
        Utils.GroundTransform(transform, m_GroundCheckDistance);
        Utils.SetRandomRotation(transform);
        var p = lvl.Penalty;
        var rnd = (int)(Formuls.GoldInChest(lvl.difficult) * p);
        items.Add(ItemId.money, GreatRandom.RandomizeValue(rnd));
        if (withCrystal && p > 0.99f)
            items.Add(ItemId.crystal, 1);
        if (SMUtils.Range(0, 100) < 50)
        {
            items.Add(ItemId.ammo, SMUtils.Range(5,15));
        }

    }

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
            GoldMapItem prefab;
            switch (item.Key)
            {
                case ItemId.crystal:
                    prefab = DataBaseController.Instance.GrystalMapItemPrefab;
                    break;
                case ItemId.ammo:
                    var prefab2 = DataBaseController.Instance.AmmoMapItem;
                    var ammo = DataBaseController.GetItem<AmmoMapItem>(prefab2,transform.position);
                    ammo.Init(ItemId.ammo, 10, true);
                    SubInitMapItem(ammo);
                    return;
                    break;
                default:
                    prefab = DataBaseController.Instance.GoldMapItemPrefab;
                    break;
            }
            var mo = DataBaseController.GetItem<GoldMapItem>(prefab,
                transform.position);

            mo.Init(item.Key, item.Value,true);
            SubInitMapItem(mo);
        }
    }

    private void SubInitMapItem(BaseMapItem item)
    {
        item.transform.SetParent(Map.Instance.miscContainer, true);
        item.StartFly(transform);
    }

    private void PlayOpen()
    {
        isOpen = true;
        if (animator != null)
        {
            animator.SetBool("isOpen",true);
        }
    }

}

