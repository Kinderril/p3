using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum TutorialPart
{
    start,
    move,
    shoot,
    aftershot,
    cast,
    aftercast,
    take,
    boss,
}

public class TutorialLevel : MonoBehaviour
{
    private int killedWithWeapon;
    private int killedWithSpell;
    private bool isGoldCollected = false;
    private bool isBonusTaken = false;
    private Hero hero;

    public List<BaseMonster> Monsters = new List<BaseMonster>(); 
    public Gate GateWeapons;
    public Gate GateSpells;
    public Gate GateMisc;
    public Transform TriggersParent;

    private LevelObject map;
    private bool isEnergyRegen = false;
    private bool isHealthRegen = false;
    private float nextGiveTimeHealth = 0;
    private float nextGiveTimeEnergy = 0;
    private TutorialPart tutorialPart;

    public event Action<TutorialPart> OnTutorialChange;
    public event Action OnManecenKilled;

    public TutorialPart TutorialPart
    {
        set
        {
            if (value <= tutorialPart)
                return;

            tutorialPart = value;
            if (OnTutorialChange != null)
            {
                OnTutorialChange(tutorialPart);
            }
            isEnergyRegen = false;
            isHealthRegen = false;
            switch (tutorialPart)
            {
                case TutorialPart.cast:
                    isEnergyRegen = true;
                    break;
                case TutorialPart.boss:
                    isEnergyRegen = true;
                    isHealthRegen = true;
                    Map.Instance.SpawnBoss(0);
                    break;
            }
        }
    }

    void Awake()
    {
        map = GetComponent<LevelObject>();
        map.OnInited += OnInited;
        foreach (Transform tr in TriggersParent)
        {
            var t = tr.GetComponent<TutorStateChanger>();
            if (t != null)
            {
                t.Init(this);
            }
        }
        foreach (var baseMonster in Monsters)
        {
            baseMonster.enabled = false;
        }
    }

    public void BossEnter()
    {
        isHealthRegen = true;
    }

    public string StatusMessage()
    {
        string ss = "";
        switch (tutorialPart)
        {
            case TutorialPart.shoot:
                ss = killedWithWeapon + "/2";
                break;
                case TutorialPart.cast:
                ss = killedWithSpell + "/2";
                break;
        }
        return ss;
    }

    private void OnInited(Hero obj)
    {
        hero = obj;
        map.OnInited -= OnInited;
        MainController.Instance.level.OnItemCollected += OnItemCollected;
        Map.Instance.enemies = Monsters;
    }

    private void OnItemCollected(ItemId arg1, float arg2, float arg3)
    {
        if (arg1 == ItemId.money)
        {
            GoldCollected();
        }
    }

    public void ManecenKilledWeapon()
    {
        killedWithWeapon++;
        if (OnManecenKilled != null)
        {
            OnManecenKilled();
        }
        if (killedWithWeapon >= 2)
        {
            GateWeapons.Open();
            TutorialPart = TutorialPart.aftershot;
        }
    }


    private void Update()
    {
        if (nextGiveTimeEnergy < Time.time)
        {
            nextGiveTimeEnergy = Time.time + 5f;
            if (isEnergyRegen)
            {
//                if (!MainController.Instance.level.Energy.IsFull)
//                {
                hero.GetItems(ItemId.energy, -50);
//                }
            }
        }
        if (isHealthRegen)
        {
            if (nextGiveTimeHealth < Time.time)
            {
                nextGiveTimeHealth = Time.time + 0.5f;
                if (hero.CurHp < hero.Parameters[ParamType.Heath] * 0.8f)
                {
                    hero.GetHeal(9000);
                }
            }
        }
    }

    public void ManecenKilledSpell()
    {
        if (OnManecenKilled != null)
        {
            OnManecenKilled();
        }
        killedWithSpell++;
        if (killedWithSpell >= 2)
        {
            GateSpells.Open();
            TutorialPart = TutorialPart.aftercast;
        }
    }

    public void GoldCollected()
    {
        isGoldCollected = true;
        if (isGoldCollected)
        {
            GateMisc.Open();
        }
    }

    public void BonusCollected()
    {
        isBonusTaken = true;
        if (isGoldCollected && isBonusTaken)
        {
            GateMisc.Open();
        }
    }

    void OnDestroy()
    {
        OnTutorialChange = null;
        OnManecenKilled = null;
        MainController.Instance.level.OnItemCollected -= OnItemCollected;
    }
}

