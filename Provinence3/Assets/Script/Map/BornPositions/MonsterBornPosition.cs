using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MonsterBornPosition : BaseBornPosition
{
    public BaseMonster monsterPrebaf;
    public int difficulty = 1;
    private Level level;
    private int totalUnits;
    private TimerManager.ITimer timer;
    private Action<Unit> OnEnemyDead;
    private Hero hero;
    private bool isReborned = false;
    private List<BaseMonster> activeMonsters = new List<BaseMonster>(); 

    public void Init(Map map, Action<Unit> OnEnemyDead,Level level, Hero hero)
    {
        this.hero = hero;
        isReborned = false;
        this.level = level;
        this.OnEnemyDead = OnEnemyDead;
        difficulty = difficulty + level.difficult - 1;
        base.Init(map);
//        BornMosters();
    }
    public void BornMosters()
    {
        if (work)
        {
            var p = transform.position;
            for (int i = 0; i < unitsCout; i++)
            {
                var b = new Vector3(p.x + UnityEngine.Random.Range(-radius, radius), p.y,
                    p.z + UnityEngine.Random.Range(-radius, radius));
                BornEnemy(b, OnEnemyDead, hero);
            }
        }
    }

    public override BornPositionType GetBornPositionType()
    {
        return BornPositionType.monster;
    }

    public void BornEnemy(Vector3 pos, Action<Unit> OnEnemyDead, Hero hero)
    {
        BaseMonster monster;
        if (monsterPrebaf == null)
        {
            monster = DataBaseController.Instance.mosntersLevel[difficulty].RandomElement();
        }
        else
        {
            monster = monsterPrebaf;
        }
         
        totalUnits = 0;
        if (monster != null)
        {
            var unit = DataBaseController.GetItem(monster, pos);
            map.enemies.Add(unit);
            unit.Init(hero);
            if (UnityEngine.Random.Range(0, 100) < 7)
            {
                unit.Overcharg();
            }
            unit.OnGetHitMonster += OnGetHitMonster;
            unit.transform.SetParent(map.enemiesContainer);
            unit.OnDead += OnEnemyDead;
            unit.OnDead += OnDead;
            totalUnits++;
            activeMonsters.Add(unit);
        }
        else
        {
            Debug.Log("can't dinf monster level " + difficulty);
        }
    }

    private void OnGetHitMonster(BaseMonster monster)
    {
        foreach (var activeMonster in activeMonsters)
        {
            if (activeMonster != monster)
            {
                activeMonster.StartAttack();
            }
        }
    }

    private void OnDead(Unit unit)
    {
        var monster = (unit as BaseMonster);
        activeMonsters.Remove(monster);
        monster.OnGetHitMonster += OnGetHitMonster;
        unit.OnDead -= OnDead;
        totalUnits--;
//        Debug.Log("OnDead  " + totalUnits + "    " + isReborned) ;
        if (totalUnits <= 0)
        {
            StartReborn();
        }
    }

    private void StartReborn()
    {
        if (!isReborned)
        {
            int sec = UnityEngine.Random.Range(60*2, 60*3);
#if UNITY_EDITOR
            if (DebugController.Instance.RESPAWN_TIME_CREEPS_FAST)
            {
                sec = 15;
            }
#endif
            timer = MainController.Instance.TimerManager.MakeTimer(TimeSpan.FromSeconds(sec));
            timer.OnTimer += OnReborn;

            isReborned = true;
        }
    }

    private void OnReborn()
    {
        timer = null;
        if (work)
        {
            BornMosters();
        }
    }

    public void EndLevel()
    {
        if (timer != null)
        {
            timer.Stop();
        }
    }
}
