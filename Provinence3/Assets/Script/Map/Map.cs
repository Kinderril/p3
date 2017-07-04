using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Map : Singleton<Map>
{
    private const string BASE_WAY = "prefabs/level/Level";
    public List<MonsterBornPosition> MonsterBornPositions;
    public List<BossBornPosition> BossAppearPos;
    private Transform bornPositions;
    public Transform enemiesContainer;
    public BossSpawner BossSpawner;
    public Transform effectsContainer;
    public Transform miscContainer;
    public Transform bulletContainer;
    private Transform heroBornPositions;
    private Transform bossBonusMapElement;
    private Level level;
    public List<BaseMonster> enemies;
    public CameraFollow CameraFollow;
    private BossUnit boss;
    private LevelObject levelMainObject;
    private Action OnMonstersReady;
//    private Dictionary<string, GameObject> LoadedLevels = new Dictionary<string, GameObject>();
    public event Action<Unit> OnEnemyDeadCallback;
    private string allInfo;

    public Hero Init(Level lvl, LevelObject levelObject, int heroBornPositionIndex)
    {
        allInfo = "";
        TimeUtils.StartMeasure("LOAD LEVEL SCENE");
        level = lvl;
        levelMainObject = levelObject;
        heroBornPositions = levelMainObject.transform.Find("BornPos/HeroBornPositions");
        bossBonusMapElement = levelMainObject.transform.Find("BornPos/BossBonusMapElements");
        bornPositions = levelMainObject.transform.Find("BornPos");
        enemiesContainer = transform.Find("Enemies");

        allInfo += TimeUtils.EndMeasure("LOAD LEVEL SCENE");
        TimeUtils.StartMeasure("LOAD HERO");
        DebugController.Instance.InfoField2.text = allInfo;

        var hero = DataBaseController.GetItem(DataBaseController.Instance.prefabHero, GetHeroBoenPos(heroBornPositionIndex));
        hero.Init(lvl);
        enemies = new List<BaseMonster>();
        MonsterBornPositions = new List<MonsterBornPosition>();
        BossAppearPos = new List<BossBornPosition>();
        levelMainObject.Init(hero);
        List<ChestBornPosition> chestPositions = new List<ChestBornPosition>();

        allInfo += TimeUtils.EndMeasure("LOAD HERO");
        TimeUtils.StartMeasure("PARSE BORN");
        DebugController.Instance.InfoField2.text = allInfo;
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
                        MonsterBornPositions.Add(mBP);
//                        mBP.Init(this, OnEnemyDead, lvl, hero);
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

        allInfo += TimeUtils.EndMeasure("PARSE BORN");
        TimeUtils.StartMeasure("LOAD QUESTS");
        DebugController.Instance.InfoField2.text = allInfo;

        var questsPositions = MonsterBornPositions.RandomElement(levelObject.QuestCount);
        LoadQuests(questsPositions);


        allInfo += TimeUtils.EndMeasure("LOAD QUESTS");
        TimeUtils.StartMeasure("LOAD MONSTERS");
        DebugController.Instance.InfoField2.text = allInfo;

        foreach (var monsterBornPosition in MonsterBornPositions)
        {
            monsterBornPosition.Init(this, OnEnemyDead, lvl, hero);
        }

        allInfo += TimeUtils.EndMeasure("LOAD MONSTERS");
        TimeUtils.StartMeasure("LOAD LAST");
        DebugController.Instance.InfoField2.text = allInfo;

        int cnt = (int)(level.CrystalsBonus);
        var rnd = chestPositions.RandomElement(cnt);
        foreach (var chestBornPosition in rnd)
        {
            chestBornPosition.SetCrystal();
        }
        foreach (var chestBornPosition in chestPositions)
        {
            chestBornPosition.Init(this,lvl);
        }
        CameraFollow.Init(hero);

        allInfo += TimeUtils.EndMeasure("LOAD LAST");
        DebugController.Instance.InfoField2.text = allInfo;

        return hero;
    }

    private void LoadQuests(List<MonsterBornPosition> questsPositions)
    {
        foreach (var bornPosition in questsPositions)
        {
            var giver = DataBaseController.GetItem(DataBaseController.Instance.QuestGiverPrefab, bornPosition.transform.position);
            level.AddQuestGiver(giver);
        }
    }

    public void LoadBoss()
    {

        BossSpawner = new BossSpawner(enemies.Count, OnSpawnBoss,levelMainObject.PrecentMonsterToBoss_0_1);
        if (bossBonusMapElement != null)
        {
            foreach (Transform tr in bossBonusMapElement)
            {
                var bonusBoss = tr.GetComponent<BossBonusMapElement>();
                bonusBoss.Init(BossSpawner);
            }
        }
    }


    public void StartLoadingMonsters()
    {
        StartCoroutine(Loading());
    }

    private IEnumerator Loading()
    {
        int curCount = 0;
        foreach (var monsterBornPosition in MonsterBornPositions)
        {
            yield return new WaitForEndOfFrame();
            monsterBornPosition.BornMosters();
            DebugController.Instance.InfoField1.text = "Count:"+enemies.Count.ToString();
        }
        LoadBoss();
        if (OnMonstersReady != null)
            OnMonstersReady();
    }

//    private void LoadLevelGameObject(LevelObject levelIndex)
//    {
//        TimeUtils.StartMeasure("RES LOAD");
//        if (levelMainObject != null)
//        {
//            Debug.LogError("Level isn't unloaded!!!");
//            Destroy(levelMainObject.gameObject);
//        }
//        //        Resources.LoadAsync(BASE_WAY + levelIndex, typeof(GameObject));
////        var nameLevel = BASE_WAY + levelIndex;
//
//        GameObject prefab;
////        if (LoadedLevels.ContainsKey(nameLevel))
////        {
////            prefab = LoadedLevels[nameLevel];
////        }
////        else
////        {
////            prefab = Resources.Load(nameLevel, typeof(GameObject)) as GameObject;
////            LoadedLevels.Add(nameLevel,prefab);
////        }
//
//        allInfo += TimeUtils.EndMeasure("RES LOAD");
//        DebugController.Instance.InfoField2.text = allInfo;
//        TimeUtils.StartMeasure("Instantiate SCENE");
////        levelMainObject = levelIndex;// GameObject.Instantiate(prefab).GetComponent<LevelObject>();
////        levelMainObject.transform.SetParent(transform);
//        allInfo += TimeUtils.EndMeasure("Instantiate SCENE");
//        DebugController.Instance.InfoField2.text = allInfo;

//    }

    void Update()
    {
        if (level == null || level.MainHero == null || level.IsPause || !level.IsPlaying)
            return;
        var mainHero = level.MainHero;
        float mainHeroDist;
        bool isActive;
        foreach (var baseMonster in enemies)
        {
            if (baseMonster != null && !baseMonster.IsDead && !baseMonster.IsDisabled)
            {
                mainHeroDist = (mainHero.transform.position - baseMonster.transform.position).sqrMagnitude;
                isActive = mainHeroDist < BaseMonster.AI_DIST;
                baseMonster.gameObject.SetActive(isActive);
                baseMonster.SetDistance(mainHeroDist);
                if (isActive)
                {
                    baseMonster.CheckDistance();
                    baseMonster.UpdateByManager();
                }
            }
        }
        if (boss != null)
        {
            boss.CheckArrow();
        }
        levelMainObject.UpdateByMap();
    }

    private Vector3 GetHeroBoenPos(int index)
    {
        Vector3 vector3s = Vector3.zero;
        var playerData =  MainController.Instance.PlayerData;
        List<int> opensRespawnPoints = MainController.Instance.PlayerData.OpenLevels.GetAllBornPositions(level.MissionIndex);

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
            var opend = opensRespawnPoints.Contains(heroBP.ID);
            Debug.Log(heroBP.ID + " ... " + opend);
            heroBP.Init(this, opend);
//            vector3s = v.position;
        }
        return vector3s;
    }

    public void SpawnBoss(int bonuses)
    {
        Vector3 pos = default(Vector3);
        float curDist = 9999999;
        foreach (var bossBornPosition in BossAppearPos)
        {
            var tmpDist = (bossBornPosition.transform.position - level.MainHero.transform.position).sqrMagnitude;
            if (curDist > tmpDist)
            {
                curDist = tmpDist;
                pos = bossBornPosition.transform.position;
            }
        }

        var bossPrefab = DataBaseController.Instance.BossUnits.RandomElement();
        level.LevelStatistics.BossName = bossPrefab.BossName;
        if (bossPrefab != null)
        {
            CameraFollow.CameraShake.Init(0.5f);
            boss = DataBaseController.GetItem<BossUnit>(bossPrefab, pos);
            boss.ModificateParams(level.difficult);
            if (level.IsTutor)
            {
                boss.Parameters.SetAbsolute(ParamType.PPower, 10);
                boss.Parameters.SetAbsolute(ParamType.MPower, 10);
                boss.Parameters.SetAbsolute(ParamType.PDef, 1);
                boss.Parameters.SetAbsolute(ParamType.MDef, 1);
            }
            var hero = MainController.Instance.level.MainHero;
            boss.Init(hero);
            enemies.Add(boss);
            var resultBossHP = Formuls.ModifyBossHP(boss, bonuses);
            boss.SetHp(resultBossHP);
            boss.Parameters.SetAbsolute(ParamType.Heath, resultBossHP);
            hero.ArrowTarget.Init(boss);
            boss.transform.SetParent(enemiesContainer);
            MainController.Instance.level.BigMessageAppear("Boss have appear", "", Color.red);
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

    private void OnSpawnBoss(int bonuses)
    {
        SpawnBoss(bonuses);
    }

    public void EndLevel()
    {
        foreach (var enemy in enemies)
        {
            enemy.DeInit();
        }
        MainController.Instance.level.MainHero.Dispose();
        Utils.ClearTransform(enemiesContainer);
        Utils.ClearTransform(miscContainer);
        Utils.ClearTransform(bulletContainer);
        foreach (var bornPosition in MonsterBornPositions)
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
        SceneManager.UnloadScene("Level" + level.MissionIndex);
        if (levelMainObject != null)
            Destroy(levelMainObject.gameObject);
    }

    private void OnEnemyDead(Unit obj)
    {
        if (OnEnemyDeadCallback != null)
        {
            OnEnemyDeadCallback(obj);
        }
        obj.OnDead -= OnEnemyDead;
        enemies.Remove(obj as BaseMonster);
        BossSpawner.EnemieDead();
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

    public BaseMonster FindClosesEnemy(Vector3 v,float maxSqrDist = 9999,List<Unit> rejected = null)
    {
        float curDist = 9999;
        BaseMonster unit = null;
        IEnumerable<BaseMonster> pTargets;
        if (rejected != null)
        {
            pTargets = enemies.Where(x => x.gameObject.activeSelf && !rejected.Contains(x));
        }
        else
        {
            pTargets = enemies.Where(x => x.gameObject.activeSelf);
        }
        foreach (var enemy in pTargets)
        {
            var pDist = (enemy.transform.position - v).sqrMagnitude;
//            Debug.Log("Dist:" + pDist);
            if (pDist < curDist && pDist < maxSqrDist)
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

    public void SetCallBackMonstersReady(Action OnMonstersReady)
    {
        this.OnMonstersReady = OnMonstersReady;
    }
}

