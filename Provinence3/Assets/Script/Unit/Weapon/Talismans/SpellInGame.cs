using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class SpellInGame : IBulletHolder
{
    protected const float LVL_1_AV_MONSTER_HP = 100;
    protected const float LVL_10_AV_MONSTER_HP = 330;

    protected const string base_path = "prefabs/talisman/";
    public SpellItem sourseItem;
    public float currentEnergy;
    public Action<bool,float,int> OnReady;
    protected Hero hero;
    private bool isUnderCooldown = false;
    private TimerManager.ITimer timer;
    private int currentCharges = 0;
    private float max;
    protected float power;
    private AbsorberWithPosition CastEffect;
    private TriggerInGame TriggerInGame;
    private SummonInGame SummonInGame;
    private Unit owner;
//    private Level level;

    public virtual string PowerInfo()
    {
        return power.ToString();
    }


    public virtual void Init(Level level, SpellItem sourseItem, int countTalismans,Unit owner)
    {
        this.sourseItem = sourseItem;
        this.owner = owner;
//        this.level = level;
        max = sourseItem.SpellData.Cost * sourseItem.SpellData.Charges;
        if (level != null)
        {
            hero = level.MainHero;
            level.OnItemCollected += (id, f, delta) =>
            {
                if (id == ItemId.energy)
                {
                    AddEnergy(delta/countTalismans);
                }
            };
//            var db = DataBaseController.Instance;
            var absorber = VisualEffectSetter.CastEffects[sourseItem.SpellData.IdCast];


            if (absorber != null)
            {
                CastEffect = DataBaseController.GetItem<AbsorberWithPosition>(absorber);
                CastEffect.transform.SetParent(hero.transform,false);
            }
        }
        if (DebugController.Instance.ALL_TALISMAN_CHARGED)
        {
            currentEnergy = max;
        }
    }

    public void Dispose()
    {
        if (TriggerInGame != null)
        {
            TriggerInGame.Dispose();
            TriggerInGame = null;
        }
        if (timer != null)
        {
            timer.Stop();
            timer = null;
        }
    }
    
    public BaseMonster GetClosestMonster()
    {
        return Map.Instance.FindClosesEnemy(hero.transform.position);
    }

    public void UseIfCan()
    {
        if (CanUse())
        {
            Unit p = null;
            if (FindTarget(out p))
            {
                Use(p);
            }
        }
    }

    private void UseWithUnit()
    {
        Unit p = null;
        if (FindTarget(out p))
        {
            Use(p);
        }
    }

    private bool FindTarget(out Unit p)
    {
        p = null;
        var haveTrg = true;
        if (sourseItem.SpellData.StartType == SpellTargetType.ClosestsEnemy
          || sourseItem.SpellData.TargetType == SpellTargetType.ClosestsEnemy)
        {
            if (owner is Hero)
            {
                p = Map.Instance.FindClosesEnemy(owner.transform.position, 40);
                if (p == null)
                {
                    haveTrg = false;

                }
            }
            else
            {
                p = MainController.Instance.level.MainHero;
            }
        }
        if (sourseItem.SpellData.TargetType == SpellTargetType.Self)
        {
            p = owner;
        }
        return haveTrg;
    }

    protected virtual void Use(Unit unit)
    {
        isUnderCooldown = true;
        timer = MainController.Instance.TimerManager.MakeTimer(TimeSpan.FromMilliseconds(1500));
        timer.OnTimer += OnTimerCome;
        AddEnergy(sourseItem.SpellData.Cost,true);
        if (CastEffect != null)
        {
            CastEffect.SetAndPlay(hero);
        }
        switch (sourseItem.SpellData.SpellCoreType)
        {
            case SpellCoreType.Shoot:
                LaunchBullets(unit);
                break;
            case SpellCoreType.Summon:
                ActivateSummon();
                break;
            case SpellCoreType.Trigger:
                ActivateTrigger();
                break;
        }
        DoCallback();
        owner.UseSpellCallback(this);
    }

    private void ActivateTrigger()
    {
        if (TriggerInGame == null)
        {
            TriggerInGame = new TriggerInGame(this, owner);
        }
        TriggerInGame.Use();
    }

    private void ActivateSummon()
    {
        if (SummonInGame == null)
        {
            SummonInGame = new SummonInGame(this, owner);
        }
        SummonInGame.Use();
    }

    public void ActivateBySummon(SummonnerBehaviour summonnerBehaviour)
    {
        Unit trg;
        if (FindTarget(out trg))
        {
            summonnerBehaviour.ActivationFine();
            LaunchBullets(trg, summonnerBehaviour);
        }
    }

    public void ActivateByTrigger()
    {
        Unit p;
        if (FindTarget(out p))
        {
            LaunchBullets(p);
        }
    }

    
    private void LaunchBullets(Unit trg,SummonnerBehaviour summoner = null)
    {
        var bulletData = sourseItem.SpellData.Bullet;
        Bullet bulletPrefab;
        switch (bulletData.BulletColliderType)
        {
            case BulletColliderType.box:
                bulletPrefab = DataBaseController.Instance.DataStructs.BaseBulletAOECube;
                break;
            case BulletColliderType.sphrere:
                bulletPrefab = DataBaseController.Instance.DataStructs.BaseBulletAOESphere;
                break;
            default:
            case BulletColliderType.noOne:
                bulletPrefab = DataBaseController.Instance.DataStructs.BaseBullet;
                break;
        }

        var bullet = DataBaseController.GetItem<Bullet>(bulletPrefab);
        switch (bulletData.BulletColliderType)
        {

            case BulletColliderType.sphrere:
                var sCollider = bullet.Collider as SphereCollider;
                if (sCollider != null)
                {
                    sCollider.radius = bulletData.ColliderSize.magnitude;
                }
                break;
            case BulletColliderType.box:
                var bCollider = bullet.Collider as BoxCollider;
                if (bCollider != null)
                {
                    bCollider.size = bulletData.ColliderSize;
                }
                break;
        }


        bullet.transform.SetParent(Map.Instance.bulletContainer,true);
        Vector3 startPos = Vector3.zero;
        Vector3 endPos = Vector3.zero;
        if (summoner == null)
        {
            switch (sourseItem.SpellData.StartType)
            {
                case SpellTargetType.Self:
                    startPos = owner.transform.position;
                    break;
                case SpellTargetType.ClosestsEnemy:
                    startPos = trg.transform.position;
                    break;
                case SpellTargetType.LookDirection:
                    startPos = owner.transform.position;
                    break;
            }
        }
        else
        {
            startPos = summoner.transform.position; 
        }
        switch (sourseItem.SpellData.TargetType)
        {
            case SpellTargetType.Self:
                endPos = owner.transform.position;
                break;
            case SpellTargetType.ClosestsEnemy:
                endPos = trg.transform.position;
                break;
            case SpellTargetType.LookDirection:
                endPos = owner.transform.position;
                break;
        }
        if (sourseItem.SpellData.Bullet.BaseBulletTarget == BaseBulletTarget.target)
        {
            bullet.Init(endPos - startPos, this);
        }
        else
        {
            bullet.Init(trg, this, startPos);
        }
        var effect = DataBaseController.GetItem<BaseEffectAbsorber>(VisualEffectSetter.BulletEffects[bulletData.IdVisual]);
        effect.transform.SetParent(bullet.transform, false);
        effect.transform.localPosition = Vector3.zero;


    }

    private void OnTimerCome()
    {
        isUnderCooldown = false;
        if (CastEffect != null)
        {
            CastEffect.Stop();
        }
        DoCallback();
        timer = null;
    }

    public void AddEnergy(float val, bool canBePositive = false)
    {
        if (canBePositive || val < 0)
        {
            currentEnergy = Mathf.Clamp(currentEnergy - val, 0, max);
            DoCallback();
        }
    }

    private void DoCallback()
    {
        if (OnReady != null)
        {
            float percent = currentEnergy/(float) sourseItem.SpellData.Cost;
            currentCharges = (int)percent;
            if (percent > 1)
            {
                percent -= currentCharges;
            }
            
            OnReady(CanUse(), percent, currentCharges);
        }
    }

    public bool CanUse()
    {
        var b1 = currentEnergy >= sourseItem.SpellData.Cost && !isUnderCooldown;

        return b1;
    }

    public static SpellInGame Creat(SpellItem spellItem, int countTalismans, Level level,Unit owner)
    {
        var sp = new SpellInGame();
        sp.Init(level,spellItem,countTalismans,owner);

        owner.AddSpell(sp);
        return sp;
    }

    public SpecialAbility SpecAbility
    { get {return  SpecialAbility.none;} }
    public float Power { get { return -1f; } }
    public float Range { get { return sourseItem.SpellData.Bullet.LifeTime; } }//TODO calc
    public Unit Owner { get { return owner; } }
    public WeaponType DamageType { get { return WeaponType.magic;} }
    public Transform BulletComeOut {
        get { return owner.weaponsContainer; }
    }

    public Transform Transform
    {
        get { return owner.transform; }
    }

    public Vector3 FindStartPosition(Bullet bullet)
    {
        if (BulletComeOut != null)
        {
            return BulletComeOut.position;
        }
        return Transform.position;
    }

    public Vector3 FindTrgPosition(Vector3 direction, Vector3 start)
    {

        return direction.normalized * Range + start;
    }

}

