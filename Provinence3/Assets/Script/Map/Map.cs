using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Map : Singleton<Map>
{
    private const string BASE_WAY = "prefabs/level/Level";
    public List<MonsterBornPosition> appearPos;
    public List<BossBornPosition> BossAppearPos;
    private Transform bornPositions;
    public Transform enemiesContainer;
    private BossSpawner bossSpawner;
    public Transform effectsContainer;
    public Transform miscContainer;
    public Transform bulletContainer;
    private Transform heroBornPositions;
    private Transform bossBonusMapElement;
    private Level level;
    public List<BaseMonster> enemies;
    public CameraFollow CameraFollow;
    private BossUnit boss;
    private GameObject levelMainObject;

    public Hero Init(Level lvl, int levelIndex, int heroBornPositionIndex)
    {
        level = lvl;
        LoadLevelGameObject(levelIndex);
        Application.LoadLevelAdditive("Level" + levelIndex);
        heroBornPositions = levelMainObject.transform.Find("BornPos/HeroBornPositions");
        bossBonusMapElement = levelMainObject.transform.Find("BornPos/BossBonusMapElements");
        bornPositions = levelMainObject.transform.Find("BornPos");
        enemiesContainer = transform.Find("Enemies");

        var hero = DataBaseController.GetItem(DataBaseController.Instance.prefabHero, GetHeroBoenPos(heroBornPositionIndex));
        hero.Init();
        enemies = new List<BaseMonster>();
        appearPos = new List<MonsterBornPosition>();
        BossAppearPos = new List<BossBornPosition>();
        List<ChestBornPosition> chestPositions = new List<ChestBornPosition>();

        foreach (Transform bornPosition in bornPositions)
        {
            var bp = bornPosition.GetComponent<BaseBornPosition>();
            if (bp != null)
            {
                switch (bp.GetBornPositionType())
                {
                    case BornPositionType.chest:
                        var cBP = (bp as ChestBornPosition);
                        chestPositions.Add(cBP);
                        break;
                    case BornPositionType.monster:
                        var mBP = (bp as MonsterBornPosition);
                        appearPos.Add(mBP);
                        mBP.Init(this, OnEnemyDead, lvl, hero);
                        break;
                    case BornPositionType.boss:
                        var bBP = (bp as BossBornPosition);
                        BossAppearPos.Add(bBP);
                        bBP.Init(this);
                        break;
                    case BornPositionType.bonus:
                        var bnBP = (bp as BonusBornPosition);
                        bnBP.Init(this,lvl);
                        break;
                }
            }
        }
        var rnd = chestPositions.RandomElement();
        rnd.SetCrystal();
        foreach (var chestBornPosition in chestPositions)
        {
            chestBornPosition.Init(this,lvl);
        }
        CameraFollow.Init(hero.transform);
        bossSpawner = new BossSpawner(enemies.Count,OnSpawnBoss);
        if (bossBonusMapElement != null)
        {
            foreach (Transform tr in bossBonusMapElement)
            {
                var bonusBoss = tr.GetComponent<BossBonusMapElement>();
                bonusBoss.Init(bossSpawner);
            }
        }
//        callback();
        return hero;
    }

    private void LoadLevelGameObject(int levelIndex)
    {
        if (levelMainObject != null)
        {
            Destroy(levelMainObject);
        }
//        Resources.LoadAsync(BASE_WAY + levelIndex, typeof(GameObject));

        var prefab = Resources.Load(BASE_WAY + levelIndex,typeof(GameObject)) as GameObject;
        levelMainObject = GameObject.Instantiate(prefab);
        levelMainObject.transform.SetParent(transform);
    }

    void Update()
    {
        if (level == null || level.MainHero == null)
            return;
        var mainHero = level.MainHero;
        float mainHeroDist;
        bool isActive;
        foreach (var baseMonster in enemies)
        {
            if (!baseMonster.IsDead && !baseMonster.IsDisabled)
            {
                mainHeroDist = (mainHero.transform.position - baseMonster.transform.position).sqrMagnitude;
                isActive = mainHeroDist < BaseMonster.AI_DIST;
                baseMonster.gameObject.SetActive(isActive);
                if (isActive)
                {
                    baseMonster.CheckDistance(mainHeroDist);
                    baseMonster.UpdateByManager();
                }
            }
        }
    }

    private Vector3 GetHeroBoenPos(int index)
    {
        Vector3 vector3s = Vector3.zero;
        var playerData =  MainController.Instance.PlayerData;
       
        foreach (Transform v in heroBornPositions)
        {
            if (vector3s == Vector3.zero)
            {
                vector3s = v.position;
            }

            v.GetComponent<MeshRenderer>().enabled = false;
            var heroBP = v.GetComponent<HeroBornPosition>();
            if (heroBP.ID == index)
            {
                vector3s = v.position;
//                break;
            }
            var opend = playerData.OpenLevels.IsPositionOpen(level.MissionIndex, index);
            Debug.Log(index + " ... " + opend);
            heroBP.Init(this, opend);
//            vector3s = v.position;
        }
        return vector3s;
    }

    private void OnSpawnBoss(int bonuses)
    {
        var pos = BossAppearPos.RandomElement().transform.position;
        var bossPrefab = DataBaseController.Instance.BossUnits.FirstOrDefault(x => x.Parameters.Level == level.difficult);
        if (bossPrefab != null)
        {
            boss = DataBaseController.GetItem<BossUnit>(bossPrefab, pos);
            var hero = MainController.Instance.level.MainHero;
            boss.Init(hero);
            boss.CurHp = boss.CurHp*(1-0.03f*bonuses);
            hero.ArrowTarget.Init(boss);
            boss.transform.SetParent(enemiesContainer);
            MainController.Instance.level.MessageAppear("Boss have appear", Color.red, DataBaseController.Instance.ItemIcon(ItemId.crystal));
            if (level.OnBossAppear != null)
            {
                level.OnBossAppear(boss);
            }
        }
        else
        {
            Debug.LogError("Can't find bossPrefab");
        }
    }

    public void EndLevel()
    {
        foreach (var enemy in enemies)
        {
            enemy.DeInit();
        }
        Utils.ClearTransform(enemiesContainer);
        Utils.ClearTransform(miscContainer);
        Utils.ClearTransform(bulletContainer);
        foreach (var bornPosition in appearPos)
        {
            bornPosition.EndLevel();
        }
        if (boss != null)
        {
            Destroy(boss.gameObject);
        }
    }

    public void DestroyLevel()
    {
        Application.UnloadLevel("Level" + level.MissionIndex);
        Destroy(levelMainObject);
    }

    private void OnEnemyDead(Unit obj)
    {
        obj.OnDead -= OnEnemyDead;
        enemies.Remove(obj as BaseMonster);
        bossSpawner.EnemieDead();
        level.EnemieDead();
    }

    public List<BaseMonster> GetEnimiesInRadius(float rad)
    {
        List < BaseMonster > list = new List<BaseMonster>();
        foreach (var enemy in enemies.Where(x => x is BaseMonster && (x as BaseMonster).IsInRadius(rad)))
        {
            list.Add(enemy as BaseMonster);
        }
        return list;
    } 

    public BaseMonster FindClosesEnemy(Vector3 v)
    {
        float curDist = 999999;
        BaseMonster unit = null;
        foreach (var enemy in enemies.Where(x=>!x.IsDisabled))
        {
            var pDist = (enemy.transform.position - v).sqrMagnitude;
            if (pDist < curDist)
            {
                curDist = pDist;
                unit = enemy;
            }
        }
        return unit;
    }

    public void LeaveEffect(BaseEffectAbsorber ps,Transform oldTransform)
    {
        ps.transform.SetParent(effectsContainer, true);
        if (ps != null && ps.gameObject != null)
        {
            StartCoroutine(ps.DestroyPS(oldTransform,4, "1"));
        }
    }

}

