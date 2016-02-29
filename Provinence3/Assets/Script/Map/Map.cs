﻿using System;
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
    private Transform heroBornPositions;
    private Level level;
    public List<BaseMonster> enemies;
    public CameraFollow CameraFollow;
    private BossUnit boss;
    private GameObject levelMainObject;

    public Hero Init(Level lvl,int heroBornPositionIndex, int levelIndex)
    {
        level = lvl;
        LoadLevelGameObject(levelIndex);
        Application.LoadLevelAdditive("Level" + levelIndex);
        heroBornPositions = levelMainObject.transform.Find("BornPos/HeroBornPositions");
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
        return hero;
    }

    private void LoadLevelGameObject(int levelIndex)
    {
        if (levelMainObject != null)
        {
            Destroy(levelMainObject);
        }

        var prefab = Resources.Load(BASE_WAY + levelIndex,typeof(GameObject)) as GameObject;
        levelMainObject = GameObject.Instantiate(prefab);
        levelMainObject.transform.SetParent(transform);
    }

    private Vector3 GetHeroBoenPos(int index)
    {
        Vector3 vector3s = Vector3.zero;
        var playerData =  MainController.Instance.PlayerData;
       
        foreach (Transform v in heroBornPositions)
        {
            v.GetComponent<MeshRenderer>().enabled = false;
            var heroBP = v.GetComponent<HeroBornPosition>();
            if (heroBP.ID == index)
            {
                vector3s = v.position;
//                break;
            }
            heroBP.Init(this,playerData.OpenLevels.IsPositionOpen(level.MissionIndex,index));
//            vector3s = v.position;
        }
        return vector3s;
    }

    private void OnSpawnBoss()
    {
        var pos = BossAppearPos.RandomElement().transform.position;
        var bossPrefab = DataBaseController.Instance.BossUnits.FirstOrDefault(x => x.Parameters.Level == level.difficult);
        if (bossPrefab != null)
        {
            boss = DataBaseController.GetItem<BossUnit>(bossPrefab, pos);
            var hero = MainController.Instance.level.MainHero;
            boss.Init(hero);
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
        foreach (var bornPosition in appearPos)
        {
            bornPosition.EndLevel();
        }
        if (boss != null)
        {
            Destroy(boss.gameObject);
        }
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
        foreach (var enemy in enemies)
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

    public void LeaveEffect(BaseEffectAbsorber ps)
    {
        ps.transform.SetParent(effectsContainer, true);
        ps.DestroyPS(4);
    }

}

